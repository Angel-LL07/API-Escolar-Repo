using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace API.Persistencia
{
    public interface IGenericRepository <T> where T : class
    {
        Task<T> ObtenerPorIdAsync(object id);
        Task<T> ObtenerAsync(Expression<Func<T,bool>> match = null, string IncludeProperties="");
        Task<IEnumerable<T>> ObtenerTodosAsync(Expression<Func<T,bool>> match = null, Func<IQueryable<T>,IOrderedQueryable<T>> orderBy= null,string IncludeProperties="");
        Task<T> AgregarAsin(T entity);
        Task<T>ActualizarAsync(T entity,int key);
        void EliminarAsyn(T entity);
    }
}
