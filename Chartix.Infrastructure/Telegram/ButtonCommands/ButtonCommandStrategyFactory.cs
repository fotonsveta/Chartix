using System.Collections.Generic;
using System.Linq;
using Chartix.Infrastructure.Telegram.Interfaces;
using Chartix.Infrastructure.Telegram.Models;

namespace Chartix.Infrastructure.Telegram.ButtonCommands
{
    public class ButtonCommandStrategyFactory : ICommandStrategyFactory<IButtonCommand>
    {
        private readonly IEnumerable<IButtonCommand> _commands;

        public ButtonCommandStrategyFactory(IEnumerable<IButtonCommand> commands)
        {
            _commands = commands;
        }

        public IButtonCommand GetCommand(string data)
        {
            var menuButtonData = new MenuButtonData(data);
            var command = _commands.FirstOrDefault(c => c.Button == menuButtonData.Button);
            command.SetDataOneTime(menuButtonData);
            return command;
        }
    }
}
