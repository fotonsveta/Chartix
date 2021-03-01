using System.Collections.Generic;
using Chartix.Core.Entities;
using Chartix.Core.Specifications;

namespace Chartix.Core.Interfaces
{
    public interface IRepository<T>
        where T : BaseEntity
    {
        T GetById(long id);

        T FindSingle(Specification<T> specification);

        IEnumerable<T> FindNoTracking(Specification<T> specification);

        IEnumerable<T> FindNoTracking<TKey>(
            Specification<T> specification = null,
            OrderBySpecification<T, TKey> orderBySpecification = null,
            int pageNumber = 1,
            int pageSize = int.MaxValue);

        int Count(Specification<T> specification = null);

        T Add(T entity);

        void Update(T entity);

        void Remove(T entity);

        void Remove(long id);
    }
}
