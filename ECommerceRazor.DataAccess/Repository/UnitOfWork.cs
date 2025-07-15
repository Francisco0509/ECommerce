using ECommerceRazor.DataAccess.Repository.IRepository;
using ECommerceRazor.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceRazor.DataAccess.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;
        public UnitOfWork(AppDbContext context)
        {
            _context = context;
            Categoria = new CategoriaRepository(_context);
            Producto = new ProductoRepository(_context);
            CarritoCompra = new CarritoCompraRepository(_context);
            Orden = new OrdenRepository(_context);
            DetalleOrden = new DetalleOrdenRepository(_context);
            ApplicationUser = new ApplicationUserRepository(_context);
        }

        public ICategoriaRepository Categoria { get; private set; }
        public IProductoRepository Producto { get; private set; }
        public ICarritoCompraRepository CarritoCompra { get; private set; }
        public IOrdenRepository Orden { get; private set; }
        public IDetalleOrdenRepository DetalleOrden { get; private set; }   
        public IApplicationUserRepository ApplicationUser { get; set; }
        public void Dispose()
        {
            _context.Dispose();
        }

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
