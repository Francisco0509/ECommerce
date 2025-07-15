using ECommerceRazor.DataAccess.Repository.IRepository;
using ECommerceRazor.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceRazor.DataAccess.Repository
{
    public class CarritoCompraRepository : Repository<CarritoCompra>, ICarritoCompraRepository
    {
        private readonly AppDbContext _context;
        public CarritoCompraRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public int DecrementarContador(CarritoCompra carritoCompra, int contador)
        {
            carritoCompra.Cantidad -= contador;
            _context.SaveChanges();
            return carritoCompra.Cantidad;
        }

        public int IncrementarContador(CarritoCompra carritoCompra, int contador)
        {
            carritoCompra.Cantidad += contador;
            _context.SaveChanges();
            return carritoCompra.Cantidad;
        }
    }
}
