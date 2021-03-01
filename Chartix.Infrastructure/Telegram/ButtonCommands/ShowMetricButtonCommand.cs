using System.Threading.Tasks;
using Chartix.Core.Interfaces;
using Chartix.Infrastructure.Telegram.Interfaces;
using Chartix.Infrastructure.Telegram.Models;

namespace Chartix.Infrastructure.Telegram.ButtonCommands
{
    public class ShowMetricButtonCommand : ButtonCommand
    {
        private readonly IBotClient _botClient;
        private readonly IMetricRepository _metricRepository;

        public ShowMetricButtonCommand(IBotClient botClient, IMetricRepository metricRepository)
        {
            _botClient = botClient;
            _metricRepository = metricRepository;
        }

        public override ButtonCode Button => ButtonCode.ShowMetric;

        public override async Task HandleAsync(CallbackQueryUpdateMessage message)
        {
            var mainMetric = _metricRepository.GetMainByExternalId(message.ChatId);
            if (mainMetric != null)
            {
                var metricText = mainMetric.ToString();
                await _botClient.SendTextMessageAsync(message, metricText);
            }
            else
            {
                await _botClient.SendTextMessageAsync(message, MessageCode.NoMainMetric);
            }
        }
    }
}
