using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SalesInvoice.Models
{
    public class Product
    {
        public int id { get; set; }
        [DisplayName("ProductName")]
        public string productName { get; set; }
        [Display(Name ="DateAdded")]
        public DateTime dateAdded { get; set; }
        public double Quantity { get; set; } //double cause if it with Kg
        public decimal price { get; set; }
        public int categoryId { get; set; }
        public Category category { get; set; }
    }
}
