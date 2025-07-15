using ECommerceRazor.DataAccess.Repository.IRepository;
using ECommerceRazor.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;

namespace ECommerceRazor.Pages.Cliente.Inicio
{
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
            CarritoCompra = new() {
                Producto = _unitOfWork.Producto.GetFirstOrDefaul(p => p.Id == id, "Categoria")
            };
            if (CarritoCompra == null)
            {
                return RedirectToPage("/Index");
            }
            return Page();
        }

        //public IActionResult OnPostAgregarAlCarrito()
        //{
        //    if (CarritoCompra.Cantidad < 1 || CarritoCompra.Cantidad > Producto.Stock)
        //    {
        //        ModelState.AddModelError("Cantidad", $"Debe ingresar un valor entre 1 y {Producto.Stock}");
        //        return Page();
        //    }

        //    //Aquí se maneja la lógica del carrito
        //    TempData["Success"] = $"{CarritoCompra.Cantidad} unidad(es) del producto {Producto.Nombre} añadido al carrito. ";
        //    return RedirectToPage("/Cliente/Inicio/Index");
        //}
    }
}
