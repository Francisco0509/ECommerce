using ECommerceRazor.DataAccess.Repository.IRepository;
using ECommerceRazor.Models;
using ECommerceRazor.Utility;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Stripe.Checkout;

namespace ECommerceRazor.Pages.Cliente.Carrito
{
    public class ConfirmacionOrdenModel : PageModel
    {
        private readonly IUnitOfWork _unitOfWork;
        public int OrdenId { get; set; }
        public ConfirmacionOrdenModel(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public void OnGet(int id)
        {
            Orden orden = _unitOfWork.Orden.GetFirstOrDefault(o => o.Id == id);

            if (orden.SessionId != null)
            {
                var servicio = new SessionService();
                Session session = servicio.Get(orden.SessionId);
                if (session.PaymentStatus.ToLower() == "paid") 
                { 
                    orden.PaymentIntentId = session.PaymentIntentId;
                    orden.Estado = CNT.EstadoPAgoEnviado;
                    _unitOfWork.Save();
                }
            }

            List<CarritoCompra> ListaCarritoCompra = _unitOfWork.CarritoCompra.GetAll(
                filter: c => c.ApplicationUserId == orden.UsuarioId,
                includeProperties: null).ToList();
            //Eliminar el carrito de compras del usuario
            _unitOfWork.CarritoCompra.RemoveRange(ListaCarritoCompra);
            _unitOfWork.Save();
            OrdenId = id;
        }
    }
}
