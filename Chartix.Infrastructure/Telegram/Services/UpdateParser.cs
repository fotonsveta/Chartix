using System;
using Chartix.Infrastructure.Telegram.Models;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Chartix.Infrastructure.Telegram.Services
{
    public class UpdateParser
    {
        public (UpdateMessage, string) Parse(Update update)
        {
            if (update == null)
            {
                return (null, "Null update message received");
            }

            try
            {
                switch (update.Type)
                {
                    case UpdateType.Message:
                    case UpdateType.EditedMessage:
                        return ParseMessage(update);

                    case UpdateType.CallbackQuery:
                        return ParseCallbackQuery(update);
                }
            }
            catch (ArgumentNullException ane)
            {
                return (null, ane.Message);
            }

            return (null, $"Ignore update message with type {update.Type}");
        }

        private (CallbackQueryUpdateMessage, string) ParseCallbackQuery(Update update)
        {
            IsNotNullBlock(update.CallbackQuery, update.Id, update.Type, nameof(update.CallbackQuery));

            return (new CallbackQueryUpdateMessage
            {
                UpdateId = update.Id,
                UpdateDatetime = update.CallbackQuery.Message.Date,
                MessageId = update.CallbackQuery.Message.MessageId,
                ChatId = update.CallbackQuery.Message.Chat.Id,
                Username = update.CallbackQuery.Message.Chat.Username,
                LanguageCode = SetLangCode(update.CallbackQuery.From.LanguageCode),
                Content = update.CallbackQuery.Data,
            }, null);
        }

        private LangCode SetLangCode(string code)
        {
            return !string.IsNullOrEmpty(code) && code == "ru" ? LangCode.Ru : LangCode.En;
        }

        private (UpdateMessage, string) ParseMessage(Update update)
        {
            var message = FindMessageBlock(in update);

            IsNotNullBlock(message.From, update.Id, update.Type, nameof(message.From));
            IsNotNullBlock(message.Chat, update.Id, update.Type, nameof(message.Chat));

            if (message.Type == MessageType.Text)
            {
                if (string.IsNullOrEmpty(message.Text))
                {
                    return (null, $"Update message with id {update.Id} has null text");
                }

                var command = ParseCommand(message.Text);
                if (string.IsNullOrEmpty(command))
                {
                    return (new TextUpdateMessage
                    {
                        UpdateId = update.Id,
                        UpdateDatetime = message.Date,
                        MessageId = message.MessageId,
                        ChatId = message.Chat.Id,
                        Username = message.Chat.Username,
                        LanguageCode = SetLangCode(message.From.LanguageCode),
                        Content = message.Text,
                    }, null);
                }
                else
                {
                    return (new CommandUpdateMessage
                    {
                        UpdateId = update.Id,
                        UpdateDatetime = message.Date,
                        MessageId = message.MessageId,
                        ChatId = message.Chat.Id,
                        Username = message.Chat.Username,
                        LanguageCode = SetLangCode(message.From.LanguageCode),
                        Content = command,
                    }, null);
                }
            }
            else if (message.Type == MessageType.Document)
            {
                IsNotNullBlock(message.Document, update.Id, update.Type, nameof(message.Document));
                return (new DocumentUpdateMessage
                {
                    UpdateId = update.Id,
                    UpdateDatetime = message.Date,
                    MessageId = message.MessageId,
                    ChatId = message.Chat.Id,
                    Username = message.Chat.Username,
                    LanguageCode = SetLangCode(message.From.LanguageCode),
                    DocumentFileId = message.Document.FileId,
                    DocumentMimeType = message.Document.MimeType,
                }, null);
            }

            return (null, $"Update message with id {update.Id} has unexpected message type {message.Type}");

            Message FindMessageBlock(in Update update)
            {
                Message message;
                if (update.Type == UpdateType.Message)
                {
                    IsNotNullBlock(update.Message, update.Id, update.Type, nameof(update.Message));
                    message = update.Message;
                }
                else if (update.Type == UpdateType.EditedMessage)
                {
                    IsNotNullBlock(update.EditedMessage, update.Id, update.Type, nameof(update.EditedMessage));
                    message = update.EditedMessage;
                }
                else
                {
                    throw new ArgumentNullException($"Update message with id {update.Id} has unexpected type {update.Type}");
                }

                return message;
            }
        }

        private string ParseCommand(string message)
        {
            var parts = message.Split(' ');
            var command = parts[0];
            if (!command.StartsWith(@"/"))
            {
                return null;
            }

            command = command.Substring(1, command.Length - 1);
            return command;
        }

        private void IsNotNullBlock(object block, int id, UpdateType updateType, string blockName)
        {
            if (block == null)
            {
                throw new ArgumentNullException($"Update message with id {id} and type {updateType} has empty block {blockName}");
            }
        }
    }
}
