using System;
using System.Linq.Expressions;
using Chartix.Core.Entities;

namespace Chartix.Core.Specifications
{
    public class ValueDateOrderBySpecification : OrderBySpecification<Value, DateTime>
    {
        public override Expression<Func<Value, DateTime>> ToExpression()
        {
            return p => p.ValueDate;
        }
    }
}
