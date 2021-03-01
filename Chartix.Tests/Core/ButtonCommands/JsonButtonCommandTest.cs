using System.Linq;
using System.Threading.Tasks;
using Chartix.Core.Interfaces;
using Chartix.Core.Models;
using Chartix.Infrastructure.Telegram.ButtonCommands;
using Chartix.Infrastructure.Telegram.Interfaces;
using Chartix.Infrastructure.Telegram.Models;
using Chartix.Infrastructure.Telegram.Services;
using Chartix.Tests.Core.Bot;
using Chartix.Tests.Order;
using Moq;
using Xunit;

namespace Chartix.Tests.Core.ButtonCommands
{
    [TestCaseOrderer("Chartix.Tests.Order.PriorityOrderer", "OrderedTest")]
    public class JsonButtonCommandTest
    {
        public JsonButtonCommandTest()
        {
        }

        [Theory]
        [TestPriority(5)]
        [InlineData(111)]
        [InlineData(222)]
        public async Task ToJsonButtonCommand_HandleAsync_Success(int updateId)
        {
            var message = new CallbackQueryUpdateMessage { ChatId = 1, UpdateId = updateId };

            var botResponseMock = new Mock<FakeBotResponse>();
            botResponseMock.Setup(x => x.Response(It.IsAny<string>()));
            IBotClient botClient = new FakeBotClient(botResponseMock.Object);

            var metricRepoMock = new Mock<IMetricRepository>();
            var testData = new TestData();
            var metrics = testData.GetMetrics(updateId);
            metricRepoMock.Setup(x => x.GetBySourceIdWithValues(It.IsAny<long>())).Returns(metrics);

            var sourceRepoMock = new Mock<ISourceRepository>();
            sourceRepoMock.Setup(x => x.GetByExternalId(It.IsAny<long>())).Returns(testData.Source);

            var command = new ToJsonButtonCommand(botClient, metricRepoMock.Object, sourceRepoMock.Object);
            await command.HandleAsync(message);

            var filename = FileName.GetJsonFilename(updateId.ToString());
            botResponseMock.Verify(x => x.Response(filename));
        }

        [Theory]
        [TestPriority(1)]
        [InlineData(111)]
        [InlineData(222)]
        public async Task ParseAsync_Success(int updateId)
        {
            IBotClient botClient = new FakeBotClient(new FakeBotResponse());
            var parser = new MetricsJsonParser(botClient);

            var filename = FileName.GetJsonFilename(updateId.ToString());
            var newMetrics = await parser.ParseAsync(filename);

            var testMetrics = new TestData().GetMetrics(updateId);
            Assert.NotNull(newMetrics);
            Assert.Equal(newMetrics.Count(), testMetrics.Count());
            foreach (var newMetric in newMetrics)
            {
                var testMetric = testMetrics.FirstOrDefault(x => x.Name == newMetric.Name && x.Unit == newMetric.Unit);
                Assert.NotNull(testMetric);
                Assert.Equal(newMetric.Values.Count(), testMetric.Values.Count());
                foreach (var value in newMetric.Values)
                {
                    Assert.NotNull(testMetric.Values.FirstOrDefault(x =>
                        x.Content == value.Content && x.ValueDate == value.ValueDate));
                }
            }
        }
    }
}
