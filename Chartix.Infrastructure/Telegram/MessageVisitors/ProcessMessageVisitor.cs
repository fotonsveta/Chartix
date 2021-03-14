using System;
using System.Threading.Tasks;
using Chartix.Core.Entities;
using Chartix.Core.Interfaces;
using Chartix.Infrastructure.Telegram.ButtonCommands;
using Chartix.Infrastructure.Telegram.Commands;
using Chartix.Infrastructure.Telegram.Interfaces;
using Chartix.Infrastructure.Telegram.Models;
using Chartix.Infrastructure.Telegram.Services;

namespace Chartix.Infrastructure.Telegram.MessageVisitors
{
    public class ProcessMessageVisitor : IProcessMessageVisitor
    {
        private readonly IBotClient _botClient;
        private readonly ICommandStrategyFactory<ICommand> _commands;
        private readonly ISourceRepository _sourceRepository;
        private readonly ICommandStrategyFactory<IButtonCommand> _buttonCommands;
        private readonly IValueHandlerService _valueHandlerService;
        private readonly IMetricRepository _metricRepository;

        public ProcessMessageVisitor(
                                    IBotClient botClient,
                                    ICommandStrategyFactory<ICommand> commands,
                                    ISourceRepository sourceRepository,
                                    ICommandStrategyFactory<IButtonCommand> buttonCommands,
                                    IValueHandlerService valueHandlerService,
                                    IMetricRepository metricRepository)
        {
            _botClient = botClient;
            _commands = commands;
            _sourceRepository = sourceRepository;
            _buttonCommands = buttonCommands;
            _valueHandlerService = valueHandlerService;
            _metricRepository = metricRepository;
        }

        public static MessageCode GetMessageCode(StateType state)
        {
            var messageCode = state == StateType.NeedAddUnit ? MessageCode.EnterMainUnit : MessageCode.EnterMainMetric;
            return state == StateType.HasMetric ? MessageCode.Done : messageCode;
        }

        public async Task Process(UpdateMessage message)
        {
            await message.Accept(this);
        }

        public async Task Visit(TextUpdateMessage message)
        {
            var source = GetSource(message);
            source.UpdateLastActionDate();

            // Need to check the user's state to understand how to process text
            if (source.State == StateType.HasMetric)
            {
                // Processing the value for the main metric
                await _valueHandlerService.HandleAsync(message);
                _sourceRepository.Update(source);
                return;
            }
            else if (source.State == StateType.NeedAddMetric)
            {
                // Create new metric with name
                var newMetric = new Metric(message.Content);
                newMetric.UpdateSource(source);
                var mainMetric = _metricRepository.GetMainBySourceId(source.Id);
                newMetric.UpdateMain(mainMetric == null);
                _metricRepository.Add(newMetric);
                source.UpdateState(StateType.NeedAddUnit);
            }
            else if (source.State == StateType.NeedAddUnit)
            {
                // Adding metric unit for existing metric
                var editedMetric = _metricRepository.GetNotCreatedMetric(source.Id)
                    ?? throw new ArgumentException($"Can't add main unit, there is no edited metric for source {message.ChatId}");

                editedMetric.UpdateUnit(message.Content);
                editedMetric.SetCreated();
                _metricRepository.Update(editedMetric);
                source.UpdateState(StateType.HasMetric);
            }

            _sourceRepository.Update(source);
            await _botClient.SendTextMessageAsync(message, GetMessageCode(source.State));
        }

        public async Task Visit(CommandUpdateMessage message)
        {
            var source = GetSource(message);
            source.UpdateLastActionDate();
            _sourceRepository.Update(source);

            if (source.State == StateType.HasMetric)
            {
                var command = _commands.GetCommand(message.Content);
                await command.HandleAsync(message);
                return;
            }

            await _botClient.SendTextMessageAsync(message, GetMessageCode(source.State));
        }

        public async Task Visit(DocumentUpdateMessage message)
        {
            var source = GetSource(message);

            string jsonMimeType = "application/json";
            if (message.DocumentMimeType != jsonMimeType)
            {
                await _botClient.SendTextMessageAsync(message, MessageCode.ExpectedJsonFile);
                return;
            }

            try
            {
                var parser = new MetricsJsonParser(_botClient);
                var newMetrics = await parser.ParseAsync(message.DocumentFileId);
                _metricRepository.UpdateMetricData(source, newMetrics);
                await _botClient.SendTextMessageAsync(message, MessageCode.Done);
            }
            catch
            {
                await _botClient.SendTextMessageAsync(message, MessageCode.JsonError);
                throw;
            }

            // If there is no main metric, set any other
            var hasMainMetric = _metricRepository.SetMainMetricIfHasNo(source.Id);
            source.UpdateState(hasMainMetric ? StateType.HasMetric : StateType.NeedAddMetric);
            source.UpdateLastActionDate();
            _sourceRepository.Update(source);
        }

        public async Task Visit(CallbackQueryUpdateMessage message)
        {
            // This is the result of selecting a menu button
            var buttonCommand = _buttonCommands.GetCommand(message.Content);
            await buttonCommand.HandleAsync(message);
        }

        private Source GetSource(UpdateMessage message)
        {
            return _sourceRepository.FindOrCreateNew(message.ChatId, message.Username)
                ?? throw new ArgumentException($"Error, there is no source with id {message.ChatId}");
        }
    }
}
