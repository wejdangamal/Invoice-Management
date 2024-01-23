namespace SalesInvoice.Models.ViewModels
{
    public class OrderMasterDetailsVM
    {
        public string orderCode { get; set; }
        public string CustomerName { get; set; }
        public string dateAdded { get; set; }
        public decimal Total_Price { get; set; }
        public List<listOfProducts> productsList { get; set; }
    }
    public class listOfProducts
    {
        public string product_Id { get; set; }
        public string product_Name { get; set; }
        public string quantity { get; set; }
        public string price { get; set; }
    }
}
