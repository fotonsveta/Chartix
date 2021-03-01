using System.Linq;
using System.Threading.Tasks;
using Chartix.Core.Interfaces;
using Chartix.Core.Models;
using Chartix.Infrastructure.Interfaces;
using Chartix.Infrastructure.Services;
using Chartix.Infrastructure.Telegram.Interfaces;
using Chartix.Infrastructure.Telegram.Models;
using Chartix.Infrastructure.Telegram.Services;

namespace Chartix.Infrastructure.Telegram.ButtonCommands
{
    public class PlotButtonCommand : ButtonCommand
    {
        private readonly IBotClient _botClient;
        private readonly IMetricRepository _metricRepository;
        private readonly IValueRepository _valueRepository;

        public PlotButtonCommand(IBotClient botClient, IMetricRepository metricRepository, IValueRepository valueRepository)
        {
            _botClient = botClient;
            _metricRepository = metricRepository;
            _valueRepository = valueRepository;
        }

        public override ButtonCode Button => ButtonCode.Plot;

        public override async Task HandleAsync(CallbackQueryUpdateMessage message)
        {
            var mainMetric = _metricRepository.GetMainByExternalId(message.ChatId);
            if (mainMetric == null)
            {
                await _botClient.SendTextMessageAsync(message, MessageCode.NoMainMetric);
                return;
            }

            var values = _valueRepository.GetByMetricId(mainMetric.Id, 1, int.MaxValue);
            if (values.Count() < 2)
            {
                await _botClient.SendTextMessageAsync(message, MessageCode.ToFewValues);
                return;
            }

            var plotData = new PlotDataFactory().ValuesTo(values, mainMetric.ToString());
            var filename = FileName.GetPlotFilename(message.UpdateId.ToString());
            IFileService fileService = new ScottPlotService(new PngFileName(filename), plotData);

            using (var plotService = new FileReaderService(fileService))
            {
                var stream = plotService.GetFileStream();
                await _botClient.SendFileAsync(message.ChatId, stream, filename);
            }
        }
    }
}
