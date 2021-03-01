using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Chartix.Core.Validation;

namespace Chartix.Core.Entities
{
    [Table("Source")]
    public class Source : BaseEntity
    {
        private readonly ICollection<Metric> _metrics;

        public Source(long externalId, string name)
        {
            ExternalId = externalId;
            Name = Check.NullIfEmpty(name) ?? throw new ArgumentNullException(nameof(name));

            _metrics = new List<Metric>();
        }

        [Required]
        public long ExternalId { get; private set; }

        [Required]
        public string Name { get; private set; }

        public StateType State { get; private set; }

        public DateTime LastActionDate { get; private set; }

        public IEnumerable<Metric> Metrics { get => _metrics; }

        public void UpdateState(StateType newState) => State = newState;

        public void UpdateLastActionDate() => LastActionDate = DateTime.UtcNow;
    }
}
