using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;

namespace ECommerceRazor.Models
{
    public class Categoria
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [StringLength(100, ErrorMessage = "El nombre no puede ser mayor a 100 caracteres.")]
        [Display(Name = "Nombre de la categoría.")]
        public string? Nombre { get; set; }
        [Required(ErrorMessage = "El orden de visualización es obligatorio.")]
        [Range(1, int.MaxValue, ErrorMessage = "El orden de visualización debe ser mayor a 0.")]
        [Display(Name = "Orden de visualización.")]
        public int OrdenVisualizacion { get; set; }
        public DateTime FechaCreacion { get; set; } = DateTime.Now;

        //relacipon de 1 a muchos: Una categoría puede tener muchos productos
        public ICollection<Producto>? Productos { get; set; }
    }
}
