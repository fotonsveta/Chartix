using System.Threading.Tasks;
using Chartix.Infrastructure.Telegram.Models;

namespace Chartix.Infrastructure.Telegram.ButtonCommands
{
    public abstract class ButtonCommand : IButtonCommand
    {
        public string Parameter { get; private set; }

        public int MenuMessageId { get; private set; }

        public abstract ButtonCode Button { get; }

        protected virtual ButtonCode NextButton { get; set; }

        public abstract Task HandleAsync(CallbackQueryUpdateMessage message);

        public void SetDataOneTime(MenuButtonData menuButtonData)
        {
            if (MenuMessageId == 0)
            {
                MenuMessageId = menuButtonData.MenuMessageId;
                Parameter = menuButtonData.Parameter;
            }
        }

        protected string FormatKey(long id) => new MenuButtonData(NextButton, MenuMessageId, id.ToString()).ToString();
    }
}
