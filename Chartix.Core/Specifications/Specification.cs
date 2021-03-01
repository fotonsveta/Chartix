using System;
using System.Linq.Expressions;
using Chartix.Core.Entities;

namespace Chartix.Core.Specifications
{
    public abstract class Specification<T>
        where T : BaseEntity
    {
        public abstract Expression<Func<T, bool>> ToExpression();

        public bool IsSatisfiedBy(T entity)
        {
            Func<T, bool> predicate = ToExpression().Compile();
            return predicate(entity);
        }
    }
}
