using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace NotPad.Core.Infrastructure
{
    public interface IRepository<T> where T : BaseEntity, IBaseEntity, new()
    {
        NotPadDbContext DataContext { get; }

        T Add(T entity);
        T Update(T entity);
        bool Delete(int id);
        bool Delete(T entity);
        T GetById(int Id);
        IEnumerable<T> GetAll();
        T Get(Expression<Func<T, bool>> where);
        IQueryable<T> GetMany(Expression<Func<T, bool>> where);
        IQueryable<T> Table { get; }
        IQueryable<T> TableNoTracking { get; }
        bool Any(Expression<Func<T, bool>> any);
        bool IsExist(Expression<Func<T, bool>> where);
    }

    public class RepositoryBase<T> : IRepository<T> where T : BaseEntity, IBaseEntity, new()
    {
        private NotPadDbContext dataContext;
        public readonly DbSet<T> dbset;
        public RepositoryBase(IDatabaseFactory databaseFactory)
        {
            DatabaseFactory = databaseFactory;
            dbset = DataContext.Set<T>();
        }

        public IDatabaseFactory DatabaseFactory
        {
            get;
            private set;
        }
        public NotPadDbContext DataContext
        {
            get { return dataContext ?? (dataContext = DatabaseFactory.Get()); }
        }

        public virtual T Add(T entity)
        {
            dbset.Add(entity);

            return entity;
        }
        public virtual T Update(T entity)
        {
            dbset.Attach(entity);
            var entry = DataContext.Entry<T>(entity).State = System.Data.Entity.EntityState.Modified;

            return entity;
        }

        public virtual bool Delete(int id)
        {
            T entity = GetById(id);
            bool result = Delete(entity);
            return result;
        }
        public virtual bool Delete(T entity)
        {
            if (entity is IDeletable && (entity as IDeletable).IsDeleted == false)
            {
                (entity as IDeletable).IsDeleted = true;
                entity = Update(entity);
            }
            else
            {
                dbset.Remove(entity);
                DataContext.SaveChanges();
            }

            return (entity as IDeletable).IsDeleted;
        }

        public T Get(Expression<Func<T, bool>> where)
        {
            return dbset.Where(where).AsNoTracking().FirstOrDefault<T>();
        }
        public virtual T GetById(int id)
        {
            return dbset.Where(a => a.Id == id).FirstOrDefault<T>();
        }
        public virtual IEnumerable<T> GetAll()
        {
            return from a in dbset.AsNoTracking() select a;
        }
        public virtual IQueryable<T> GetMany(Expression<Func<T, bool>> where)
        {
            return TableNoTracking.Where(where);
        }

        public virtual IQueryable<T> Table
        {
            get
            {
                return dbset;
            }
        }
        public virtual IQueryable<T> TableNoTracking
        {
            get
            {
                return dbset.AsNoTracking();
            }
        }

        public virtual bool Any(Expression<Func<T, bool>> any)
        {
            return dbset.Any(any);
        }
        public virtual bool IsExist(Expression<Func<T, bool>> where)
        {
            return dbset.Any(where);
        }

    }
}
