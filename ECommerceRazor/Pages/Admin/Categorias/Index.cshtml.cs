using ECommerceRazor.DataAccess;
using ECommerceRazor.DataAccess.Repository.IRepository;
using ECommerceRazor.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace ECommerceRazor.Pages.Admin.Categorias
{
    public class IndexModel : PageModel
    {
        private readonly IUnitOfWork _unitOfWork;
        public IndexModel(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IEnumerable<Categoria> lCategorias { get; set; } = default!;

        public void OnGet()
        {
            //Cargar las categorías desde la base de datos
            lCategorias = _unitOfWork.Categoria.GetAll().OrderBy(c => c.OrdenVisualizacion);

        }

        public async Task<IActionResult> OnPostDeleteAsync([FromBody] int id) 
        {
            var categoria = _unitOfWork.Categoria.GetFirstOrDefault(c => c.Id == id);
            if(categoria == null)
            {
                TempData["Error"] = "La categoría no existe.";
                return RedirectToPage("Index");
                //return NotFound();
            }
                

            _unitOfWork.Categoria.Remove(categoria);
            _unitOfWork.Save();
            TempData["Success"] = "La categoría se eliminó con éxito.";
            return new JsonResult(new { success = true });
        }
    }
}
