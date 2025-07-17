using ECommerceRazor.DataAccess.Repository.IRepository;
using ECommerceRazor.Models;
using ECommerceRazor.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using System.Security.Claims;

namespace ECommerceRazor.Pages.Cliente.Inicio
{
    [Authorize]
    public class DetalleModel : PageModel
    {
        private readonly IUnitOfWork _unitOfWork;
        public DetalleModel(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [BindProperty]
        public CarritoCompra CarritoCompra { get; set; }


        public IActionResult OnGet(int id)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            CarritoCompra = new() {
                ApplicationUserId = claim.Value,
                Producto = _unitOfWork.Producto.GetFirstOrDefault(p => p.Id == id, "Categoria"),
                ProductoId = id,
            };
            if (CarritoCompra == null)
            {
                return RedirectToPage("/Index");
            }
            return Page();
        }

        public IActionResult OnPost()
        {
            

            if (ModelState.IsValid)
            {
                //Cargar el producto 
                var producto = _unitOfWork.Producto.GetFirstOrDefault(p => p.Id == CarritoCompra.ProductoId);

                if (producto == null)
                {
                    return NotFound("No se encontró el producto relacionado");
                }

                //Validar si el inventario es 0
                if (producto.Stock <= 0)
                {
                    TempData["Error"] = $"El producto no tiene inventario disponible.";
                    return RedirectToAction("Detalle", new { id = CarritoCompra.ProductoId });
                }

                //Validar que la cantidad no sea mayor a la cantidad de producto en inventario
                if (CarritoCompra.Cantidad < 1 || CarritoCompra.Cantidad > producto.Stock)
                {

                    //ModelState.AddModelError("Cantidad", $"Debe ingresar un valor entre 1 y {CarritoCompra.Producto.Stock}");
                    TempData["Error"] = $"Debe ingresar un valor entre 1 y {producto.Stock}";
                    return RedirectToAction("Detalle", new { id = CarritoCompra.ProductoId });
                }

                //Reducir la cantidad disponible del producto
                producto.Stock -= CarritoCompra.Cantidad;

                CarritoCompra carritoCompraDesdeBd = _unitOfWork.CarritoCompra.GetFirstOrDefault(
                    filter: c => c.ProductoId == CarritoCompra.ProductoId 
                    && c.ApplicationUserId == CarritoCompra.ApplicationUserId);

                if (carritoCompraDesdeBd == null)
                { 
                    _unitOfWork.CarritoCompra.Add(CarritoCompra);
                    _unitOfWork.Save();
                    TempData["Success"] = $"{CarritoCompra.Cantidad} unidad(es) añadido al carrito. ";

                    //Session
                    HttpContext.Session.SetInt32(CNT.CarritoSesion,
                        _unitOfWork.CarritoCompra.GetAll(u => u.ApplicationUserId == CarritoCompra.ApplicationUserId).ToList().Count());
                }
                else
                {
                    _unitOfWork.CarritoCompra.IncrementarContador(carritoCompraDesdeBd, CarritoCompra.Cantidad); 
                    TempData["Success"] = $"{CarritoCompra.Cantidad} unidad(es) actualizadas en el carrito. ";
                }

                return RedirectToPage("/Index");
            }
            
            return Page();
        }
    }
}
