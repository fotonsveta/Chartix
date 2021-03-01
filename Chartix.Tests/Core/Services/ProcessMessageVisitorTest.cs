using System.Threading.Tasks;
using Chartix.Core.Entities;
using Chartix.Core.Interfaces;
using Chartix.Infrastructure.Telegram.Interfaces;
using Chartix.Infrastructure.Telegram.MessageVisitors;
using Chartix.Infrastructure.Telegram.Models;
using Chartix.Infrastructure.Telegram.Services;
using Chartix.Tests.Core.Bot;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Chartix.Tests.Core.Services
{
    public class ProcessMessageVisitorTest
    {
        public ProcessMessageVisitorTest()
        {
            BotResponseMock = new Mock<FakeBotResponse>();
            BotResponseMock.Setup(x => x.Response(It.IsAny<string>()));
            IBotClient botClient = new FakeBotClient(BotResponseMock.Object);

            SourceRepoMock = new Mock<ISourceRepository>();
            ValueRepoMock = new Mock<IValueRepository>();
            MetricRepoMock = new Mock<IMetricRepository>();

            var valueHandlerLoggerMock = new Mock<ILogger<ValueHandlerService>>();
            var valueHandlerService = new ValueHandlerService(
                valueHandlerLoggerMock.Object,
                botClient,
                MetricRepoMock.Object,
                ValueRepoMock.Object);

            var helper = new ServiceHelper();
            var commandFactory = helper.CreateCSF(botClient);
            var buttonCommandFactory = helper.CreateButtonCSF(
                botClient,
                SourceRepoMock.Object,
                MetricRepoMock.Object,
                ValueRepoMock.Object);

            MessageProcessor = new ProcessMessageVisitor(
                botClient,
                commandFactory,
                SourceRepoMock.Object,
                buttonCommandFactory,
                valueHandlerService,
                MetricRepoMock.Object);
        }

        private Mock<FakeBotResponse> BotResponseMock { get; }

        private Mock<ISourceRepository> SourceRepoMock { get; }

        private Mock<IValueRepository> ValueRepoMock { get; }

        private Mock<IMetricRepository> MetricRepoMock { get; }

        private IProcessMessageVisitor MessageProcessor { get; }

        [Theory]
        [InlineData(LangCode.Ru, @"start", StateType.NeedAddMetric, MessageCode.EnterMainMetric)]
        [InlineData(LangCode.En, @"about", StateType.NeedAddMetric, MessageCode.EnterMainMetric)]
        [InlineData(LangCode.Ru, @"help", StateType.NeedAddUnit, MessageCode.EnterMainUnit)]
        [InlineData(LangCode.En, @"menu", StateType.NeedAddUnit, MessageCode.EnterMainUnit)]
        [InlineData(LangCode.Ru, @"notcommand", StateType.NeedAddMetric, MessageCode.EnterMainMetric)]
        public async Task Process_CommandMessage_SourceHasNotMetric(LangCode langCode, string content, StateType sourceState,
            MessageCode returnCode)
        {
            int externalId = 1;
            string name = "Koko";
            var source = new Source(externalId, name);
            source.UpdateState(sourceState);
            SourceRepoMock.Setup(r => r.FindOrCreateNew(externalId, name)).Returns(() => source);

            var message = new CommandUpdateMessage
            {
                ChatId = externalId,
                Username = name,
                Content = content,
                LanguageCode = langCode,
            };

            await message.Accept(MessageProcessor);

            var localizer = new Localizer();
            var text = localizer.GetMessage(langCode, returnCode);
            BotResponseMock.Verify(x => x.Response(text), Times.Once);
        }

        [Theory]
        [InlineData(LangCode.Ru, StateType.NeedAddMetric)]
        [InlineData(LangCode.En, StateType.NeedAddUnit)]
        public async Task Process_TextMessage_SourceHasNoMetric(LangCode langCode, StateType initialState)
        {
            int externalId = 1;
            string name = "Koko";
            Source source = new Source(externalId, name);
            source.UpdateState(initialState);

            var mainMetric = new Metric("Some metric");
            mainMetric.UpdateSource(source);
            mainMetric.UpdateMain(true);

            MetricRepoMock.Setup(r => r.GetMainBySourceId(It.IsAny<long>())).Returns(() => mainMetric);
            MetricRepoMock.Setup(r => r.GetNotCreatedMetric(It.IsAny<long>())).Returns(() => mainMetric);

            SourceRepoMock.Setup(r => r.FindOrCreateNew(externalId, name)).Returns(() => source);
            SourceRepoMock.Setup(r => r.Update(source));

            MetricRepoMock.Setup(r => r.Add(It.IsAny<Metric>()));
            MetricRepoMock.Setup(r => r.Update(It.IsAny<Metric>()));

            var message = new TextUpdateMessage
            {
                ChatId = externalId,
                Username = name,
                Content = "name or unit of metric",
                LanguageCode = langCode,
            };

            await message.Accept(MessageProcessor);

            var localizer = new Localizer();
            var text = localizer.GetMessage(langCode, ProcessMessageVisitor.GetMessageCode(source.State));
            BotResponseMock.Verify(x => x.Response(text), Times.Once);
        }
    }
}
