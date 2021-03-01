using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;
using Chartix.Core.Validation;

namespace Chartix.Core.Entities
{
    [Table("Metric")]
    public class Metric : BaseEntity
    {
        private readonly ICollection<Value> _values;

        public Metric(string name)
        {
            Name = Check.NullIfEmpty(name) ?? throw new ArgumentNullException(nameof(name));
            _values = new Collection<Value>();
        }

        [JsonConstructor]
        public Metric(string name, string unit)
            : this(name)
        {
            Unit = Check.NullIfEmpty(unit) ?? throw new ArgumentNullException(nameof(unit));
        }

        [Required]
        public string Name { get; private set; }

        public string Unit { get; private set; }

        [JsonIgnore]
        public bool IsMain { get; private set; }

        [JsonIgnore]
        public bool IsCreated { get; private set; }

        [JsonIgnore]
        public long SourceId { get; private set; }

        [JsonIgnore]
        public Source Source { get; private set; }

        public IEnumerable<Value> Values { get => _values; }

        public bool UpdateSource(Source source)
        {
            if (source == null)
            {
                return false;
            }

            Source = source;
            SourceId = source.Id;
            return true;
        }

        public void UpdateUnit(string unit) => Unit = unit;

        public void UpdateMain(bool newIsMain) => IsMain = newIsMain;

        public void AddValue(Value value) => _values.Add(value);

        public void SetCreated() => IsCreated = true;

        public override string ToString() => string.IsNullOrEmpty(Unit) ? Name : $"{Name} ({Unit})";

        public bool HasSameNameUnit(Metric other)
        {
            if (other == null)
            {
                return false;
            }

            return Name.Equals(other.Name) && Unit.Equals(other.Unit);
        }

        public bool HasAnyValue()
        {
            return Values != null && Values.Any();
        }
    }
}
