using ECommerceRazor.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceRazor.DataAccess.Repository.IRepository
{
    public interface ICarritoCompraRepository : IRepository<CarritoCompra>
    {
        //Incrementar la cantidad del producto en el carrito de compra
        int IncrementarContador(CarritoCompra carritoCompra, int contador);
        //Decrementar la cantidad del producto en el carrito de compra
        int DecrementarContador(CarritoCompra carritoCompra, int contador);
    }
}
