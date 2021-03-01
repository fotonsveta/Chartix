using System;
using System.Globalization;
using System.Linq;
using Chartix.Core.Entities;
using Chartix.Infrastructure.Telegram.Models;

namespace Chartix.Infrastructure.Telegram.Services
{
    public class ValueParser
    {
        private readonly string[] _ruDateFormats = new string[]
        {
            "dd.MM", "dd.MM.yyyy",
            "d.MM", "d.MM.yyyy",
            "dd.M", "dd.M.yyyy",
            "d.M", "d.M.yyyy",
        };

        private readonly string[] _engDateFormats = new string[]
        {
            "MM.dd", "yyyy.MM.dd", "MM/dd", "yyyy/MM/dd",
            "M.dd", "yyyy.M.dd", "M/dd", "yyyy/M/dd",
            "MM.d", "yyyy.MM.d", "MM/d", "yyyy/MM/d",
            "M.d", "yyyy.M.d", "M/d", "yyyy/M/d",
        };

        public (Value, string) Parse(UpdateMessage message)
        {
            var parts = message.Content.Split(" ").Where(s => !string.IsNullOrEmpty(s)).ToArray();

            if (!double.TryParse(parts[0], NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out double number))
            {
                return (null, $"Wrong format for number {parts[0]} (in {message.Content})");
            }

            var valueDate = message.UpdateDatetime;
            if (parts.Length > 1)
            {
                var date = ParseDate(parts[1], message.LanguageCode);
                if (date is null)
                {
                    return (null, $"Wrong format for date {parts[1]} (in {message.Content})");
                }

                valueDate = (DateTime)date;
            }

            if (parts.Length > 2)
            {
                (int hour, int minutes) = ParseTime(parts[2]);
                if (hour == -1)
                {
                    return (null, $"Wrong format for time {parts[2]} (in {message.Content})");
                }

                valueDate = valueDate.Date.AddHours(hour).AddMinutes(minutes);
            }
            else
            {
                valueDate = valueDate.Date.AddHours(message.UpdateDatetime.Hour).AddMinutes(message.UpdateDatetime.Minute);
            }

            return (new Value(number, valueDate), null);
        }

        private DateTime? ParseDate(string dateText, LangCode langCode)
        {
            var formats = langCode is LangCode.Ru ? _ruDateFormats : _engDateFormats;

            return DateTime.TryParseExact(dateText, formats, null, DateTimeStyles.None, out DateTime date) ?
                date :
                null;
        }

        private (int hour, int minutes) ParseTime(string timeText)
        {
            try
            {
                var parts = timeText.Split(":");
                var hour = int.Parse(parts[0]);
                var minutes = int.Parse(parts[1]);
                if (hour < 0 || hour > 23 || minutes < 0 || minutes > 59)
                {
                    throw new ArgumentException("Wrong time format");
                }

                return (hour, minutes);
            }
            catch (Exception)
            {
                return (-1, -1);
            }
        }
    }
}
