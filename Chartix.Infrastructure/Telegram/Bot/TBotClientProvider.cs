using System;
using Chartix.Infrastructure.Telegram.Interfaces;
using Microsoft.Extensions.Logging;
using MihaZupan;
using Telegram.Bot;

namespace Chartix.Infrastructure.Telegram.Bot
{
    public class TBotClientProvider : ITBotClientProvider
    {
        private readonly ILogger<TBotClientProvider> _logger;
        private readonly BotConfig _config;

        public TBotClientProvider(ILogger<TBotClientProvider> logger, BotConfig config)
        {
            _logger = logger;
            _config = config;
            Client = TryConnect();
        }

        public ITelegramBotClient Client { get; }

        public void Configure()
        {
            if (_config.NeedSetWebhook && !string.IsNullOrEmpty(_config.WebhookUrl))
            {
                Client.SetWebhookAsync(_config.WebhookUrl);
            }
        }

        private TelegramBotClient TryConnect()
        {
            try
            {
                return Connect();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        private TelegramBotClient Connect()
        {
            if (_config is null)
            {
                throw new ArgumentException("Bot config section is empty in appsettings.json");
            }

            if (_config.NeedSetUserData && (string.IsNullOrEmpty(_config.Username) || string.IsNullOrEmpty(_config.Password)))
            {
                throw new ArgumentException("Userdata not full.");
            }

            TelegramBotClient botClient;
            if (string.IsNullOrEmpty(_config.Host) || _config.Port == 0)
            {
                botClient = new TelegramBotClient(_config.Token);
            }
            else
            {
                var proxy = _config.NeedSetUserData ?
                    new HttpToSocks5Proxy(_config.Host, _config.Port, _config.Username, _config.Password) :
                    new HttpToSocks5Proxy(_config.Host, _config.Port);

                botClient = new TelegramBotClient(_config.Token, proxy);
            }

            _logger.LogDebug($"Successfilly connect to TelegramBotClient");
            return botClient;
        }
    }
}
