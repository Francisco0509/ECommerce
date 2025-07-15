using ECommerceRazor.DataAccess.Repository.IRepository;
using ECommerceRazor.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceRazor.DataAccess.Repository
{
    public class OrdenRepository : Repository<Orden>, IOrdenRepository
    {
        private readonly AppDbContext _context;
        public OrdenRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public void Update(Orden orden)
        {
            var objDesdeBd = _context.Ordenes.FirstOrDefault(c => c.Id == orden.Id);
            _context.Ordenes.Update(objDesdeBd);
        }
    }
}
