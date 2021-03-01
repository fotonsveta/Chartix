using System.Threading.Tasks;
using Chartix.Infrastructure.Telegram.Interfaces;
using Chartix.Infrastructure.Telegram.Models;
using Telegram.Bot.Types.Enums;

namespace Chartix.Infrastructure.Telegram.Commands
{
    public class HelpCommand : ICommand
    {
        private readonly IBotClient _botClient;

        public HelpCommand(IBotClient botClient)
        {
            _botClient = botClient;
        }

        public string Name => "help";

        public async Task HandleAsync(UpdateMessage message)
        {
            await _botClient.SendTextMessageAsync(message, MessageCode.Help, ParseMode.Html);
        }
    }
}
