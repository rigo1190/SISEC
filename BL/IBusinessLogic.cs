using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public interface IBusinessLogic<T> where T : class
    {     
        bool Delete(object id);
        bool Delete(T entityToDelete);
        bool DeleteAll();
        System.Linq.IQueryable<T> Get(System.Linq.Expressions.Expression<Func<T, bool>> filter = null, Func<System.Linq.IQueryable<T>, System.Linq.IOrderedQueryable<T>> orderBy = null, string includeProperties = "");
        System.Linq.IQueryable<T> GetLocal(System.Linq.Expressions.Expression<Func<T, bool>> filter = null, Func<System.Linq.IQueryable<T>, System.Linq.IOrderedQueryable<T>> orderBy = null, string includeProperties = "");
        T GetByID(object id);
        bool Insert(T entity);
        bool Update(T entityToUpdate);

        bool LoadReference(T entity, string navigationProperty);
       
    }
}
