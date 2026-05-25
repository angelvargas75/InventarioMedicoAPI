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
            var resultado = await _pedidoService.CrearPedidoAsync(productoId, cantidad);
            return Ok(resultado);
        }

        // Concurrencia con Task.WhenAll
        // GET /api/pedidos/dashboard
        [HttpGet("dashboard")]
        public async Task<IActionResult> ObtenerDashboard()
        {
            var resultado = await _pedidoService.ObtenerDashboardAsync();
            return Ok(resultado);
        }
    }
}
