using System.Threading.Tasks;
using Chartix.Infrastructure.Telegram.Interfaces;
using Chartix.Infrastructure.Telegram.Models;

namespace Chartix.Infrastructure.Telegram.ButtonCommands
{
    public class TopMenuButtonCommand : ButtonCommand
    {
        private readonly IBotClient _botClient;

        public TopMenuButtonCommand(IBotClient botClient)
        {
            _botClient = botClient;
        }

        public override ButtonCode Button => ButtonCode.Menu;

        public override async Task HandleAsync(CallbackQueryUpdateMessage message)
        {
            message = message with { MenuMessageId = MenuMessageId };
            await _botClient.EditTextButtonsMenuAsync(message, ButtonStructure.TopMenuStructure);
        }
    }
}
