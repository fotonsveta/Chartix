using System;
using System.Text.Json;
using System.Threading.Tasks;
using Chartix.Core.Entities;
using Chartix.Core.Interfaces;
using Chartix.Infrastructure.Telegram.Interfaces;
using Chartix.Infrastructure.Telegram.MessageVisitors;
using Chartix.Infrastructure.Telegram.Models;
using Microsoft.Extensions.Logging;
using Telegram.Bot.Types;

namespace Chartix.Infrastructure.Telegram.Services
{
    public class BotUpdateService : IBotUpdateService
    {
        private readonly ILogger<BotUpdateService> _logger;
        private readonly IBotClient _botClient;
        private readonly IUpdateRepository _updateRepository;
        private readonly IProcessMessageVisitor _processMessageVisitor;

        public BotUpdateService(
                                ILogger<BotUpdateService> logger,
                                IBotClient botClient,
                                IUpdateRepository updateRepository,
                                IProcessMessageVisitor processMessageVisitor)
        {
            _logger = logger;
            _botClient = botClient;
            _updateRepository = updateRepository;
            _processMessageVisitor = processMessageVisitor;
        }

        public async Task HandleAsync(Update update)
        {
            (UpdateMessage message, string error) = new UpdateParser().Parse(update);

            if (!string.IsNullOrEmpty(error))
            {
                _logger.LogError(error);
                if (update != null)
                {
                    _logger.LogDebug(JsonSerializer.Serialize(update));
                }

                return;
            }

            try
            {
                _logger.LogDebug($"Receive message {message.UpdateId} for source {message.ChatId} with type {message.Type}");
                if (IsProccesedMessage(message.UpdateId, message.ChatId))
                {
                    return;
                }

                await message.Accept(_processMessageVisitor);

                SaveProccessedMessage(message.UpdateId, message.ChatId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                _logger.LogError(JsonSerializer.Serialize(update));
                await _botClient.SendTextMessageAsync(message, MessageCode.InnerError);
            }
        }

        private bool IsProccesedMessage(int updateId, long externalId)
        {
            var update = _updateRepository.Get(updateId, externalId);
            return update != null;
        }

        private void SaveProccessedMessage(int updateId, long externalId) => _updateRepository.Add(new ProcessedUpdate(updateId, externalId));
    }
}
