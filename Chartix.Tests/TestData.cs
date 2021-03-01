using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Chartix.Core.Entities;

namespace Chartix.Tests
{
    public class TestData
    {
        public const int Code1 = 111;
        public const int Code2 = 222;

        private Source _source;

        public Source Source => _source = new Source(1, "qwe");

        public IEnumerable<Metric> GetMetrics(int code)
        {
            switch (code)
            {
                case Code1:
                    return GetMetrics111();
                case Code2:
                    return GetMetrics222();
            }

            throw new Exception("unknown code");
        }

        private IEnumerable<Metric> GetMetrics111()
        {
            var values1 = new Collection<Value>
            {
                NewValue(50, new DateTime(2020, 01, 01)),
                NewValue(52, new DateTime(2020, 01, 02)),
            };

            var values2 = new Collection<Value>
            {
                NewValue(101, new DateTime(2020, 02, 01)),
                NewValue(105, new DateTime(2020, 02, 02)),
                NewValue(110, new DateTime(2020, 02, 03)),
            };

            var metric1 = new Metric("Weight", "Kg");
            metric1.UpdateSource(Source);
            foreach (var val in values1)
            {
                metric1.AddValue(val);
            }

            var metric2 = new Metric("Growth", "Cm");
            metric2.UpdateSource(Source);
            foreach (var val in values2)
            {
                metric2.AddValue(val);
            }

            return new List<Metric>() { metric1, metric2 };
        }

        private IEnumerable<Metric> GetMetrics222()
        {
            var source = new Source(1, "qwe");

            var metric1 = new Metric("Height", "Cm");
            metric1.UpdateSource(source);

            var metric2 = new Metric("Weight", "Kg");
            metric2.UpdateSource(source);

            var metric3 = new Metric("Time", "Sec");
            metric2.UpdateSource(source);

            return new List<Metric>() { metric1, metric2, metric3 };
        }

        private Value NewValue(double content, DateTime date) => new Value(content, date);
    }
}
