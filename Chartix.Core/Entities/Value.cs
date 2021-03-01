using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Chartix.Core.Entities
{
    [Table("Value")]
    public class Value : BaseEntity
    {
        public Value(double content, DateTime valueDate)
        {
            Content = content;
            ValueDate = valueDate;
        }

        public double Content { get; private set; }

        public DateTime ValueDate { get; private set; }

        [JsonIgnore]
        public long MetricId { get; private set; }

        [JsonIgnore]
        public Metric Metric { get; private set; }

        public bool UpdateMetric(Metric metric)
        {
            if (metric == null)
            {
                return false;
            }

            Metric = metric;
            MetricId = metric.Id;
            return true;
        }

        public bool HasSameContent(Value other)
        {
            if (other == null)
            {
                return false;
            }

            return Content == other.Content && ValueDate == other.ValueDate;
        }

        public override string ToString() => $"{Content} ({ValueDate})";
    }
}
