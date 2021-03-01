using System;

namespace Chartix.Infrastructure.Telegram.Models
{
    public class MenuButtonData
    {
        public MenuButtonData(string callbackData)
        {
            if (string.IsNullOrEmpty(callbackData))
            {
                throw new ArgumentException("Empty data from button");
            }

            var parts = callbackData.Split("_");
            if (!Enum.TryParse(parts[0], out ButtonCode button))
            {
                throw new ArgumentException($"Unknown button command {callbackData}");
            }

            Button = button;
            MenuMessageId = int.Parse(parts[1]);
            if (parts.Length > 2)
            {
                Parameter = parts[2];
            }
        }

        public MenuButtonData(ButtonCode button, int menuMessageId, string parameter = null)
        {
            Button = button;
            MenuMessageId = menuMessageId;
            Parameter = parameter;
        }

        public ButtonCode Button { get; }

        public int MenuMessageId { get; }

        public string Parameter { get; }

        public override string ToString()
        {
            return string.IsNullOrEmpty(Parameter)
                ? $"{Button}_{MenuMessageId}"
                : $"{Button}_{MenuMessageId}_{Parameter}";
        }
    }
}
