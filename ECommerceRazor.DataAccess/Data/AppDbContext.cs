using ECommerceRazor.Models;
using Microsoft.EntityFrameworkCore;

namespace ECommerceRazor.DataAccess
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        //TODO: agregar las entidades del modelo de dominio
        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<Producto> Productos { get; set; }
    }
}
