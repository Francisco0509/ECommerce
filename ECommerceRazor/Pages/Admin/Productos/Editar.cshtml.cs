using ECommerceRazor.DataAccess.Repository.IRepository;
using ECommerceRazor.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ECommerceRazor.Pages.Admin.Productos
{
    public class EditarModel : PageModel
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public EditarModel(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
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

        public IActionResult OnGet(int id)
        {
            //Cargar producto existende desde la base de datos
            Producto = _unitOfWork.Producto.GetFirstOrDefault(p => p.Id == id);
            if(Producto == null)
            {
                ModelState.AddModelError(string.Empty, "Producto no encontrado.");
                return NotFound();
            }

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
            
            //Validamos que el modelo sea correcto(no falte algún dato)
            if (!ModelState.IsValid)
            {
                return Page();
            }
            
            var productoBD = _unitOfWork.Producto.GetFirstOrDefault(p => p.Id == Producto.Id);
            if (productoBD == null)
            { 
                return NotFound();
            }

            //Procesar subida de imagen, si se sube
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

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    Producto.ImagenSubida.CopyTo(fileStream);
                }

                //Eliminar la imagen anterior si existe
                if (!string.IsNullOrEmpty(Producto.Imagen))
                {
                    string oldFilePath = Path.Combine(upladoFolder, Producto.Imagen);
                    if (System.IO.File.Exists(oldFilePath))
                    {
                        System.IO.File.Delete(oldFilePath);
                    }
                }

                Producto.Imagen = "/products/" + uniqueFileName;
            }
            else
            {
                //Recuperar la imagen de la base de datos
                var productoDesdeBd = _unitOfWork.Producto.GetFirstOrDefault(p => p.Id == Producto.Id);
                if (productoDesdeBd != null)
                {
                    Producto.Imagen = productoDesdeBd.Imagen;
                }
            }

                
            //Actualizar el producto en la base de datos
            _unitOfWork.Producto.Update(Producto);
            _unitOfWork.Save();
            TempData["success"] = "Producto actualizado correctamente";
            return RedirectToPage("Index");

        }
    }
}
