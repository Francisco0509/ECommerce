using ECommerceRazor.DataAccess.Repository.IRepository;
using ECommerceRazor.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceRazor.DataAccess.Repository
{
    public class DetalleOrdenRepository : Repository<DetalleOrden>, IDetalleOrdenRepository
    {
        private readonly AppDbContext _context;
        public DetalleOrdenRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public void Update(DetalleOrden detalleOrden)
        {
            var objDesdeBd = _context.DetallesOrden.FirstOrDefault(c => c.Id == detalleOrden.Id);
            _context.DetallesOrden.Update(objDesdeBd);
        }
    }
}
