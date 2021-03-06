using System;
using System.Collections.Generic;
using System.Linq;
using Chartix.Core.Entities;
using Chartix.Core.Interfaces;
using Chartix.Core.Specifications;
using Chartix.Infrastructure.Db;
using Microsoft.EntityFrameworkCore;

namespace Chartix.Infrastructue.Repositories
{
    public class Repository<T> : IRepository<T>
        where T : BaseEntity
    {
        private readonly AppDbContext _context;
        private readonly DbSet<T> _dbSet;

        public Repository(AppDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public T GetById(long id)
        {
            return _dbSet.Find(id);
        }

        public IEnumerable<T> Find(Specification<T> specification)
        {
            return _dbSet.Where(specification.ToExpression()).AsEnumerable();
        }

        public T FindSingle(Specification<T> specification)
        {
            return _dbSet.FirstOrDefault(specification.ToExpression());
        }

        public IEnumerable<T> FindNoTracking(Specification<T> specification)
        {
            return _dbSet.Where(specification.ToExpression()).AsNoTracking().AsEnumerable();
        }

        public IEnumerable<T> FindNoTracking<TKey>(
            Specification<T> specification = null,
            OrderBySpecification<T, TKey> orderBySpecification = null,
            int pageNumber = 1,
            int pageSize = int.MaxValue)
        {
            var query = _dbSet as IQueryable<T>;

            if (specification != null)
            {
                query = query.Where(specification.ToExpression());
            }

            if (orderBySpecification != null)
            {
                query = query.OrderBy(orderBySpecification.ToExpression());
            }

            query = query.Skip((pageNumber - 1) * pageSize).Take(pageSize);

            return query.AsNoTracking()
                        .AsEnumerable();
        }

        public int Count(Specification<T> specification = null)
        {
            return specification != null ?
                _dbSet.Where(specification.ToExpression()).Count() :
                _dbSet.Count();
        }

        public T Add(T entity)
        {
            if (entity is null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            var entityEntry = _dbSet.Add(entity);
            _context.SaveChanges();
            return entityEntry.Entity;
        }

        public void Update(T entity)
        {
            if (entity is null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            _dbSet.Update(entity);
            _context.SaveChanges();
        }

        public void Remove(T entity)
        {
            if (entity is null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            _dbSet.Remove(entity);
            _context.SaveChanges();
        }

        public void Remove(long id)
        {
            T entity = _dbSet.Find(id);
            Remove(entity);
        }
    }
}
