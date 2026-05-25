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
            var resultado = await _productoService.ObtenerPaginadoAsync(pagina, porPagina);
            return Ok(resultado);
        }

        //IQueryable - filtra en BD (correcto)
        // GET /api/productos/buscar?categoria=Medicamentos
        [HttpGet("buscar")]
        public async Task<IActionResult> BuscarPorCategoria(string categoria)
        {
            var resultado = await _productoService.BuscarPorCategoriaAsync(categoria);
            return Ok(resultado);
        }
        // IEnumerable - filtra en memoria (incorrecto, solo para demostrar)
        // GET /api/productos/buscar-memoria?categoria=Medicamentos
        [HttpGet("buscar-memoria")]
        public IActionResult BuscarPorCategoriaEnMemoria(string categoria)
        {
            var resultado = _productoService.BuscarPorCategoriaEnMemoria(categoria);
            return Ok(resultado);
        }

        //Cache
        // GET /api/productos/5
        [HttpGet("{id}")]
        public async Task<IActionResult> ObtenerPorId(int id)
        {
            var producto = await _productoService.ObtenerPorIdAsync(id);

            if (producto == null)
                return NotFound(new { Mensaje = $"Producto con Id {id} no encontrado" });

            return Ok(producto);
        }
    }
}
