namespace Chartix.Core.Models
{
    public static class FileName
    {
        private static string JsonFilename { get; } = "metrics{0}.json";

        private static string PlotFilename { get; } = "plot{0}.png";

        public static string GetJsonFilename(string suffix) => string.Format(JsonFilename, suffix);

        public static string GetPlotFilename(string suffix) => string.Format(PlotFilename, suffix);
    }
}
