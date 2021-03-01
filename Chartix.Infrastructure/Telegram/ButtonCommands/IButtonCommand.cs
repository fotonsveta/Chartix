using System.Threading.Tasks;
using Chartix.Infrastructure.Telegram.Models;

namespace Chartix.Infrastructure.Telegram.ButtonCommands
{
    public interface IButtonCommand
    {
        public string Parameter { get; }

        public int MenuMessageId { get; }

        public ButtonCode Button { get; }

        public Task HandleAsync(CallbackQueryUpdateMessage message);

        public void SetDataOneTime(MenuButtonData menuButtonData);
    }
}
