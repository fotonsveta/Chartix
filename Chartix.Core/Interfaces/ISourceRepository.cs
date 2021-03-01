using Chartix.Core.Entities;

namespace Chartix.Core.Interfaces
{
    public interface ISourceRepository : IRepository<Source>
    {
        Source GetByExternalId(long externalId);

        Source FindOrCreateNew(long externalId, string name);
    }
}
