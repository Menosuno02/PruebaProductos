using Microsoft.AspNetCore.Mvc;
using PruebaProductos.Server.Models;
using PruebaProductos.Server.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PruebaProductos.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductosController : ControllerBase
    {
        private readonly RepositoryProductos repo;

        public ProductosController(RepositoryProductos repo)
        {
            this.repo = repo;
        }

        // GET: api/Productos
        [HttpGet]
        public async Task<IActionResult> GetProductosPrueba(
            [FromQuery] int? codigo,
            [FromQuery] string? descripcion,
            [FromQuery] DateTime? fecha,
            [FromQuery] string? categoria,
            [FromQuery] decimal? precio)
        {
            try
            {
                var productos = await repo.GetProductosAsync(codigo, descripcion, fecha, categoria, precio);
                return Ok(productos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }

        // GET: api/Productos/5
        [HttpGet("{codigo}")]
        public async Task<IActionResult> GetProducto(int codigo)
        {
            try
            {
                var producto = await repo.FindProductoAsync(codigo);
                if (producto == null)
                {
                    return NotFound($"No se encontró ningún producto con el código {codigo}");
                }
                return Ok(producto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }

        // POST: api/Productos
        [HttpPost]
        public async Task<IActionResult> PostProducto([FromBody] Producto producto)
        {
            try
            {
                var exito = await repo.CreateProductoAsync(producto);
                if (!exito)
                {
                    return StatusCode(500, "No se pudo crear el producto");
                }
                return CreatedAtAction(nameof(GetProducto), new { codigo = producto.CodigoProducto }, producto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }

        // PUT: api/Productos/5
        [HttpPut("{codigo}")]
        public async Task<IActionResult> PutProducto(int codigo, [FromBody] Producto producto)
        {
            try
            {
                var exito = await repo.UpdateProductoAsync(codigo, producto);
                if (!exito)
                {
                    return NotFound($"No se encontró ningún producto con el código {codigo} o el producto proporcionado no coincide con el código");
                }
                return Ok($"Producto con código {codigo} actualizado correctamente");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }

        // DELETE: api/Productos/5
        [HttpDelete("{codigo}")]
        public async Task<IActionResult> DeleteProducto(int codigo)
        {
            try
            {
                var exito = await repo.DeleteProductoAsync(codigo);
                if (!exito)
                {
                    return NotFound($"No se encontró ningún producto con el código {codigo}");
                }
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }
    }
}
