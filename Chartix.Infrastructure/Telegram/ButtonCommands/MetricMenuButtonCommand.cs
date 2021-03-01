using System.Threading.Tasks;
using Chartix.Infrastructure.Telegram.Interfaces;
using Chartix.Infrastructure.Telegram.Models;

namespace Chartix.Infrastructure.Telegram.ButtonCommands
{
    public class MetricMenuButtonCommand : ButtonCommand
    {
        private readonly IBotClient _botClient;

        public MetricMenuButtonCommand(IBotClient botClient)
        {
            _botClient = botClient;
        }

        public override ButtonCode Button => ButtonCode.MetricMenu;

        public override async Task HandleAsync(CallbackQueryUpdateMessage message)
        {
            message = message with { MenuMessageId = MenuMessageId };
            await _botClient.EditTextButtonsMenuAsync(message, ButtonStructure.MetricMenuStructure);
        }
    }
}
