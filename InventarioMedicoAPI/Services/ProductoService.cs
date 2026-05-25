using InventarioMedicoAPI.Data;
using InventarioMedicoAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace InventarioMedicoAPI.Services
{
    public class ProductoService
    {
        private readonly AppDbContext _context;
        private readonly IMemoryCache _cache;
        public ProductoService(AppDbContext context, IMemoryCache cache)
        {
            _context = context;
            _cache = cache;
        }

        // Paginación + async/await
        // Se usa IQueryable para que el filtro y la paginación se ejecuten en la BD
        // y no traer todos los registros a memoria
        public async Task<PaginadoResponse> ObtenerPaginadoAsync(int pagina, int porPagina)
        {
            var query = _context.Productos.AsQueryable();
            var total = await query.CountAsync();

            var productos = await query
                .OrderBy(p=> p.Id)
                .Skip((pagina -1)* porPagina)
                .Take(porPagina)
                .ToListAsync();

            return new PaginadoResponse
            {
                PaginaActual = pagina,
                RegistrosPorPagina = porPagina,
                TotalRegistros = total,
                TotalPaginas = (int)Math.Ceiling((double)total / porPagina),
                Data = productos
            };
        }

        //IEnumerable vs IQueryable
        // MAL: ToList() trae TODOS los registros a memoria y filtra en C#
        public List<Producto> BuscarPorCategoriaEnMemoria(string categoria)
        {
            return _context.Productos
                .ToList()
                .Where(p => p.Categoria == categoria)
                .ToList();
        }

        // BIEN: Where() se traduce a sql, filtra en la bd antes de traer datos
        public async Task<List<Producto>> BuscarPorCategoriaAsync(string categoria)
        {
            return await _context.Productos
                .Where(p => p.Categoria == categoria)
                .ToListAsync();
        }

        // Caché / Memorización
        // Primero busca en caché, si no está consulta la BD y guarda el resultado
        public async Task<Producto?> ObtenerPorIdAsync(int id)
        {
            string cacheKey = $"producto_{id}";

            if (!_cache.TryGetValue(cacheKey, out Producto? producto))
            {
                // No está en caché, consulta la BD
                producto = await _context.Productos.FindAsync(id);

                if (producto != null)
                {
                    // Guarda en caché por 10 minutos
                    _cache.Set(cacheKey, producto, TimeSpan.FromMinutes(10));
                }
            }

            return producto;
        }
    }
}
