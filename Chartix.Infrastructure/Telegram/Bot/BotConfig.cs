namespace Chartix.Infrastructure.Telegram.Bot
{
    public class BotConfig
    {
        public string Token { get; set; }

        public string Host { get; set; }

        public int Port { get; set; }

        public bool NeedSetUserData { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public bool NeedSetWebhook { get; set; }

        public string WebhookUrl { get; set; }
    }
}