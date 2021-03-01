using System.Collections.Generic;
using Chartix.Core.Entities;

namespace Chartix.Core.Interfaces
{
    public interface IValueRepository : IRepository<Value>
    {
        IEnumerable<Value> GetByMetricId(long metricId, int currentPageNumber, int pageSize = int.MaxValue);

        int CountByMetric(long metricId);
    }
}
