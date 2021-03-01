using System.Threading.Tasks;
using Chartix.Infrastructure.Telegram.Interfaces;
using Chartix.Infrastructure.Telegram.Models;

namespace Chartix.Infrastructure.Telegram.ButtonCommands
{
    public class FromJsonButtonCommand : ButtonCommand
    {
        private readonly IBotClient _botClient;

        public FromJsonButtonCommand(IBotClient botClient)
        {
            _botClient = botClient;
        }

        public override ButtonCode Button => ButtonCode.FromJson;

        public override async Task HandleAsync(CallbackQueryUpdateMessage message)
        {
            await _botClient.SendTextMessageAsync(message, MessageCode.UploadJson);
        }
    }
}
