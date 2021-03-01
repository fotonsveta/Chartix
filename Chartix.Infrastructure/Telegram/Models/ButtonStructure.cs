using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Chartix.Infrastructure.Telegram.Models
{
    public class ButtonStructure
    {
        public static ButtonStructure TopMenuStructure => new ButtonStructure()
        {
            TopButtonCode = ButtonCode.Menu,
            PreviousButtonCode = ButtonCode.None,
            MenuButtons = new Collection<ButtonCode>
            {
                ButtonCode.MetricMenu,
                ButtonCode.ChooseDelValue,
                ButtonCode.Plot,
                ButtonCode.FileMenu,
            },
        };

        public static ButtonStructure MetricMenuStructure => new ButtonStructure()
        {
            TopButtonCode = ButtonCode.MetricMenu,
            PreviousButtonCode = ButtonCode.Menu,
            MenuButtons = new Collection<ButtonCode>
            {
                ButtonCode.ShowMetric,
                ButtonCode.ChooseMainMetric,
                ButtonCode.AddMetric,
                ButtonCode.ChooseDelMetric,
            },
        };

        public static ButtonStructure FileMenuStructure => new ButtonStructure()
        {
            TopButtonCode = ButtonCode.FileMenu,
            PreviousButtonCode = ButtonCode.Menu,
            MenuButtons = new Collection<ButtonCode>
            {
                ButtonCode.ToJson,
                ButtonCode.FromJson,
            },
        };

        public ButtonCode TopButtonCode { get; private set; }

        public IEnumerable<ButtonCode> MenuButtons { get; private set; }

        public ButtonCode PreviousButtonCode { get; private set; }
    }
}
