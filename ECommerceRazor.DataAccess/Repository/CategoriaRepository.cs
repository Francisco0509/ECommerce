using ECommerceRazor.DataAccess.Repository.IRepository;
using ECommerceRazor.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceRazor.DataAccess.Repository
{
    public class CategoriaRepository : Repository<Categoria>, ICategoriaRepository
    {
        private readonly AppDbContext _context;
        public CategoriaRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }       

        public void Update(Categoria categoria)
        {
            var objDesdeBd = _context.Categorias.FirstOrDefault(c => c.Id == categoria.Id);
            objDesdeBd.Nombre = categoria.Nombre;
            objDesdeBd.OrdenVisualizacion = categoria.OrdenVisualizacion;             
        }
    }
}
