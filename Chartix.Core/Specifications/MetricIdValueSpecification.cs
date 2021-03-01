using System;
using System.Linq.Expressions;
using Chartix.Core.Entities;

namespace Chartix.Core.Specifications
{
    public class MetricIdValueSpecification : Specification<Value>
    {
        private readonly long _metricId;

        public MetricIdValueSpecification(long metricId)
        {
            _metricId = metricId;
        }

        public override Expression<Func<Value, bool>> ToExpression()
        {
            return p => p.MetricId == _metricId;
        }
    }
}
