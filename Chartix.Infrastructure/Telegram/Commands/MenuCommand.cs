using System.Threading.Tasks;
using Chartix.Infrastructure.Telegram.Interfaces;
using Chartix.Infrastructure.Telegram.Models;

namespace Chartix.Infrastructure.Telegram.Commands
{
    public class MenuCommand : ICommand
    {
        private readonly IBotClient _botClient;

        public MenuCommand(IBotClient botClient)
        {
            _botClient = botClient;
        }

        public string Name => "menu";

        public async Task HandleAsync(UpdateMessage message)
        {
            await _botClient.SendTextButtonsMenuAsync(message, ButtonStructure.TopMenuStructure);
        }
    }
}
