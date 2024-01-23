namespace SalesInvoice.Models
{
    public class Order
    {
        public int id { get; set; }
        public string order_code { get; set; }
        public string CustomerName { get; set; }
        public DateTime dateAdded { get; set; }
        public ICollection<OrderItems> orderItems { get; set; }
    }
}
