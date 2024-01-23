using System.ComponentModel.DataAnnotations;

namespace SalesInvoice.Models.ViewModels
{
    public class CategoryVM
    {
        public int id { get; set; }
        [Required]
        public string name { get; set; }
    }
}
