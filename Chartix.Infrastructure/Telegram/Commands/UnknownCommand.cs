using System.Threading.Tasks;
using Chartix.Infrastructure.Telegram.Interfaces;
using Chartix.Infrastructure.Telegram.Models;

namespace Chartix.Infrastructure.Telegram.Commands
{
    public class UnknownCommand : ICommand
    {
        private readonly IBotClient _botClient;

        public UnknownCommand(IBotClient botClient)
        {
            _botClient = botClient;
        }

        public string Name => null;

        public async Task HandleAsync(UpdateMessage message)
        {
            await _botClient.SendTextMessageAsync(message, MessageCode.UnknownCommand);
        }
    }
}
