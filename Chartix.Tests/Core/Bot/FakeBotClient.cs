using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Chartix.Infrastructure.Telegram.Bot;
using Chartix.Infrastructure.Telegram.Interfaces;
using Chartix.Infrastructure.Telegram.Models;
using Chartix.Infrastructure.Telegram.Services;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace Chartix.Tests.Core.Bot
{
    public class FakeBotClient : IBotClient
    {
        private readonly ILocalizer _localizer;
        private readonly FakeBotResponse _botResponse;

        public FakeBotClient(FakeBotResponse botResponse)
        {
            _localizer = new Localizer();
            _botResponse = botResponse;
        }

        public ITelegramBotClient TelegramBot => throw new System.NotImplementedException();

        public Task SendFileAsync(long externalId, Stream stream, string filename)
        {
            var dir = Directory.GetCurrentDirectory();
            var path = Path.Combine(dir, filename);

            using (var file = new FileStream(path, FileMode.Create))
            {
                stream.CopyTo(file);
            }

            _botResponse.Response(filename);
            return Task.CompletedTask;
        }

        public Task SendFileAsync(long externalId, byte[] bytes, string filename)
        {
            var dir = Directory.GetCurrentDirectory();
            var path = Path.Combine(dir, filename);

            using (var file = new FileStream(path, FileMode.Create))
            {
                file.Write(bytes, 0, bytes.Length);
            }

            _botResponse.Response(filename);
            return Task.CompletedTask;
        }

        public Task SendTextButtonsMenuAsync(UpdateMessage message, ButtonStructure structure)
        {
            var menuBuilder = new InlineKeyboardButtonMenuBuilder(_localizer);
            var replyMarkup = menuBuilder.AddButtonCodePerLine(structure.MenuButtons, message.LanguageCode, 1).Build();

            PrintOutReplyMarkup(message.LanguageCode, structure.TopButtonCode, replyMarkup);

            return Task.CompletedTask;
        }

        public Task EditTextButtonsMenuAsync(CallbackQueryUpdateMessage message, ButtonStructure structure)
        {
            return SendTextButtonsMenuAsync(message, structure);
        }

        public Task EditTextButtonsMenuWithPagingAsync(CallbackQueryUpdateMessage message, ButtonCode topButtonCode, PagingMenuData menuData)
        {
            var menuBuilder = new InlineKeyboardButtonMenuBuilder(_localizer);
            var replyMarkup = menuBuilder.AddTextPerLine(menuData.ButtonTexts)
                .AddPagingLine(menuData.CurrentPage, menuData.PageCount, topButtonCode, message.MenuMessageId)
                .Build();

            PrintOutReplyMarkup(message.LanguageCode, topButtonCode, replyMarkup);

            return Task.CompletedTask;
        }

        public Task SendTextMessageAsync(UpdateMessage message, MessageCode messageCode, ParseMode parseMode = ParseMode.Default)
        {
            var text = _localizer.GetMessage(message.LanguageCode, messageCode);
            _botResponse.Response(text);
            return Task.CompletedTask;
        }

        public Task SendTextMessageAsync(UpdateMessage message, string text, ParseMode parseMode = ParseMode.Default)
        {
            _botResponse.Response(text);
            return Task.CompletedTask;
        }

        public async Task DownloadFileAsync(string fileId, Stream destination)
        {
            var dir = Directory.GetCurrentDirectory();
            var path = Path.Combine(dir, fileId);

            if (!File.Exists(path))
            {
                return;
            }

            using (var file = new FileStream(path, FileMode.Open))
            {
                await file.CopyToAsync(destination);
                if (destination.CanSeek)
                {
                    destination.Seek(0, SeekOrigin.Begin);
                }
            }
        }

        private void PrintOutReplyMarkup(LangCode langCode, ButtonCode topButtonCode, InlineKeyboardMarkup replyMarkup)
        {
            var text = _localizer.GetButtonText(langCode, topButtonCode);
            var textBuilder = new StringBuilder().AppendLine(text);

            foreach (var row in replyMarkup.InlineKeyboard)
            {
                var rowText = new StringBuilder();
                foreach (var column in row)
                {
                    rowText.Append($"{column.Text} ({column.CallbackData})\t");
                }

                textBuilder.AppendLine(rowText.ToString());
            }

            _botResponse.Response(textBuilder.ToString());
        }
    }
}
