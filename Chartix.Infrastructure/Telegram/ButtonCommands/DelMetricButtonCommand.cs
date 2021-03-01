using System;
using System.Threading.Tasks;
using Chartix.Core.Entities;
using Chartix.Core.Interfaces;
using Chartix.Infrastructure.Telegram.Interfaces;
using Chartix.Infrastructure.Telegram.Models;

namespace Chartix.Infrastructure.Telegram.ButtonCommands
{
    public class DelMetricButtonCommand : ButtonCommand
    {
        private readonly IBotClient _botClient;
        private readonly ISourceRepository _sourceRepository;
        private readonly IMetricRepository _metricRepository;

        public DelMetricButtonCommand(IBotClient botClient, ISourceRepository sourceRepository, IMetricRepository metricRepository)
        {
            _botClient = botClient;
            _sourceRepository = sourceRepository;
            _metricRepository = metricRepository;
        }

        public override ButtonCode Button => ButtonCode.DelMetric;

        public override async Task HandleAsync(CallbackQueryUpdateMessage message)
        {
            if (!long.TryParse(Parameter, out long metricId))
            {
                throw new ArgumentException($"Can't perfom {Button} command, id parameter is not a number - {Parameter}");
            }

            var source = _sourceRepository.GetByExternalId(message.ChatId)
                ?? throw new ArgumentException($"Can't perfom {Button} command, there is no source with id {message.ChatId}");

            var metric = _metricRepository.GetById(metricId)
                ?? throw new ArgumentException($"Can't perfom {Button} command, there is no metric with id {metricId}");

            metric.SetDeleted();
            _metricRepository.Update(metric);

            if (metric.IsMain)
            {
                // after removed the main metric, set any other main
                var hasMainMain = _metricRepository.SetMainMetricIfHasNo(source.Id);
                source.UpdateState(hasMainMain ? StateType.HasMetric : StateType.NeedAddMetric);
                _sourceRepository.Update(source);
            }

            await _botClient.SendTextMessageAsync(message, MessageCode.Done);
        }
    }
}
