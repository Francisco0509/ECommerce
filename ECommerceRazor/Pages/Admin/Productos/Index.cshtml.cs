using ECommerceRazor.DataAccess.Repository.IRepository;
using ECommerceRazor.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ECommerceRazor.Pages.Admin.Productos
{
    public class IndexModel : PageModel
    {
        private readonly IUnitOfWork _unitOfWork;
        public IndexModel(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IEnumerable<Producto> lProductos { get; set; }
        public void OnGet()
        {
            lProductos = _unitOfWork.Producto.GetAll("Categoria"); //Incluir la categoría en la consulta
        }

        public async Task<IActionResult> OnPostDeleteAsync([FromBody] int id)
        {
            var producto = _unitOfWork.Producto.GetFirstOrDefaul(c => c.Id == id);
            if (producto == null)
            {
                TempData["Error"] = "El producto no existe.";
                return RedirectToPage("Index");
                //return NotFound();
            }


            _unitOfWork.Producto.Remove(producto);
            _unitOfWork.Save();
            TempData["Success"] = "El producto se eliminó con éxito.";
            return new JsonResult(new { success = true });
        }
    }
}
