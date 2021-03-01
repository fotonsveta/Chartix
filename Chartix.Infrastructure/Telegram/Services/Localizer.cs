using System.Collections.Generic;
using Chartix.Infrastructure.Telegram.Interfaces;
using Chartix.Infrastructure.Telegram.Models;

namespace Chartix.Infrastructure.Telegram.Services
{
    public class Localizer : ILocalizer
    {
        private readonly Dictionary<LangCode, Dictionary<ButtonCode, string>> _buttons =
            new Dictionary<LangCode, Dictionary<ButtonCode, string>>
            {
                [LangCode.Ru] = new Dictionary<ButtonCode, string>
                {
                    [ButtonCode.Menu] = "Меню",
                    [ButtonCode.MetricMenu] = "Показатели",
                    [ButtonCode.ShowMetric] = "Показать текущий показатель",
                    [ButtonCode.AddMetric] = "Добавить показатель",
                    [ButtonCode.ChooseDelMetric] = "Удалить показатель",
                    [ButtonCode.ChooseMainMetric] = "Изменить текущий показатель",
                    [ButtonCode.ChooseDelValue] = "Удалить значение",
                    [ButtonCode.Plot] = "Построить график",
                    [ButtonCode.FileMenu] = "Выгрузить / загрузить",
                    [ButtonCode.ToJson] = "Выгрузить в json-файл",
                    [ButtonCode.FromJson] = "Загрузить из json-файла",
                },
                [LangCode.En] = new Dictionary<ButtonCode, string>
                {
                    [ButtonCode.Menu] = "Menu",
                    [ButtonCode.MetricMenu] = "Metrics",
                    [ButtonCode.ShowMetric] = "Show main metric",
                    [ButtonCode.AddMetric] = "Add metric",
                    [ButtonCode.ChooseDelMetric] = "Delete metric",
                    [ButtonCode.ChooseMainMetric] = "Change main metric",
                    [ButtonCode.ChooseDelValue] = "Delete value",
                    [ButtonCode.Plot] = "Create plot",
                    [ButtonCode.FileMenu] = "Import / export",
                    [ButtonCode.ToJson] = "Export to json",
                    [ButtonCode.FromJson] = "Import from json",
                },
            };

        private readonly Dictionary<LangCode, Dictionary<MessageCode, string>> _messages =
            new Dictionary<LangCode, Dictionary<MessageCode, string>>
            {
                [LangCode.Ru] = new Dictionary<MessageCode, string>
                {
                    [MessageCode.UnknownCommand] = "Неизвестная команда",
                    [MessageCode.EnterSomething] = "Введите значение или команду",
                    [MessageCode.EnterMainMetric] = "Введите название отслеживаемого показателя",
                    [MessageCode.EnterMainUnit] = "Введите единицу измерения отслеживаемого показателя",
                    [MessageCode.About] = FormatRuAboutMessage,
                    [MessageCode.Help] = FormatRuHelpMessage,
                    [MessageCode.Done] = "Сделано",
                    [MessageCode.NoMainMetric] = @"У вас нет показателей, добавьте один через команду /menu",
                    [MessageCode.InnerError] = "Произошла ошибка, попробуйте снова",
                    [MessageCode.InvalidValue] = @"Неверный формат значения, введите команду /help, чтобы увидеть правильный",
                    [MessageCode.ValueWithoutMetric] = "Введенное значение не к чему привязывать, не задан текущий показатель",
                    [MessageCode.Hello] = "Привет",
                    [MessageCode.ToFewValues] = "Слишком мало значений для построения графика",
                    [MessageCode.UploadJson] = "Загрузите файл (ожидаемый формат можно посмотреть в выгруженном файле, см. меню - Выгрузить в файл json)",
                    [MessageCode.ExpectedJsonFile] = "Извините, я могу загружать данные только из JSON файла",
                    [MessageCode.JsonError] = "Файл не доступен, либо имеет некорректные данные",
                    [MessageCode.NoMetrics] = "У вас не заданы показатели",
                },
                [LangCode.En] = new Dictionary<MessageCode, string>
                {
                    [MessageCode.UnknownCommand] = "Unknown command",
                    [MessageCode.EnterSomething] = "Enter a value or command",
                    [MessageCode.EnterMainMetric] = "Enter a name of the tracked metric",
                    [MessageCode.EnterMainUnit] = "Enter the unit of measure for the tracked metric",
                    [MessageCode.About] = FormatEngAboutMessage,
                    [MessageCode.Help] = FormatEngHelpMessage,
                    [MessageCode.Done] = "Done",
                    [MessageCode.NoMainMetric] = @"You have no metrics, add one via the /menu command",
                    [MessageCode.InnerError] = "An error has occurred, try again",
                    [MessageCode.InvalidValue] = @"Invalid value format, enter the /help command to see the correct one",
                    [MessageCode.ValueWithoutMetric] = "Value has nothing to bind, the current metric is not set",
                    [MessageCode.Hello] = "Hi",
                    [MessageCode.ToFewValues] = "Too few values to create plot",
                    [MessageCode.UploadJson] = "Import from file (the expected format can be viewed in the exported file, see menu - Export to json",
                    [MessageCode.ExpectedJsonFile] = "Sorry, I can only load data from a JSON file",
                    [MessageCode.JsonError] = "The file is not available or has incorrect data",
                    [MessageCode.NoMetrics] = "Metric list is empty",
                },
            };

        private static string FormatRuAboutMessage => "Бот предназначен для ведения изменений значений некоторого показателя с течением времени " +
            "(например, массы тела). Периодически добавляйте текущее значение, и в итоге можете построить график изменений.";

        private static string FormatRuHelpMessage => @"Допустимый формат ввода значений:
<b>Число</b> <b>Дата</b> <b>Время</b>
<b>Число</b> - целое или с точкой, обязательное,
<b>Дата</b> - в формате день.месяц или день.месяц.год, необязательное,
<b>Время</b> - в формате часы:минуты, необязательное,
если не указаны дата или время, будет использовано текущее время.";

        private static string FormatEngAboutMessage => "The bot is designed to manage changes in the values of a certain metric over time (for example, body " +
            "weight). Periodically add the current value, and in the end you can make a plot of changes.";

        private static string FormatEngHelpMessage => @"Valid input format for values:
<b>Number</b> <b>Date</b> <b>Time</b>
<b>Number</b> - integer or with a dot, obligatory,
<b>Date</b> - in the format month/day or year/month/day, optional,
<b>Time</b> - in the format hours:minutes, optional,
if no date or time is specified, the current time will be used.";

        public string GetMessage(LangCode code, MessageCode key)
        {
            return _messages[code][key];
        }

        public string GetButtonText(LangCode code, ButtonCode key)
        {
            return _buttons[code][key];
        }
    }
}
