using Microsoft.EntityFrameworkCore;
using PruebaProductos.Server.Data;
using PruebaProductos.Server.Models;

namespace PruebaProductos.Server.Repositories
{
    public class RepositoryProductos
    {
        private ProductosContext context;

        public RepositoryProductos(ProductosContext context)
        {
            this.context = context;
        }
        public async Task<List<Producto>> GetProductosAsync(int? codigo, string? descripcion, DateTime? fecha, string? categoria, decimal? precio)
        {
            var query = context.Productos.AsQueryable();

            // Filtrar por código si se proporciona
            if (codigo.HasValue)
            {
                query = query.Where(p => p.CodigoProducto == codigo.Value);
            }

            // Filtrar por descripción si se proporciona
            if (!string.IsNullOrEmpty(descripcion))
            {
                query = query.Where(p => p.DescripcionProducto.Contains(descripcion));
            }

            // Filtrar por fecha si se proporciona
            if (fecha.HasValue)
            {
                query = query.Where(p => p.FechaCreacion == fecha.Value);
                // Asumiendo que 'Fecha' es un campo en tu entidad Producto y su tipo es DateOnly
            }

            // Filtrar por categoría si se proporciona
            if (!string.IsNullOrEmpty(categoria))
            {
                query = query.Where(p => p.Categoria == categoria);
                // Asumiendo que 'Categoria' es un campo en tu entidad Producto de tipo string
            }

            // Filtrar por precio si se proporciona
            if (precio.HasValue)
            {
                query = query.Where(p => p.Precio == precio.Value);
                // Asumiendo que 'Precio' es un campo en tu entidad Producto de tipo double
            }

            return await query.ToListAsync();
        }

        //CREATE
        public async Task<Boolean> CreateProductoAsync(Producto producto)
        {
            var codigo = context.Productos.Max(p => p.CodigoProducto);
            producto.CodigoProducto = codigo +1;

            context.Productos.Add(producto);
            try
            {
                await context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateException ex)
            {
                Console.WriteLine(ex);
                return false;
            }

        }
        //READ
        public async Task<Producto> FindProductoAsync(int codigo)
        {
            var producto = await context.Productos.FindAsync(codigo);

            if (producto == null)
            {
                Console.WriteLine("No se encuentra ningun producto con el codigo proporcionado");
                return null;
            }

            return producto;
        }
        //UPDATE
        public async Task<Boolean> UpdateProductoAsync(int codigo, Producto producto)
        {
            if (codigo != producto.CodigoProducto)
            {
                Console.WriteLine("El codigo proporcionado no coincide con el producto a modificar");
                return false;
            }

            context.Entry(producto).State = EntityState.Modified;

            try
            {
                await context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {

                Console.WriteLine(ex);
            }

            return true; 
        }
        //DELETE
        public async Task<Boolean> DeleteProductoAsync(int codigo)
        {
            var producto = await context.Productos.FindAsync(codigo);
            if (producto == null)
            {
                Console.WriteLine("No se encuentra el producto a eliminar");
                return false;
            }

            context.Productos.Remove(producto);
            await context.SaveChangesAsync();

            return true;
        }
        
    }
}
