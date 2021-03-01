using Chartix.Infrastructure.Telegram.Models;

namespace Chartix.Infrastructure.Telegram.Interfaces
{
    public interface ILocalizer
    {
        string GetMessage(LangCode code, MessageCode key);

        string GetButtonText(LangCode code, ButtonCode key);
    }
}
