using Telegram.Bot;

namespace Chartix.Infrastructure.Telegram.Interfaces
{
    public interface ITBotClientProvider
    {
        ITelegramBotClient Client { get; }

        void Configure();
    }
}
