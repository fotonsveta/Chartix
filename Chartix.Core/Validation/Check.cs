namespace Chartix.Core.Validation
{
    public static class Check
    {
        public static string NullIfEmpty(string value) => string.IsNullOrEmpty(value) ? null : value;
    }
}
