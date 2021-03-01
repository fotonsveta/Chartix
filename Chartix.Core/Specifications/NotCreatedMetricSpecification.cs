﻿using System;
using System.Linq.Expressions;
using Chartix.Core.Entities;

namespace Chartix.Core.Specifications
{
    public class NotCreatedMetricSpecification : Specification<Metric>
    {
        private readonly long _sourceId;

        public NotCreatedMetricSpecification(long sourceId)
        {
            _sourceId = sourceId;
        }

        public override Expression<Func<Metric, bool>> ToExpression()
        {
            return p => p.SourceId == _sourceId && !p.IsCreated;
        }
    }
}
