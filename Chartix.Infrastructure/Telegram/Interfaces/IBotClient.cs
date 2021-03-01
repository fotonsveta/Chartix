using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Chartix.Infrastructure.Telegram.Models;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;

namespace Chartix.Infrastructure.Telegram.Interfaces
{
    public interface IBotClient
    {
        ITelegramBotClient TelegramBot { get; }

        Task SendTextMessageAsync(UpdateMessage message, MessageCode messageCode, ParseMode parseMode = ParseMode.Default);

        Task SendTextMessageAsync(UpdateMessage message, string text, ParseMode parseMode = ParseMode.Default);

        Task SendTextButtonsMenuAsync(UpdateMessage message, ButtonStructure structure);

        Task EditTextButtonsMenuAsync(CallbackQueryUpdateMessage message, ButtonStructure structure);

        Task EditTextButtonsMenuWithPagingAsync(CallbackQueryUpdateMessage message, ButtonCode topButtonCode, PagingMenuData menuData);

        Task SendFileAsync(long externalId, Stream stream, string filename);

        Task SendFileAsync(long externalId, byte[] bytes, string filename);

        Task DownloadFileAsync(string fileId, Stream destination);
    }
}
