using System.Collections.Generic;
using Chartix.Core.Entities;
using Chartix.Core.Interfaces;
using Chartix.Core.Specifications;
using Chartix.Infrastructue.Repositories;
using Chartix.Infrastructure.Db;

namespace Chartix.Infrastructure.Repositories
{
    public class ValueRepository : Repository<Value>, IValueRepository
    {
        private readonly AppDbContext _context;

        public ValueRepository(AppDbContext context)
            : base(context)
        {
            _context = context;
        }

        public IEnumerable<Value> GetByMetricId(long metricId, int currentPageNumber, int pageSize = int.MaxValue)
        {
            return FindNoTracking(
                new MetricIdValueSpecification(metricId),
                new ValueDateOrderBySpecification(),
                currentPageNumber,
                pageSize);
        }

        public int CountByMetric(long metricId)
        {
            return Count(new MetricIdValueSpecification(metricId));
        }
    }
}
