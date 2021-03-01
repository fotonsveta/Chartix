using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Chartix.Core.Entities;

namespace Chartix.Core.Models
{
    public class JsonMetric
    {
        public JsonMetric()
        {
            Values = new Collection<JsonValue>();
        }

        public string Name { get; set; }

        public string Unit { get; set; }

        public ICollection<JsonValue> Values { get; set; }

        public Metric ConvertTo()
        {
            var metric = new Metric(Name, Unit);
            foreach (var value in Values)
            {
                metric.AddValue(value.ConvertTo());
            }

            return metric;
        }

        public class JsonValue
        {
            public double Content { get; set; }

            public DateTime ValueDate { get; set; }

            public Value ConvertTo() => new Value(Content, ValueDate);
        }
    }
}
