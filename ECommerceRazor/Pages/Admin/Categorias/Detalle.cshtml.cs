using ECommerceRazor.DataAccess;
using ECommerceRazor.DataAccess.Repository.IRepository;
using ECommerceRazor.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ECommerceRazor.Pages.Admin.Categorias
{
    public class DetalleModel : PageModel
    {
        private readonly IUnitOfWork _unitOfWork;
        public DetalleModel(IUnitOfWork untiOfWork)
        {
            _unitOfWork = untiOfWork;
        }

        [BindProperty]
        public Categoria Categoria { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Categoria = _unitOfWork.Categoria.GetFirstOrDefaul(c => c.Id == id);
            if (Categoria == null)
            {
                return NotFound();
            }

            return Page();
        }
    }
}
