using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceRazor.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Required(ErrorMessage = "El nombre es oblligatorio.")]
        public string Nombres { get; set; }
        [Required(ErrorMessage = "Los apellidos son oblligatorios.")]
        public string Apellidos { get; set; }
        [Required(ErrorMessage = "La dirección es obligatoria.")]
        public string Direccion { get; set; }
        [Required(ErrorMessage = "La dirección es obligatoria.")]
        public string CodigoPostal { get; set; }
        [Required(ErrorMessage = "La ciudad es obligatoria.")]
        public string Ciudad { get; set; }
        [Required(ErrorMessage = "El país es obligatorio.")]
        public string Pais { get; set; }
    }
}
