using InventarioMedicoAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace InventarioMedicoAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductosController : ControllerBase
    {
        private readonly ProductoService _productoService;
        public ProductosController(ProductoService productoService)
        {
            _productoService = productoService;
        }

        //Paginación + async/await
        // GET /api/productos?pagina=1&porPagina=50
        [HttpGet]
        public async Task<IActionResult> ObtenerPaginado(int pagina = 1, int porPagina = 50)
        {
            try
            {
                if (pagina <= 0)
                    return BadRequest(new { Mensaje = "El número de página debe ser mayor a 0" });

                if (porPagina <= 0 || porPagina > 100)
                    return BadRequest(new { Mensaje = "El número de registros debe ser entre 1 y 100" });

                var resultado = await _productoService.ObtenerPaginadoAsync(pagina, porPagina);
                return Ok(resultado);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Mensaje = "Error interno del servidor", Detalle = ex.Message });
            }
        }

        //IQueryable - filtra en BD (correcto)
        // GET /api/productos/buscar?categoria=Medicamentos
        [HttpGet("buscar")]
        public async Task<IActionResult> BuscarPorCategoria(string? categoria)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(categoria))
                    return BadRequest(new { Mensaje = "La categoría no puede estar vacía" });

                categoria = categoria.Trim();
                if (string.IsNullOrWhiteSpace(categoria))
                    return BadRequest(new { Mensaje = "La categoría no puede estar vacía" });

                var categoriasValidas = new[] { "Medicamentos", "Insumos", "Equipos", "Laboratorio", "Emergencia" };
                if (!categoriasValidas.Contains(categoria))
                    return BadRequest(new { Mensaje = $"Categoría inválida. Las categorías válidas son: {string.Join(", ", categoriasValidas)}" });

                var resultado = await _productoService.BuscarPorCategoriaAsync(categoria);
                return Ok(resultado);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Mensaje = "Error interno del servidor", Detalle = ex.Message });
            }
        }
        // IEnumerable - filtra en memoria (incorrecto, solo para demostrar)
        // GET /api/productos/buscar-memoria?categoria=Medicamentos
        [HttpGet("buscar-memoria")]
        public IActionResult BuscarPorCategoriaEnMemoria(string? categoria)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(categoria))
                    return BadRequest(new { Mensaje = "La categoría no puede estar vacía" });

                categoria = categoria.Trim();

                var resultado = _productoService.BuscarPorCategoriaEnMemoria(categoria);
                return Ok(resultado);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Mensaje = "Error interno del servidor", Detalle = ex.Message });
            }
        }

        //Cache
        // GET /api/productos/5
        [HttpGet("{id}")]
        public async Task<IActionResult> ObtenerPorId(int id)
        {
            try
            {
                if (id <= 0)
                    return BadRequest(new { Mensaje = "El ID debe ser mayor a 0" });

                var producto = await _productoService.ObtenerPorIdAsync(id);

                if (producto == null)
                    return NotFound(new { Mensaje = $"Producto con Id {id} no encontrado" });

                return Ok(producto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Mensaje = "Error interno del servidor", Detalle = ex.Message });
            }
        }
    }
}
