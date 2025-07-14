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
            lProductos = _unitOfWork.Producto.GetAll();
        }
    }
}
