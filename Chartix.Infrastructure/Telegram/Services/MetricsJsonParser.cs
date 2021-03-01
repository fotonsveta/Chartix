using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;
using System.Threading.Tasks;
using Chartix.Core.Entities;
using Chartix.Core.Models;
using Chartix.Infrastructure.Telegram.Interfaces;

namespace Chartix.Infrastructure.Telegram.Services
{
    public class MetricsJsonParser
    {
        private readonly IBotClient _botClient;

        public MetricsJsonParser(IBotClient botClient)
        {
            _botClient = botClient;
        }

        public async Task<IEnumerable<Metric>> ParseAsync(string fileId)
        {
            var options = new JsonSerializerOptions()
            {
                Encoder = JavaScriptEncoder.Create(UnicodeRanges.All),
            };

            ICollection<JsonMetric> jsonMetrics;
            using (var stream = new MemoryStream())
            {
                await _botClient.DownloadFileAsync(fileId, stream);
                stream.Position = 0;
                jsonMetrics = await JsonSerializer.DeserializeAsync<ICollection<JsonMetric>>(stream, options);
            }

            return jsonMetrics.Select(x => x.ConvertTo());
        }
    }
}
