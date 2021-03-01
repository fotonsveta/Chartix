using Chartix.Core.Entities;

namespace Chartix.Core.Interfaces
{
    public interface IUpdateRepository : IRepository<ProcessedUpdate>
    {
        public ProcessedUpdate Get(int updateId, long externalId);
    }
}
