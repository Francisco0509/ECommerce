using ECommerceRazor.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceRazor.DataAccess.Repository.IRepository
{
    public interface IDetalleOrdenRepository : IRepository<DetalleOrden>
    {
        void Update(DetalleOrden detalleOrden);
    }
}
