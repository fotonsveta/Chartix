using System.Threading.Tasks;
using Chartix.Infrastructure.Telegram.Interfaces;
using Chartix.Infrastructure.Telegram.Models;

namespace Chartix.Infrastructure.Telegram.Commands
{
    public class StartCommand : ICommand
    {
        private readonly IBotClient _botClient;

        public StartCommand(IBotClient botClient)
        {
            _botClient = botClient;
        }

        public string Name => "start";

        public async Task HandleAsync(UpdateMessage message)
        {
            await _botClient.SendTextMessageAsync(message, MessageCode.Hello);
        }
    }
}
