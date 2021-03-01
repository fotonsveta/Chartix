using System;
using Chartix.Core.Models;
using Chartix.Infrastructure.Interfaces;
using Chartix.Infrastructure.Telegram.Models;

namespace Chartix.Infrastructure.Telegram.Services
{
    public class ScottPlotService : IFileService
    {
        private readonly PngFileName _pngfilename;
        private readonly PlotData _data;

        public ScottPlotService(PngFileName filename, PlotData data)
        {
            _pngfilename = filename ?? throw new ArgumentNullException(nameof(filename));
            _data = data ?? throw new ArgumentNullException(nameof(data));

            // !Note: Ticks and SaveFig riase exception "Parameter is not valid.", if XValues and YValues has one value
            if (_data.XValues == null || _data.XValues.Length < 2)
            {
                throw new ArgumentException("ScottPlot for XValues requires at least one value");
            }

            if (_data.YValues == null || _data.YValues.Length < 2)
            {
                throw new ArgumentException("ScottPlot for YValues requires at least one value");
            }
        }

        public string Filename { get => _pngfilename.Name; }

        /// <summary>
        /// Create file in current directory, path is filename.
        /// </summary>
        public void CreateLocalFile()
        {
            var plt = new ScottPlot.Plot(800, 600);

            plt.PlotScatter(_data.XValues, _data.YValues);
            plt.Ticks(dateTimeX: true);
            plt.Title(_data.Title);

            plt.SaveFig(_pngfilename.Name);
        }
    }
}
