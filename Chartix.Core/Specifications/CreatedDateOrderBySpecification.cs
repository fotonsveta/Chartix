using System;
using System.Linq.Expressions;
using Chartix.Core.Entities;

namespace Chartix.Core.Specifications
{
    public class CreatedDateOrderBySpecification<T> : OrderBySpecification<T, DateTime>
        where T : BaseEntity
    {
        public override Expression<Func<T, DateTime>> ToExpression()
        {
            return p => p.CreatedDate;
        }
    }
}
