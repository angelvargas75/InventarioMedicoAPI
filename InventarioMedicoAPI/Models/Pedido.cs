using System.ComponentModel.DataAnnotations;

namespace InventarioMedicoAPI.Models
{
    public class Pedido
    {
        public int Id { get; set; }
        public int ProductoId { get; set; }
        public int Cantidad { get; set; }
        public DateOnly FechaPedido { get; set; }
        public string Estado { get; set; }
        public Producto Producto { get; set; }
    }
}
