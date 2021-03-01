namespace Chartix.Infrastructure.Telegram.Models
{
    public class PlotData
    {
        public PlotData(double[] xValues, double[] yValues, string title)
        {
            XValues = xValues;
            YValues = yValues;
            Title = title;
        }

        public static PlotData Empty => new PlotData(new double[0], new double[0], string.Empty);

        public double[] XValues { get; }

        public double[] YValues { get; }

        public string Title { get; }
    }
}
