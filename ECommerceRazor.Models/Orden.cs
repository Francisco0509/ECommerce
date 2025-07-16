using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceRazor.Models
{
    public class Orden
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string UsuarioId { get; set; }
        [ForeignKey("UsuarioId")]
        public ApplicationUser Usuario { get; set; }
        [Required]
        [DisplayFormat(DataFormatString = "{0:C}")]
        public double TotalOrden { get; set; }
        public string? SessionId { get; set; }
        public string? PaymentIntentId { get; set; }
        public string? NombreUsuario { get; set; }
        public string? Telefono { get; set; }
        [DataType(DataType.EmailAddress)]
        public string? Email { get; set; }
        public string? Direccion { get; set; }
        [Display(Name = "Instrucciones adicionales")]
        public string InstruccionesAdicionales { get; set; }
        public string? Estado { get; set; } = "Pendiente";
        [Display(Name = "Fecha de orden")]
        public DateTime FechaOrden { get; set; }

    }
}
