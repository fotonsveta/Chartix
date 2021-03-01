using Chartix.Core.Interfaces;
using Chartix.Infrastructure.Telegram.ButtonCommands;
using Chartix.Infrastructure.Telegram.Interfaces;
using Moq;

namespace Chartix.Tests.Core.ButtonCommands
{
    public class ButtonCommandFactoryFixture
    {
        public ButtonCommandFactoryFixture()
        {
            var botClient = new Mock<IBotClient>().Object;
            var sourceRepo = new Mock<ISourceRepository>().Object;
            var metricRepo = new Mock<IMetricRepository>().Object;
            var valueRepo = new Mock<IValueRepository>().Object;

            ButtonCommandFactory = new ServiceHelper().CreateButtonCSF(botClient, sourceRepo, metricRepo, valueRepo);
        }

        public ICommandStrategyFactory<IButtonCommand> ButtonCommandFactory { get; }
    }
}
