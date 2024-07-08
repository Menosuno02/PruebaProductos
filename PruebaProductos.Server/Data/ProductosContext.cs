using Microsoft.EntityFrameworkCore;
using PruebaProductos.Server.Models;

namespace PruebaProductos.Server.Data
{
    public class ProductosContext : DbContext
    {
        public ProductosContext(DbContextOptions<ProductosContext> options) : base(options) { }

        public DbSet<Producto> Productos { get; set; }
    }
}
