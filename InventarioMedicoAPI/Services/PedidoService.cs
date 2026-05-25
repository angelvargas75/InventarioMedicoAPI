using InventarioMedicoAPI.Data;
using InventarioMedicoAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace InventarioMedicoAPI.Services
{
    public class PedidoService
    {
        private readonly AppDbContext _context;
        public PedidoService(AppDbContext context)
        {
            _context = context;
        }

        //Transacciones
        // Crea el pedido Y descuenta stock en una sola transacción
        // Si algo falla → Rollback (deshace todo)
        // Si todo bien → Commit (confirma todo)
        public async Task<ApiResponse> CrearPedidoAsync(int productoId, int cantidad)
        {
            // Solo si se usa en una BD como SQL server
            //using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var producto = await _context.Productos.FindAsync(productoId);

                if (producto == null)
                    return new ApiResponse { Success = false, Mensaje = "Producto no encontrado" };

                if (producto.Stock < cantidad)
                    return new ApiResponse { Success = false, Mensaje = $"Stock insuficiente. Stock actual: {producto.Stock}" };

                producto.Stock -= cantidad;

                // se crea el pedido
                var pedido = new Pedido
                {
                    ProductoId = productoId,
                    Cantidad = cantidad,
                    FechaPedido = DateOnly.FromDateTime(DateTime.Today),
                    Estado = "Completado",
                    Producto = producto
                };

                _context.Pedidos.Add(pedido);
                await _context.SaveChangesAsync();

                // Todo salió bien → Commit
                //await transaction.CommitAsync();

                return new ApiResponse { Success = true, Mensaje = "Pedido creado correctamente", Data = pedido };
            }
            catch (Exception ex)
            {
                // sialgo fallo → Rollback, deshace todo
               // await transaction.RollbackAsync();
                return new ApiResponse { Success = false, Mensaje = $"Error al crear pedido: {ex.Message}" };
            }
        }

        // FASE 6: Concurrencia
        // Ejecuta 3 consultas en paralelo con Task.WhenAll
        // En lugar de esperar una por una, las 3 corren al mismo tiempo
        public async Task<DashboardResponse> ObtenerDashboardAsync()
        {
            var totalProductosTask = _context.Productos.CountAsync();
            var totalActivosTask = _context.Productos.CountAsync(p => p.Activo);
            var porCategoriaTask = _context.Productos
                .GroupBy(p => p.Categoria)
                .Select(g => new { Categoria = g.Key, Total = g.Count() })
                .ToListAsync();

            //las 3 consultas corren en paralelo, no una por una
            await Task.WhenAll(totalProductosTask, totalActivosTask, porCategoriaTask);

            return new DashboardResponse
            {
                TotalProductos = totalProductosTask.Result,
                TotalActivos = totalActivosTask.Result,
                PorCategoria = porCategoriaTask.Result.Select(x => new CategoriaResumen
                {
                    Categoria = x.Categoria,
                    Total = x.Total
                }).ToList(),
            };
        }
    }
}
