using ECommerceRazor.DataAccess.Repository.IRepository;
using ECommerceRazor.Models;
using ECommerceRazor.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ECommerceRazor.Pages.Admin.Ordenes
{
    [Authorize(Roles = "Administrador")]
    public class DetalleOrdenModel : PageModel
    {
        private readonly IUnitOfWork _unitOfWork;

        public DetalleOrdenModel(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [BindProperty(SupportsGet = true)]
        //Id de la orden
        public int Id { get; set; }
        public List<string> EstadosDisponibles { get; set; }
        public Orden Orden { get; set; }
        public IEnumerable<DetalleOrden> DetalleOrdenes { get; set; }

        public IActionResult OnGet(int id)
        {
            Orden = _unitOfWork.Orden.GetFirstOrDefault(u => u.Id == id);
            if (Orden == null)
            {
                return NotFound("No se encontró la orden.");
            }

            DetalleOrdenes = _unitOfWork.DetalleOrden.GetAll(u => u.OrdenId == Id, includeProperties: "Producto");

            //Definit los estados disponibles
            EstadosDisponibles = new List<string>
            {
                CNT.EstadoCompletado,
                CNT.EsdoCancelado,
                CNT.EstadoReembolsado
            };

            return Page();
        }

        public IActionResult OnPostActualizarEstado(int id, string nuevoEstado)
        {
            //Buscar orden
            var orden = _unitOfWork.Orden.GetFirstOrDefault(o => o.Id == id);
            if (orden == null)
            {
                return new JsonResult(new { success = false, message = "Orden no encontrada." });
            }

            //Definit los estados disponibles
            var EstadosPermitidos = new List<string>
            {
                CNT.EstadoCompletado,
                CNT.EsdoCancelado,
                CNT.EstadoReembolsado
            };

            if (!EstadosPermitidos.Contains(nuevoEstado))
            {
                return new JsonResult(new { success = false, message = "Estado inválido." });
            }

            //Actualizar el estado
            orden.Estado = nuevoEstado;
            _unitOfWork.Save();

            //Retornar respuesta exitosa
            return new JsonResult(new { success = true, message = "El estado de la Orden se actualizó correctamente." });
        }
    }
}
