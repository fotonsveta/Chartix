using System.Collections.Generic;
using System.Linq;
using Chartix.Infrastructure.Telegram.Interfaces;

namespace Chartix.Infrastructure.Telegram.Commands
{
    public class CommandStrategyFactory : ICommandStrategyFactory<ICommand>
    {
        private readonly IEnumerable<ICommand> _commands;

        public CommandStrategyFactory(IEnumerable<ICommand> commands)
        {
            _commands = commands;
        }

        public ICommand GetCommand(string data)
        {
            data = data?.Trim().ToLower();
            var command = _commands.FirstOrDefault(c => c.Name == data);
            return command ?? _commands.FirstOrDefault(c => c.Name is null);
        }
    }
}
