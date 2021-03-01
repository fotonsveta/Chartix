using System.Collections.Generic;
using Chartix.Core.Interfaces;
using Chartix.Infrastructure.Telegram.ButtonCommands;
using Chartix.Infrastructure.Telegram.Commands;
using Chartix.Infrastructure.Telegram.Interfaces;

namespace Chartix.Tests
{
    public class ServiceHelper
    {
        public ICommandStrategyFactory<ICommand> CreateCSF(IBotClient botClient)
        {
            var commands = new List<ICommand>
            {
                new StartCommand(botClient),
                new AboutCommand(botClient),
                new HelpCommand(botClient),
                new MenuCommand(botClient),
                new UnknownCommand(botClient),
            };

            return new CommandStrategyFactory(commands);
        }

        public ICommandStrategyFactory<IButtonCommand> CreateButtonCSF(
            IBotClient botClient,
            ISourceRepository sourceRepo,
            IMetricRepository metricRepo,
            IValueRepository valueRepo)
        {
            var buttonCommands = new List<IButtonCommand>
            {
                new AddMetricButtonCommand(botClient, sourceRepo),
                new ChooseDelMetricButtonCommand(botClient, sourceRepo, metricRepo),
                new ChooseDelValueButtonCommand(botClient, metricRepo, valueRepo),
                new ChooseMainMetricButtonCommand(botClient, sourceRepo, metricRepo),
                new DelMetricButtonCommand(botClient, sourceRepo, metricRepo),
                new DelValueButtonCommand(botClient, valueRepo),
                new FileMenuButtonCommand(botClient),
                new FromJsonButtonCommand(botClient),
                new PlotButtonCommand(botClient, metricRepo, valueRepo),
                new MetricMenuButtonCommand(botClient),
                new SetMainMetricButtonCommand(botClient, metricRepo),
                new ShowMetricButtonCommand(botClient, metricRepo),
                new ToJsonButtonCommand(botClient, metricRepo, sourceRepo),
                new TopMenuButtonCommand(botClient),
            };

            return new ButtonCommandStrategyFactory(buttonCommands);
        }
    }
}
