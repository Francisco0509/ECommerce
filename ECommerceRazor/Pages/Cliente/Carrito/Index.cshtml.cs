using ECommerceRazor.DataAccess.Repository.IRepository;
using ECommerceRazor.Models;
using ECommerceRazor.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;

namespace ECommerceRazor.Pages.Cliente.Carrito
{
    [Authorize(Roles = "Cliente")]
    public class IndexModel : PageModel
    {
        public IEnumerable<CarritoCompra> ListaCarritoCompra { get; set; }
        public double TotalCarrito { get; set; }
        private readonly IUnitOfWork _unitOfWork;
        public IndexModel(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            TotalCarrito = 0;
        }

        

        public void OnGet()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            if (claim != null)
            {
                ListaCarritoCompra = _unitOfWork.CarritoCompra.GetAll(
                    filter: c => c.ApplicationUserId == claim.Value,
                    includeProperties: "Producto,Producto.Categoria");
                foreach (var itemCarrito in ListaCarritoCompra)
                {
                    TotalCarrito += (double)(itemCarrito.Cantidad * itemCarrito.Producto.Precio);   
                }
            }
        }

        public IActionResult OnPostMas(int carritoId)
        { 
            var carrito = _unitOfWork.CarritoCompra.GetFirstOrDefault(c => c.Id == carritoId);
            var producto = _unitOfWork.Producto.GetFirstOrDefault(c => c.Id == carrito.ProductoId);
            //Restamos cantidad del inventario
            producto.Stock -= 1;

            _unitOfWork.CarritoCompra.IncrementarContador(carrito, 1);

            
            return RedirectToPage("/Cliente/Carrito/Index");
        }

        public IActionResult OnPostMenos(int carritoId)
        {
            var carrito = _unitOfWork.CarritoCompra.GetFirstOrDefault(c => c.Id == carritoId);
            var producto = _unitOfWork.Producto.GetFirstOrDefault(c => c.Id == carrito.ProductoId);
            //Sumamos cantidad al inventario
            producto.Stock += 1;



            if (carrito.Cantidad == 1)
            {
                var contador = _unitOfWork.CarritoCompra.GetAll(u => u.ApplicationUserId == carrito.ApplicationUserId).ToList().Count - 1;
                _unitOfWork.CarritoCompra.Remove(carrito);
                _unitOfWork.Save();
                HttpContext.Session.SetInt32(CNT.CarritoSesion, contador);
            }
            else
            {
                _unitOfWork.CarritoCompra.DecrementarContador(carrito, 1);
            }
            
            return RedirectToPage("/Cliente/Carrito/Index");
        }

        public IActionResult OnPostEliminar(int carritoId)
        {
            var carrito = _unitOfWork.CarritoCompra.GetFirstOrDefault(c => c.Id == carritoId);
            var producto = _unitOfWork.Producto.GetFirstOrDefault(c => c.Id == carrito.ProductoId);
            //Regresamos la cantidad al inventario
            producto.Stock += carrito.Cantidad;

            _unitOfWork.CarritoCompra.Remove(carrito);

            //Session
            var contador = _unitOfWork.CarritoCompra.GetAll(u => u.ApplicationUserId == carrito.ApplicationUserId).ToList().Count-1;



            _unitOfWork.Save();

            HttpContext.Session.SetInt32(CNT.CarritoSesion,contador);

            return RedirectToPage("/Cliente/Carrito/Index");
        }
    }
}
