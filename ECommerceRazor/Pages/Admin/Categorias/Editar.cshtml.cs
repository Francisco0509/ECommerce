using ECommerceRazor.DataAccess;
using ECommerceRazor.DataAccess.Repository.IRepository;
using ECommerceRazor.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ECommerceRazor.Pages.Admin.Categorias
{
    public class EditarModel : PageModel
    {
        private readonly IUnitOfWork _unitOfWork;
        public EditarModel(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [BindProperty]
        public Categoria Categoria { get; set; } = default!;
        public async Task<IActionResult> OnGetAsync(int id)
        {
            // Verificamos si la categoría existe
            Categoria = _unitOfWork.Categoria.GetFirstOrDefaul(c => c.Id == id);
            if (Categoria == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
                
            var categoriaBD = _unitOfWork.Categoria.GetFirstOrDefaul(c => c.Id == Categoria.Id);
            if (categoriaBD == null)
            {
                return NotFound();
            }
                
            categoriaBD.Nombre = Categoria.Nombre;
            categoriaBD.OrdenVisualizacion = Categoria.OrdenVisualizacion;

            _unitOfWork.Categoria.Update(categoriaBD);
            _unitOfWork.Save();
            TempData["Success"] = "Categoría editada con éxito.";
            return RedirectToPage("Index");
        }
    }
}
