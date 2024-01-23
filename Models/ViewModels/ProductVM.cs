using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace SalesInvoice.Models.ViewModels
{
    public class ProductVM
    {
        public int id { get; set; }
        [DisplayName("ProductName")]
        public string productName { get; set; }
        [Display(Name = "DateAdded")]
        public DateTime dateAdded { get; set; }
        public double Quantity { get; set; } //double cause if it with Kg
        public decimal price { get; set; }
        public int categoryId { get; set; }
    }
}
