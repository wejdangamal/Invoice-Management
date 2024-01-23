namespace SalesInvoice.Models
{
    public class Category
    {
        public int id { get; set; }
        public string name { get; set; }
        public ICollection<Product> products { get; set; }
    }
}
