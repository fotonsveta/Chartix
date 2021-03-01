namespace Chartix.Infrastructure.Telegram.Models
{
    public enum MessageCode
    {
        UnknownCommand,
        EnterSomething,
        EnterMainMetric,
        EnterMainUnit,
        About,
        Help,
        Done,
        NoMainMetric,
        InnerError,
        InvalidValue,
        ValueWithoutMetric,
        Hello,
        ToFewValues,
        UploadJson,
        ExpectedJsonFile,
        JsonError,
        NoMetrics,
    }
}
