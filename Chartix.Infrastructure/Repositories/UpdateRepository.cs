using System.Linq;
using Chartix.Core.Entities;
using Chartix.Core.Interfaces;
using Chartix.Core.Specifications;
using Chartix.Infrastructue.Repositories;
using Chartix.Infrastructure.Db;

namespace Chartix.Infrastructure.Repositories
{
    public class UpdateRepository : Repository<ProcessedUpdate>, IUpdateRepository
    {
        private readonly AppDbContext _context;

        public UpdateRepository(AppDbContext context)
            : base(context)
        {
            _context = context;
        }

        public ProcessedUpdate Get(int updateId, long externalId)
        {
            return FindSingle(new UniqueUpdateSpecification(updateId, externalId));
        }
    }
}
