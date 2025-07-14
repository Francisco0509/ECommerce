using ECommerceRazor.DataAccess.Repository.IRepository;
using ECommerceRazor.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceRazor.DataAccess.Repository
{
    public class ProductoRepository : Repository<Producto>, IProductoRepository
    {
        private readonly AppDbContext _context;
        public ProductoRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }       

        public void Update(Producto producto)
        {
            var objDesdeBd = _context.Productos.FirstOrDefault(c => c.Id == producto.Id);
            objDesdeBd.Nombre = producto.Nombre;
            objDesdeBd.Descripcion = producto.Descripcion;
            objDesdeBd.Precio = producto.Precio;
            objDesdeBd.Stock = producto.Stock;
            objDesdeBd.CategoriaId = producto.CategoriaId;
            if (objDesdeBd.Imagen != null)
            { 
                objDesdeBd.Imagen = producto.Imagen;
            }
        }
    }
}
