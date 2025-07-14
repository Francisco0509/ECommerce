using ECommerceRazor.DataAccess.Repository.IRepository;
using ECommerceRazor.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ECommerceRazor.Pages.Admin.Productos
{
    public class CrearModel : PageModel
    {
        private readonly IUnitOfWork _unitOfWork;
        public CrearModel(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [BindProperty]
        public Producto Producto { get; set; }
        public IActionResult OnGet()
        {
            return Page();
        }

        public IActionResult OnPost()
        {
            //Validación personalizada para saber si el nombre ya existe en la bd V2.0 con UnitOfWork
            if (_unitOfWork.Categoria.NameExists(Producto.Nombre))
            {
                ModelState.AddModelError("Producto.Nombre", "El nombre del producto ya existe. Por favor elige otro nombre.");
                return Page();
            }

            //Validamos que el modelo sea correcto(no falte algún dato)
            if (!ModelState.IsValid)
            {
                return Page();
            }

            if (ModelState.IsValid)
            {
                _unitOfWork.Producto.Add(Producto);
                _unitOfWork.Save();
                TempData["success"] = "Producto creado correctamente";
                return RedirectToPage("Index");
            }
            return Page();
        }
    }
}
