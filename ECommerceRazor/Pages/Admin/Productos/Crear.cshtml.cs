using ECommerceRazor.DataAccess.Repository.IRepository;
using ECommerceRazor.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore.Metadata;

namespace ECommerceRazor.Pages.Admin.Productos
{
    [Authorize(Roles = "Administrador")]
    public class CrearModel : PageModel
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public CrearModel(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
        }

        [BindProperty]
        public Producto Producto { get; set; }
        [BindProperty]
        public IFormFile? ImagenSubida { get; set; }

        //Lista de categprias para el dropdown
        public IEnumerable<SelectListItem> Categorias { get; set; }


        public IActionResult OnGet()
        {
            //Carga las categorias de la bd
            Categorias = _unitOfWork.Categoria.GetAll()
                .Select(
                    c => new SelectListItem
                    {
                        Text = c.Nombre,
                        Value = c.Id.ToString()
                    }
                );

            //Validación por si la tabla categorías no tiene datos
            if (!Categorias.Any())
            {
                ModelState.AddModelError(string.Empty, "No hay cateorías disponibles. Por favor agregue una categoría.");
            }

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

            //Procesar subida de imagen
            if (Producto.ImagenSubida != null)
            {
                string upladoFolder = Path.Combine(_webHostEnvironment.WebRootPath, "img/products");
                //Se le da un nombre único a la imagen para que no haya conflicto con otras imagenes
                string uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(Producto.ImagenSubida.FileName);

                //Verificamos si existe la carpeta 
                if (!Directory.Exists(upladoFolder))
                {
                    //Si la carpeta no existe la creamos
                    Directory.CreateDirectory(upladoFolder);
                }

                string filePath = Path.Combine(upladoFolder, uniqueFileName);

                //Restricciones de tamaño y formato
                if (Producto.ImagenSubida.Length > 2 * 1024 * 1024) //2mb
                {
                    ModelState.AddModelError("ImagenSubida", "El tamaño máximo de la imagen no puede ser mayo a 2mb.");
                    return Page();
                }

                //Extensiones permitidas
                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
                if (!allowedExtensions.Contains(Path.GetExtension(Producto.ImagenSubida.FileName).ToLower()))
                {
                    ModelState.AddModelError("ImagenSubida", "El archivo debe ser una imagen (.jpg,.jpeg,.png,.gif).");
                    return Page();
                }

                using(var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    Producto.ImagenSubida.CopyTo(fileStream);
                }

                Producto.Imagen = "/products/" +  uniqueFileName;
            }

            _unitOfWork.Producto.Add(Producto);
            _unitOfWork.Save();
            TempData["success"] = "Producto creado correctamente";
            return RedirectToPage("Index");

        }
    }
}
