using Chartix.Infrastructure.Telegram.Interfaces;

namespace Microsoft.Extensions.Hosting
{
    public static class HostConfigureBotClient
    {
        public static IHost ConfigureBotClient(this IHost host)
        {
            if (host.Services.GetService(typeof(ITBotClientProvider)) is ITBotClientProvider botClientProvider)
            {
                botClientProvider.Configure();
            }

            return host;
        }
    }
}
