using InventarioMedicoAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace InventarioMedicoAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PedidosController : ControllerBase
    {
        private readonly PedidoService _pedidoService;
        public PedidosController(PedidoService pedidoService)
        {
            _pedidoService = pedidoService;
        }

        // Transacciones
        // POST /api/pedidos
        [HttpPost]
        public async Task<IActionResult> CrearPedido(int productoId, int cantidad)
        {
            try
            {
                if (productoId <= 0)
                    return BadRequest(new { Mensaje = "El ID del producto debe ser mayor a 0" });

                if (cantidad <= 0)
                    return BadRequest(new { Mensaje = "La cantidad debe ser mayor a 0" });

                var resultado = await _pedidoService.CrearPedidoAsync(productoId, cantidad);
                return Ok(resultado);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Mensaje = "Error interno del servidor", Detalle = ex.Message });
            }
        }

        // Concurrencia con Task.WhenAll
        // GET /api/pedidos/dashboard
        [HttpGet("dashboard")]
        public async Task<IActionResult> ObtenerDashboard()
        {
            try
            {
                var resultado = await _pedidoService.ObtenerDashboardAsync();
                return Ok(resultado);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Mensaje = "Error interno del servidor", Detalle = ex.Message });
            }
        }
    }
}
