using System.Collections.Generic;
using System.Linq;
using Chartix.Core.Entities;
using Chartix.Infrastructure.Telegram.Models;

namespace Chartix.Infrastructure.Telegram.Services
{
    public class PlotDataFactory
    {
        public PlotData ValuesTo(IEnumerable<Value> values, string name)
        {
            int count = values == null ? 0 : values.Count();
            if (values == null || count == 0)
            {
                return PlotData.Empty;
            }

            var xs = new double[count];
            var ys = new double[count];
            int i = 0;

            foreach (var value in values)
            {
                xs[i] = value.ValueDate.ToOADate();
                ys[i] = value.Content;
                i++;
            }

            return new PlotData(xs, ys, name);
        }
    }
}
