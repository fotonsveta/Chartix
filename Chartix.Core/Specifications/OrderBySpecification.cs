using System;
using System.Linq.Expressions;
using Chartix.Core.Entities;

namespace Chartix.Core.Specifications
{
    public abstract class OrderBySpecification<T, TKey>
        where T : BaseEntity
    {
        public abstract Expression<Func<T, TKey>> ToExpression();

        public TKey GetOrderByKey(T entity)
        {
            Func<T, TKey> predicate = ToExpression().Compile();
            return predicate(entity);
        }
    }
}
