using System;
using System.Collections.ObjectModel;
using System.IO;
using Chartix.Core.Entities;
using Chartix.Core.Models;
using Chartix.Infrastructure.Interfaces;
using Chartix.Infrastructure.Services;
using Chartix.Infrastructure.Telegram.Services;
using Xunit;

namespace Chartix.Tests.Core.Services
{
    public class ScottPlotServiceTest
    {
        public ScottPlotServiceTest()
        {
        }

        [Fact]
        public void CreatePlotWith_NonePoint()
        {
            var values = new Collection<Value>();
            var filename = FileName.GetPlotFilename("_nonepoint");

            Assert.Throws<ArgumentException>(() => CreatePlotWith(filename, values));
        }

        [Fact]
        public void CreatePlotWith_OnePoint()
        {
            var values = new Collection<Value>()
            {
                new Value(100, new DateTime(2020, 5, 1)),
            };

            var filename = FileName.GetPlotFilename("_onepoint");
            Assert.Throws<ArgumentException>(() => CreatePlotWith(filename, values));
        }

        [Fact]
        public void CreatePlotWith_TwoPoint()
        {
            var values = new Collection<Value>()
            {
                new Value(100, new DateTime(2020, 5, 1)),
                new Value(110, new DateTime(2020, 5, 23)),
            };

            var filename = FileName.GetPlotFilename("_twopoint");
            var bytes = CreatePlotWith(filename, values);
            var exist = File.Exists(filename);

            Assert.NotNull(bytes);
            Assert.True(exist);
        }

        [Fact]
        public void CreatePlotWith_TenPoint()
        {
            var values = new Collection<Value>()
            {
                new Value(100, new DateTime(2000, 5, 1)),
                new Value(110, new DateTime(2001, 5, 1)),
                new Value(113, new DateTime(2002, 5, 1)),
                new Value(117, new DateTime(2003, 5, 1)),
                new Value(121, new DateTime(2004, 5, 1)),
                new Value(131, new DateTime(2005, 5, 1)),
                new Value(134, new DateTime(2006, 5, 1)),
                new Value(139, new DateTime(2007, 5, 1)),
                new Value(141, new DateTime(2008, 5, 1)),
                new Value(150, new DateTime(2009, 5, 1)),
            };

            var filename = FileName.GetPlotFilename("_tenpoint");
            var bytes = CreatePlotWith(filename, values);

            var exist = File.Exists(filename);

            Assert.NotNull(bytes);
            Assert.True(exist);
        }

        private byte[] CreatePlotWith(string filename, Collection<Value> values)
        {
            var title = "Height (cm)";
            var plotData = new PlotDataFactory().ValuesTo(values, title);

            IFileService fileService = new ScottPlotService(new PngFileName(filename), plotData);
            var plotService = new FileReaderService(fileService);
            return plotService.CreateFile();
        }
    }
}
