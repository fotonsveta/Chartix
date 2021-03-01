using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Chartix.Core.Entities;
using Chartix.Core.Interfaces;
using Chartix.Core.Specifications;
using Chartix.Infrastructue.Repositories;
using Chartix.Infrastructure.Db;
using Microsoft.EntityFrameworkCore;

namespace Chartix.Infrastructure.Repositories
{
    public class MetricRepository : Repository<Metric>, IMetricRepository
    {
        private readonly AppDbContext _context;

        public MetricRepository(AppDbContext context)
            : base(context)
        {
            _context = context;
        }

        public IEnumerable<Metric> GetBySourceId(long sourceId, int currentPageNumber, int pageSize = int.MaxValue)
        {
            return FindNoTracking(
                new SourceIdMetricSpecification(sourceId),
                new CreatedDateOrderBySpecification<Metric>(),
                currentPageNumber,
                pageSize);
        }

        public IEnumerable<Metric> GetBySourceId(long sourceId)
        {
            return FindNoTracking(new SourceIdMetricSpecification(sourceId));
        }

        public Metric GetMainBySourceId(long sourceId)
        {
            return Find(new SourceIdMainMetricSpecification(sourceId)).FirstOrDefault();
        }

        public Metric GetNotCreatedMetric(long sourceId)
        {
            return Find(new NotCreatedMetricSpecification(sourceId)).FirstOrDefault();
        }

        public Metric GetMainByExternalId(long externalId)
        {
            var source = _context.Sources
                    .Where(new ExternalIdSourceSpecification(externalId).ToExpression())
                    .FirstOrDefault()
                    ?? throw new ArgumentException($"Error, there is no source with chat id {externalId}");

            return GetMainBySourceId(source.Id);
        }

        /// <summary>
        /// If there is no main metric, set it.
        /// </summary>
        /// <param name="sourceId">Source id.</param>
        /// <returns>Successfully installed.</returns>
        public bool SetMainMetricIfHasNo(long sourceId)
        {
            var metrics = GetBySourceId(sourceId);
            if (!metrics.Any())
            {
                return false;
            }

            if (!metrics.Any(x => x.IsMain))
            {
                var metricId = metrics.First().Id;
                var metric = GetById(metricId);
                metric.UpdateMain(true);
                Update(metric);
            }

            return true;
        }

        public void UpdateMetricData(Source source, IEnumerable<Metric> newMetrics)
        {
            if (source == null || newMetrics == null || !newMetrics.Any())
            {
                return;
            }

            var metrics = GetBySourceIdWithValues(source.Id);
            if (metrics == null)
            {
                metrics = new Collection<Metric>();
            }

            foreach (var newMetric in newMetrics)
            {
                var dbMetric = metrics.FirstOrDefault(x => newMetric.HasSameNameUnit(x));
                if (dbMetric == null)
                {
                    newMetric.UpdateSource(source);
                    newMetric.SetCreated();
                    Add(newMetric);
                }
                else if (newMetric.HasAnyValue())
                {
                    bool needUpdate = false;
                    foreach (var newValue in newMetric.Values)
                    {
                        var sameValue = dbMetric.Values.FirstOrDefault(x => newValue.HasSameContent(x));
                        if (sameValue == null)
                        {
                            dbMetric.AddValue(newValue);
                            needUpdate = true;
                        }
                    }

                    if (needUpdate)
                    {
                        Update(dbMetric);
                    }
                }
            }
        }

        public IEnumerable<Metric> GetBySourceIdWithValues(long sourceId)
        {
            return _context.Metrics
                .Include(u => u.Values)
                .Where(new SourceIdMetricSpecification(sourceId).ToExpression())
                .AsNoTracking().AsEnumerable();
        }

        public int CountBySourceId(long sourceId)
        {
            return Count(new SourceIdMetricSpecification(sourceId));
        }
    }
}
