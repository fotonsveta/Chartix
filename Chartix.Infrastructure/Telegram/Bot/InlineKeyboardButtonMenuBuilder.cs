using System.Collections.Generic;
using Chartix.Infrastructure.Telegram.Interfaces;
using Chartix.Infrastructure.Telegram.Models;
using Telegram.Bot.Types.ReplyMarkups;

namespace Chartix.Infrastructure.Telegram.Bot
{
    public class InlineKeyboardButtonMenuBuilder
    {
        private readonly ILocalizer _localizer;
        private readonly IList<IList<InlineKeyboardButton>> _menu;

        public InlineKeyboardButtonMenuBuilder(ILocalizer localizer)
        {
            _localizer = localizer;
            _menu = new List<IList<InlineKeyboardButton>>();
        }

        public InlineKeyboardMarkup Build()
        {
            return new InlineKeyboardMarkup(_menu);
        }

        public InlineKeyboardButtonMenuBuilder AddButtonCodePerLine(IEnumerable<ButtonCode> buttonCodes, LangCode langCode, int menuMessageId)
        {
            foreach (var buttonCode in buttonCodes)
            {
                _menu.Add(FormatOneButtonInline(langCode, buttonCode, menuMessageId));
            }

            return this;
        }

        public InlineKeyboardButtonMenuBuilder AddBackButton(ButtonCode buttonCode, int menuMessageId)
        {
            if (buttonCode != ButtonCode.None)
            {
                var data = new MenuButtonData(buttonCode, menuMessageId).ToString();
                _menu.Add(FormatOneButtonInline("<<<<", data));
            }

            return this;
        }

        public InlineKeyboardButtonMenuBuilder AddTextPerLine(IDictionary<string, string> texts)
        {
            foreach (var text in texts)
            {
                _menu.Add(FormatOneButtonInline(text.Value, text.Key));
            }

            return this;
        }

        public InlineKeyboardButtonMenuBuilder AddPagingLine(int current, int count, ButtonCode button, int menuMessageId)
        {
            string FormatCallbackData(int page) => new MenuButtonData(button, menuMessageId, page.ToString()).ToString();

            var pagingButtons = new List<InlineKeyboardButton>();
            if (current > 1)
            {
                pagingButtons.Add(CreateIKB("<<", FormatCallbackData(1)));
                pagingButtons.Add(CreateIKB("<", FormatCallbackData(current - 1)));
            }

            pagingButtons.Add(CreateIKB(current.ToString(), FormatCallbackData(current)));
            if (count > current)
            {
                pagingButtons.Add(CreateIKB(">", FormatCallbackData(current + 1)));
                pagingButtons.Add(CreateIKB(">>", FormatCallbackData(count)));
            }

            _menu.Add(pagingButtons);
            return this;
        }

        private IList<InlineKeyboardButton> FormatOneButtonInline(LangCode code, ButtonCode buttonText, int menuMessageId)
        {
            var data = new MenuButtonData(buttonText, menuMessageId).ToString();
            return FormatOneButtonInline(_localizer.GetButtonText(code, buttonText), data);
        }

        private IList<InlineKeyboardButton> FormatOneButtonInline(string text, string data) => new List<InlineKeyboardButton>()
            {
                CreateIKB(text, data),
            };

        private InlineKeyboardButton CreateIKB(string text, string data) =>
            new InlineKeyboardButton() { Text = text, CallbackData = data };
    }
}
