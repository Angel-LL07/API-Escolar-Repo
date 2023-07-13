using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace API.Persistencia
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private APIContext _contex;
        private DbSet<T> _dbSet = null;
        public GenericRepository(APIContext contex)
        {
            _contex = contex;
            _dbSet = _contex.Set<T>();
        }
        public async Task<T> ObtenerPorIdAsync(object id)
        {
            return await _dbSet.FindAsync(id);
        }
        public async Task<T> ObtenerAsync(Expression<Func<T, bool>> match = null, string IncludeProperties = "")
        {
            IQueryable<T> query = _dbSet;
            if (match != null)
                query = query.Where(match);
            foreach (var item in IncludeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(item);
            }

            return await query.FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<T>> ObtenerTodosAsync(Expression<Func<T, bool>> match = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, string IncludeProperties = "")
        {
            IQueryable<T> query = _dbSet;
            if (match != null)
                query = query.Where(match);
            foreach (var item in IncludeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(item);
            }

            if (orderBy != null)
            {
                return await orderBy(query).ToListAsync();
            }
            else
            {
                return await query.ToListAsync();
            }

        }

        public async Task<T> AgregarAsin(T entity)
        {
            await _contex.Set<T>().AddAsync(entity);
            return entity;
        }

        public async Task<T> ActualizarAsync(T entity, int key)
        {
            if (entity == null)
                return null;
            T existe = await _dbSet.FindAsync(key);
            if (existe != null)
            {
                _contex.Entry(existe).CurrentValues.SetValues(entity);
            }
            return existe;
        }

        public void EliminarAsyn(T entity)
        {
            _contex.Set<T>().Remove(entity);
        }

        public DbSet<T> Entidades
        {
            get
            {
                if(_dbSet == null)
                {
                    this._dbSet = _contex.Set<T>();
                }
                return _dbSet;
            }
        }


    }
}
