namespace InventarioMedicoAPI.Models
{
    public class CategoriaResumen
    {
        public string Categoria { get; set; }
        public int Total { get; set; }
    }

    public class DashboardResponse
    {
        public int TotalProductos { get; set; }
        public int TotalActivos { get; set; }
        public List<CategoriaResumen> PorCategoria { get; set; }
    }
}
