
using DAL;
using DAL.Repositories;
using System;
using System.Data.Entity;
using System.Linq;

namespace BL
{
    public class GenericBusinessLogic<T> : IBusinessLogic<T> where T : class
    {            

        internal DbContext  context;
        internal GenericRepository<T> repository;
        
        public GenericBusinessLogic(DbContext context)
        {
            
            this.context = context;
            this.repository = new GenericRepository<T>(context);
        }

        public virtual bool Delete(object id)
        {
            repository.Delete(id);
            return true;
        }

        public virtual bool Delete(T entityToDelete)
        {
            repository.Delete(entityToDelete);
            return true;
        }

        public virtual bool DeleteAll() 
        {
            repository.DeleteAll();
            return true;
        }

        public IQueryable<T> Get(System.Linq.Expressions.Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, string includeProperties = "")
        {
            return repository.Get(filter, orderBy, includeProperties).AsQueryable(); 
        }

        public IQueryable<T> GetLocal(System.Linq.Expressions.Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, string includeProperties = "")
        {
            return repository.GetLocal(filter, orderBy, includeProperties).AsQueryable();
        }

        public T GetByID(object id)
        {
            return repository.GetByID(id); 
        }

        public virtual bool Insert(T entity)
        {
            repository.Insert(entity);
            return true;

        }

        public virtual bool Update(T entityToUpdate)
        {
            repository.Update(entityToUpdate);
            return true;
        }

        public virtual bool LoadReference(T entity, string navigationProperty) 
        {
            repository.LoadReference(entity, navigationProperty);
            return true;
        }
    }
}
