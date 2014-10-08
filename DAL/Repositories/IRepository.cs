using System;
namespace DAL.Repositories
{
    interface IRepository<T> where T : class
    {
        void Delete(object id);
        void Delete(T entityToDelete);
        void DeleteAll();
        System.Collections.Generic.IEnumerable<T> Get(System.Linq.Expressions.Expression<Func<T, bool>> filter = null, Func<System.Linq.IQueryable<T>, System.Linq.IOrderedQueryable<T>> orderBy = null, string includeProperties = "");
        System.Collections.Generic.IEnumerable<T> GetLocal(System.Linq.Expressions.Expression<Func<T, bool>> filter = null, Func<System.Linq.IQueryable<T>, System.Linq.IOrderedQueryable<T>> orderBy = null, string includeProperties = "");
        T GetByID(object id);
        void Insert(T entity);
        void Update(T entityToUpdate);
        void LoadReference(T entity, string navigationProperty);

    }
}
