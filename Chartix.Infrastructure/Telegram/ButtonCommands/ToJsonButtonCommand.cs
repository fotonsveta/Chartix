using System;
using System.Linq;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;
using System.Threading.Tasks;
using Chartix.Core.Interfaces;
using Chartix.Core.Models;
using Chartix.Infrastructure.Telegram.Interfaces;
using Chartix.Infrastructure.Telegram.Models;

namespace Chartix.Infrastructure.Telegram.ButtonCommands
{
    public class ToJsonButtonCommand : ButtonCommand
    {
        private readonly IBotClient _botClient;
        private readonly IMetricRepository _metricRepository;
        private readonly ISourceRepository _sourceRepository;

        public ToJsonButtonCommand(IBotClient botClient, IMetricRepository metricRepository, ISourceRepository sourceRepository)
        {
            _botClient = botClient;
            _metricRepository = metricRepository;
            _sourceRepository = sourceRepository;
        }

        public override ButtonCode Button => ButtonCode.ToJson;

        public override async Task HandleAsync(CallbackQueryUpdateMessage message)
        {
            var source = _sourceRepository.GetByExternalId(message.ChatId)
                ?? throw new ArgumentException($"Can't perfom {Button} command, there is no source with id {message.ChatId}");

            var metrics = _metricRepository.GetBySourceIdWithValues(source.Id);
            if (metrics == null || !metrics.Any())
            {
                await _botClient.SendTextMessageAsync(message, MessageCode.NoMetrics);
                return;
            }

            var options = new JsonSerializerOptions()
            {
                WriteIndented = true,
                Encoder = JavaScriptEncoder.Create(UnicodeRanges.All),
            };

            var bytes = JsonSerializer.SerializeToUtf8Bytes(metrics, options);
            var filename = FileName.GetJsonFilename(message.UpdateId.ToString());
            await _botClient.SendFileAsync(message.ChatId, bytes, filename);
        }
    }
}
