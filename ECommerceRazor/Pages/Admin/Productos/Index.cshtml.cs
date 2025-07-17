using ECommerceRazor.DataAccess.Repository.IRepository;
using ECommerceRazor.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ECommerceRazor.Pages.Admin.Productos
{
    [Authorize(Roles = "Administrador")]
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
            lProductos = _unitOfWork.Producto.GetAll(filter: null, "Categoria"); //Incluir la categor�a en la consulta
        }

        public async Task<IActionResult> OnPostDeleteAsync([FromBody] int id)
        {
            var producto = _unitOfWork.Producto.GetFirstOrDefault(c => c.Id == id);
            if (producto == null)
            {
                TempData["Error"] = "El producto no existe.";
                return RedirectToPage("Index");
                //return NotFound();
            }


            _unitOfWork.Producto.Remove(producto);
            _unitOfWork.Save();
            TempData["Success"] = "El producto se elimin� con �xito.";
            return new JsonResult(new { success = true });
        }
    }
}
