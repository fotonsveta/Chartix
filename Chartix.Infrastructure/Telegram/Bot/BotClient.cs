using System.IO;
using System.Threading.Tasks;
using Chartix.Infrastructure.Telegram.Interfaces;
using Chartix.Infrastructure.Telegram.Models;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InputFiles;

namespace Chartix.Infrastructure.Telegram.Bot
{
    public class BotClient : IBotClient
    {
        private readonly ILocalizer _localizer;

        public BotClient(ITBotClientProvider botClientProvider, ILocalizer localizer)
        {
            TelegramBot = botClientProvider.Client;
            _localizer = localizer;
        }

        public ITelegramBotClient TelegramBot { get; }

        public async Task SendTextMessageAsync(UpdateMessage message, MessageCode messageCode, ParseMode parseMode = ParseMode.Default)
        {
            await TelegramBot.SendTextMessageAsync(
                message.ChatId,
                _localizer.GetMessage(message.LanguageCode, messageCode),
                parseMode);
        }

        public async Task SendTextMessageAsync(UpdateMessage message, string text, ParseMode parseMode = ParseMode.Default)
        {
            await TelegramBot.SendTextMessageAsync(message.ChatId, text, parseMode);
        }

        public async Task SendTextButtonsMenuAsync(UpdateMessage message, ButtonStructure structure)
        {
            var langCode = message.LanguageCode;
            var menuBuilder = new InlineKeyboardButtonMenuBuilder(_localizer);
            var answerMessage = await TelegramBot.SendTextMessageAsync(
                message.ChatId,
                _localizer.GetButtonText(langCode, structure.TopButtonCode),
                replyMarkup: null);

            var expectedAnswerId = answerMessage.MessageId;
            var replyMarkup = menuBuilder.AddButtonCodePerLine(structure.MenuButtons, langCode, expectedAnswerId).Build();
            await TelegramBot.EditMessageTextAsync(
                message.ChatId,
                expectedAnswerId,
                _localizer.GetButtonText(langCode, structure.TopButtonCode),
                replyMarkup: replyMarkup);
        }

        public async Task EditTextButtonsMenuAsync(CallbackQueryUpdateMessage message, ButtonStructure structure)
        {
            var langCode = message.LanguageCode;
            var menuBuilder = new InlineKeyboardButtonMenuBuilder(_localizer);
            var replyMarkup = menuBuilder
                .AddButtonCodePerLine(structure.MenuButtons, langCode, message.MenuMessageId)
                .AddBackButton(structure.PreviousButtonCode, message.MenuMessageId)
                .Build();

            await TelegramBot.EditMessageTextAsync(
                message.ChatId,
                message.MenuMessageId,
                _localizer.GetButtonText(langCode, structure.TopButtonCode),
                replyMarkup: replyMarkup);
        }

        public async Task EditTextButtonsMenuWithPagingAsync(CallbackQueryUpdateMessage message, ButtonCode topButtonCode, PagingMenuData menuData)
        {
            var menuBuilder = new InlineKeyboardButtonMenuBuilder(_localizer);
            var replyMarkup = menuBuilder
                .AddTextPerLine(menuData.ButtonTexts)
                .AddPagingLine(menuData.CurrentPage, menuData.PageCount, topButtonCode, message.MenuMessageId)
                .AddBackButton(menuData.Previous, message.MenuMessageId)
                .Build();

            await TelegramBot.EditMessageTextAsync(
                message.ChatId,
                message.MenuMessageId,
                _localizer.GetButtonText(message.LanguageCode, topButtonCode),
                replyMarkup: replyMarkup);
        }

        public async Task SendFileAsync(long externalId, Stream stream, string filename)
        {
            var file = new InputOnlineFile(stream, filename);
            await TelegramBot.SendDocumentAsync(externalId, file);
        }

        public async Task SendFileAsync(long externalId, byte[] bytes, string filename)
        {
            using (var stream = new MemoryStream(bytes))
            {
                var file = new InputOnlineFile(stream, filename);
                await TelegramBot.SendDocumentAsync(externalId, file);
            }
        }

        public async Task DownloadFileAsync(string fileId, Stream destination)
        {
            await TelegramBot.GetInfoAndDownloadFileAsync(fileId, destination);
        }
    }
}
