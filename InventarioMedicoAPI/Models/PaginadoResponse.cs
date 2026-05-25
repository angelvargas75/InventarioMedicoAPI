namespace InventarioMedicoAPI.Models
{
    public class PaginadoResponse
    {
        public int PaginaActual { get; set; }
        public int RegistrosPorPagina { get; set; }
        public int TotalRegistros { get; set; }
        public int TotalPaginas { get; set; }
        public List<Producto> Data { get; set; }
    }
}
