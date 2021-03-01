using System;
using System.Linq.Expressions;
using Chartix.Core.Entities;

namespace Chartix.Core.Specifications
{
    public class ExternalIdSourceSpecification : Specification<Source>
    {
        private readonly long _externalId;

        public ExternalIdSourceSpecification(long externalId)
        {
            _externalId = externalId;
        }

        public override Expression<Func<Source, bool>> ToExpression()
        {
            return p => p.ExternalId == _externalId;
        }
    }
}
