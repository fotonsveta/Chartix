using System.ComponentModel.DataAnnotations.Schema;

namespace Chartix.Core.Entities
{
    [Table("ProcessedUpdate")]
    public class ProcessedUpdate : BaseEntity
    {
        public ProcessedUpdate(int updateId, long externalId)
        {
            UpdateId = updateId;
            ExternalId = externalId;
        }

        public int UpdateId { get; private set; }

        public long ExternalId { get; private set; }
    }
}
