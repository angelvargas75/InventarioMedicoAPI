namespace InventarioMedicoAPI.Models
{
    public class ApiResponse
    {
        public bool Success { get; set; }
        public string Mensaje { get; set; }
        public object? Data { get; set; }
    }
}
