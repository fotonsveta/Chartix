using System.Threading.Tasks;
using Chartix.Infrastructure.Telegram.Interfaces;
using Chartix.Infrastructure.Telegram.Models;

namespace Chartix.Infrastructure.Telegram.Commands
{
    public class AboutCommand : ICommand
    {
        private readonly IBotClient _botClient;

        public AboutCommand(IBotClient botClient)
        {
            _botClient = botClient;
        }

        public string Name => "about";

        public async Task HandleAsync(UpdateMessage message)
        {
            await _botClient.SendTextMessageAsync(message, MessageCode.About);
        }
    }
}
