using System.Collections.Generic;
using Chartix.Core.Entities;

namespace Chartix.Core.Interfaces
{
    public interface IMetricRepository : IRepository<Metric>
    {
        IEnumerable<Metric> GetBySourceId(long sourceId);

        IEnumerable<Metric> GetBySourceId(long sourceId, int currentPageNumber, int pageSize = int.MaxValue);

        void UpdateMetricData(Source source, IEnumerable<Metric> newMetrics);

        bool SetMainMetricIfHasNo(long sourceId);

        Metric GetMainBySourceId(long sourceId);

        Metric GetMainByExternalId(long externalId);

        Metric GetNotCreatedMetric(long sourceId);

        IEnumerable<Metric> GetBySourceIdWithValues(long sourceId);

        int CountBySourceId(long sourceId);
    }
}
