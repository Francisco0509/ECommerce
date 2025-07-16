using ECommerceRazor.DataAccess.Repository.IRepository;
using ECommerceRazor.Models;
using ECommerceRazor.Utility;
using Humanizer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Stripe;
using Stripe.Checkout;
using System.Security.Claims;

namespace ECommerceRazor.Pages.Cliente
{
    [Authorize]
    public class ResumenModel : PageModel
    {
        public IEnumerable<CarritoCompra> ListaCarritoCompra { get; set; }
        [BindProperty]
        public Orden Orden { get; set; }


        private readonly IUnitOfWork _unitOfWork;
        public ResumenModel(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            Orden = new Orden();
        }



        public void OnGet()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            if (claim != null)
            {
                ListaCarritoCompra = _unitOfWork.CarritoCompra.GetAll(
                    filter: c => c.ApplicationUserId == claim.Value,
                    includeProperties: "Producto,Producto.Categoria");
                foreach (var itemCarrito in ListaCarritoCompra)
                {
                    Orden.TotalOrden += (double)(itemCarrito.Cantidad * itemCarrito.Producto.Precio);
                }

                //Obtener los datos del usuario del repositorio ApplicationUser
                ApplicationUser applicationUser = _unitOfWork.ApplicationUser.GetFirstOrDefault(
                    filter: u => u.Id == claim.Value);

                Orden.NombreUsuario = applicationUser.Nombres + " " + applicationUser.Apellidos;
                Orden.Direccion = applicationUser.Direccion;
                Orden.Telefono = applicationUser.PhoneNumber;
                Orden.InstruccionesAdicionales = Orden.InstruccionesAdicionales ?? string.Empty;
                Orden.Email = applicationUser.Email;
            }
        }

        public IActionResult OnPost()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            if (claim != null)
            {
                ListaCarritoCompra = _unitOfWork.CarritoCompra.GetAll(
                    filter: c => c.ApplicationUserId == claim.Value,
                    includeProperties: "Producto,Producto.Categoria");
                foreach (var itemCarrito in ListaCarritoCompra)
                {
                    Orden.TotalOrden += (double)(itemCarrito.Cantidad * itemCarrito.Producto.Precio);
                }

                //Obtener los datos del usuario del repositorio ApplicationUser
                ApplicationUser applicationUser = _unitOfWork.ApplicationUser.GetFirstOrDefault(
                    filter: u => u.Id == claim.Value);


                Orden.Estado = CNT.EstadoPendiente;
                Orden.FechaOrden = DateTime.Now;
                Orden.UsuarioId = claim.Value;
                Orden.NombreUsuario = applicationUser.Nombres + " " + applicationUser.Apellidos;
                Orden.Direccion = applicationUser.Direccion;
                Orden.Telefono = Orden.Telefono;
                Orden.InstruccionesAdicionales = Orden.InstruccionesAdicionales ?? string.Empty;
                Orden.Email = Orden.Email;

                _unitOfWork.Orden.Add(Orden);
                _unitOfWork.Save();

                //Agregar los detalles de la orden
                foreach(var item in ListaCarritoCompra)
                {
                    DetalleOrden detalleOrden = new DetalleOrden()
                    {
                        ProductoId = item.ProductoId,
                        OrdenId = Orden.Id,
                        Precio = (double)item.Producto.Precio,
                        NombreProducto = item.Producto.Nombre,                        
                        Cantidad = item.Cantidad
                    };

                    _unitOfWork.DetalleOrden.Add(detalleOrden);
                    _unitOfWork.Save();
                }

                //Eliminar el carrito de compras del usuario
                //_unitOfWork.CarritoCompra.RemoveRange(ListaCarritoCompra);
                _unitOfWork.Save();

                //Código para reenviar al pago en Stripe
                var domain = $"{Request.Scheme}://{Request.Host.Value}";

                var options = new SessionCreateOptions
                {
                    PaymentMethodTypes = new List<string> { "card" },
                    LineItems = new List<SessionLineItemOptions>(),
                    Mode = "payment",
                    SuccessUrl = $"{domain}/Cliente/Carrito/ConfirmacionOrden?id={Orden.Id}",
                    CancelUrl = $"{domain}/Cliente/Carrito/Index",
                };

                //Agregar items para que se muestren en Stripe
                foreach (var item in ListaCarritoCompra)
                {
                    var sessionLineItem = new SessionLineItemOptions
                    {
                        PriceData = new SessionLineItemPriceDataOptions
                        {
                            UnitAmount = (long)(item.Producto.Precio * 100), //Total en centavos
                            Currency = "mxn",
                            ProductData = new SessionLineItemPriceDataProductDataOptions
                            {
                                Name = item.Producto.Nombre,
                                //Description = $"Total de líneas: {cantidad}"
                            },
                        },
                        Quantity = item.Cantidad,
                    };

                    options.LineItems.Add(sessionLineItem);
                }

                var service = new SessionService();
                Session session = service.Create(options);
                Response.Headers.Add("Location", session.Url);

                //Obtener id de Stripe
                Orden.SessionId = session.Id;
                //Orden.PaymentIntentId = session.PaymentIntentId;

                _unitOfWork.Save();

                return new StatusCodeResult(303);
            }

            return Page();
        }

    }
}
