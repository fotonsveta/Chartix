using System;
using System.Threading.Tasks;
using Chartix.Core.Entities;
using Chartix.Core.Interfaces;
using Chartix.Infrastructure.Telegram.Interfaces;
using Chartix.Infrastructure.Telegram.Models;

namespace Chartix.Infrastructure.Telegram.ButtonCommands
{
    public class AddMetricButtonCommand : ButtonCommand
    {
        private readonly IBotClient _botClient;
        private readonly ISourceRepository _sourceRepository;

        public AddMetricButtonCommand(IBotClient botClient, ISourceRepository sourceRepository)
        {
            _botClient = botClient;
            _sourceRepository = sourceRepository;
        }

        public override ButtonCode Button => ButtonCode.AddMetric;

        public override async Task HandleAsync(CallbackQueryUpdateMessage message)
        {
            Source source = _sourceRepository.GetByExternalId(message.ChatId)
                ?? throw new ArgumentException($"Can't perfom {Button} command, there is no source with id {message.ChatId}");
            source.UpdateState(StateType.NeedAddMetric);
            _sourceRepository.Update(source);

            await _botClient.SendTextMessageAsync(message, MessageCode.EnterMainMetric);
        }
    }
}
