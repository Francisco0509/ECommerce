using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceRazor.DataAccess.Repository.IRepository
{
    public interface IRepository<T> where T : class
    {
        IEnumerable<T> GetAll(string? includeProperties = null);
        void Add(T entity);
        //void Update(T entity);
        void Remove(T entity);
        //Borrar varios registros a la vez
        void RemoveRange(IEnumerable<T> entities);
        T GetFirstOrDefaul(Expression<Func<T, bool>> ? filter = null, string? includeProperties = null);
        bool NameExists(string name);
    }
}
