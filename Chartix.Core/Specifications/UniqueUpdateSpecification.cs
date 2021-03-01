using System;
using System.Linq.Expressions;
using Chartix.Core.Entities;

namespace Chartix.Core.Specifications
{
    public class UniqueUpdateSpecification : Specification<ProcessedUpdate>
    {
        private readonly int _updateId;
        private readonly long _externalId;

        public UniqueUpdateSpecification(int updateId, long externalId)
        {
            _updateId = updateId;
            _externalId = externalId;
        }

        public override Expression<Func<ProcessedUpdate, bool>> ToExpression()
        {
            return p => p.UpdateId == _updateId && p.ExternalId == _externalId;
        }
    }
}
