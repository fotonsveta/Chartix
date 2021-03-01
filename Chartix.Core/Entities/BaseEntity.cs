using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Chartix.Core.Entities
{
    public abstract class BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [JsonIgnore]
        public long Id { get; private set; }

        [JsonIgnore]
        public bool IsDeleted { get; private set; }

        [JsonIgnore]
        public DateTime CreatedDate { get; private set; } = DateTime.UtcNow;

        public void SetDeleted() => IsDeleted = true;
    }
}
