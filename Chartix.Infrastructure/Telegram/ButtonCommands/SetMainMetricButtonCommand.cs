using System;
using System.Threading.Tasks;
using Chartix.Core.Interfaces;
using Chartix.Infrastructure.Telegram.Interfaces;
using Chartix.Infrastructure.Telegram.Models;

namespace Chartix.Infrastructure.Telegram.ButtonCommands
{
    public class SetMainMetricButtonCommand : ButtonCommand
    {
        private readonly IBotClient _botClient;
        private readonly IMetricRepository _metricRepository;

        public SetMainMetricButtonCommand(IBotClient botClient, IMetricRepository metricRepository)
        {
            _botClient = botClient;
            _metricRepository = metricRepository;
        }

        public override ButtonCode Button => ButtonCode.SetMainMetric;

        public override async Task HandleAsync(CallbackQueryUpdateMessage message)
        {
            if (!long.TryParse(Parameter, out long id))
            {
                throw new ArgumentException($"Can't perfom {Button} command, id parameter is not a number - {Parameter}");
            }

            var newMainMetric = _metricRepository.GetById(id)
                ?? throw new ArgumentException($"Can't perfom {Button} command, there is no metric with id {id}");

            var oldMainMetric = _metricRepository.GetMainByExternalId(message.ChatId);
            if (oldMainMetric != null && oldMainMetric.Id != newMainMetric.Id)
            {
                oldMainMetric.UpdateMain(false);
                _metricRepository.Update(oldMainMetric);

                newMainMetric.UpdateMain(true);
                _metricRepository.Update(newMainMetric);
            }

            await _botClient.SendTextMessageAsync(message, MessageCode.Done);
        }
    }
}
