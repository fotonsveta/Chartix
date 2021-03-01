using System.Linq;
using Chartix.Core.Entities;
using Chartix.Core.Interfaces;
using Chartix.Core.Specifications;
using Chartix.Infrastructue.Repositories;
using Chartix.Infrastructure.Db;

namespace Chartix.Infrastructure.Repositories
{
    public class SourceRepository : Repository<Source>, ISourceRepository
    {
        private readonly AppDbContext _context;

        public SourceRepository(AppDbContext context)
            : base(context)
        {
            _context = context;
        }

        public Source GetByExternalId(long externalId)
        {
            return _context.Sources
                .Where(new ExternalIdSourceSpecification(externalId).ToExpression())
                .FirstOrDefault();
        }

        public Source FindOrCreateNew(long externalId, string name)
        {
            var source = GetByExternalId(externalId);
            if (source == null)
            {
                var newSource = new Source(externalId, name);
                source = Add(newSource);
            }

            return source;
        }
    }
}
