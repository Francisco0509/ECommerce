using ECommerceRazor.DataAccess;
using ECommerceRazor.DataAccess.Repository.IRepository;
using ECommerceRazor.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ECommerceRazor.Pages.Admin.Categorias
{
    public class CrearModel : PageModel
    {
        private readonly IUnitOfWork _unitOfWork;
        public CrearModel(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [BindProperty]
        public Categoria Categoria { get; set; } = default!;

        public IActionResult OnGet()
        {
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            //Validaci�n personalizada para saber si el nombre ya existe en la bd
            //bool nombreExiste = _context.Categorias.Any(c => c.Nombre == Categoria.Nombre);
            //if (nombreExiste)
            //{
            //    ModelState.AddModelError("Categoria.Nombre", "El nombre de la categor�a ya existe. Por favor elige otro nombre.");                
            //    return Page();
            //}

            //Validaci�n personalizada para saber si el nombre ya existe en la bd V2.0 con UnitOfWork
            if (_unitOfWork.Categoria.NameExists(Categoria.Nombre))
            {
                ModelState.AddModelError("Categoria.Nombre", "El nombre de la categor�a ya existe. Por favor elige otro nombre.");
                return Page();
            }

            //Validamos que el modelo sea correcto(no falte alg�n dato)
            if (!ModelState.IsValid)
            {
                return Page();
            }
                

            _unitOfWork.Categoria.Add(Categoria);
            _unitOfWork.Save();

            //Regresar mensaje 
            TempData["Success"] = "Categor�a creada con �xito.";

            return RedirectToPage("Index");
        }
    }
}
