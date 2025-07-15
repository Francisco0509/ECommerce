using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceRazor.Utility
{
    public static class CNT
    {
        //Roles de usuario
        public const string Administrador = "Administrador";
        public const string Cliente = "Cliente";

        //Estados de la orden
        public const string EstadoPendiente = "Pago_Pendiente";
        public const string EstadoPAgoEnviado = "Estado_PagoEnviado";
        public const string EstadoPagoRechazado = "Estado_PagoRechazado";
        public const string EstadoEnviado = "Estado_Enviado";
        public const string EstadoCompletado = "Estado_Completado";
        public const string EsdoCancelado = "Estado_Cancelado";
        public const string EstadoReembolsado = "Estado_Reembolsado";
    }
}
