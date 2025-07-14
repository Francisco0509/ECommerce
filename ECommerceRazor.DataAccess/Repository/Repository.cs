using ECommerceRazor.DataAccess.Repository.IRepository;
using ECommerceRazor.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceRazor.DataAccess.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly AppDbContext _context;
        internal DbSet<T> _dbSet;
        public Repository(AppDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }
        public void Add(T entity)
        {
            _dbSet.Add(entity);
        }

        public IEnumerable<T> GetAll()
        {
            IQueryable<T> queryable = _dbSet;
            return queryable.ToList();
        }

        public T GetFirstOrDefaul(Expression<Func<T, bool>>? filter = null)
        {
            IQueryable<T> queryable = _dbSet;
            if (filter != null)
            {
                queryable = queryable.Where(filter);
            }

            return queryable.FirstOrDefault();
        }

        public void Remove(T entity)
        {
            _dbSet.Remove(entity);
        }

        public void RemoveRange(IEnumerable<T> entities)
        {
            _dbSet.RemoveRange(entities);
        }

        public bool NameExists(string name)
        {

            return _context.Productos.Any(c => c.Nombre == name);

        }
    }
}
