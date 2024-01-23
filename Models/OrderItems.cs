using System.ComponentModel.DataAnnotations.Schema;

namespace SalesInvoice.Models
{
    public class OrderItems
    {
        public int Id { get; set; }
        public int productId { get; set; }
        public Product product { get; set; }
        public int QuantityOfProduct { get; set; }
        public float Price { get; set; }
        [ForeignKey("order")]
        public int OrderId { get; set; }
        public Order order { get; set; }

    }
}
