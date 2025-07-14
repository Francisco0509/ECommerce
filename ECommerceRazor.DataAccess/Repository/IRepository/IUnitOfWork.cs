using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceRazor.DataAccess.Repository.IRepository
{
    public interface IUnitOfWork : IDisposable
    {
        ICategoriaRepository Categoria { get; }
        IProductoRepository Producto { get; }
        void Save();
    }
}
