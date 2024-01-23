using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SalesInvoice.IRepositoryPattern;
using SalesInvoice.Models;
using SalesInvoice.Models.ViewModels;

namespace SalesInvoice.Controllers
{
    public class OrderController : Controller
    {
        private readonly IRepository<Order> orderRepository;
        private readonly IRepository<OrderItems> orderItemsRepository;
        private readonly IRepository<Category> categoryRepository;
        private readonly IRepository<Product> productRepository;

        public OrderController(IRepository<Order> orderRepository, IRepository<OrderItems> orderItemsRepository,
            IRepository<Category> categoryRepository, IRepository<Product> productRepository)
        {
            this.orderRepository = orderRepository;
            this.orderItemsRepository = orderItemsRepository;
            this.categoryRepository = categoryRepository;
            this.productRepository = productRepository;
        }
        [HttpGet]
        public IActionResult All()
        {
            List<Order> orders = orderRepository.GetAll().ToList();
            return View(orders);
        }
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var orderDetails = await orderRepository.GetById(id);
            IEnumerable<OrderItems> products = orderItemsRepository.GetAll(new string[] { "product" }).ToList();
            products = products.Where(x => x.OrderId == orderDetails.id).ToList();
            OrderMasterDetailsVM details = new()
            {
                CustomerName = orderDetails.CustomerName,
                dateAdded = orderDetails.dateAdded.ToString(),
                orderCode = orderDetails.order_code,
            };
            List<listOfProducts> list = new List<listOfProducts>();
            decimal price = 0;
            foreach (var item in products)
            {
                list.Add(
                   new listOfProducts
                   {
                       price = item.Price.ToString(),
                       product_Id = item.productId.ToString(),
                       quantity = item.QuantityOfProduct.ToString(),
                       product_Name = item.product.productName
                   });
                price += (decimal)(item.Price) * (item.QuantityOfProduct);
            }
            details.productsList = list;
            details.Total_Price = price;
            return View(details);
        }
        [HttpGet]
        public IActionResult Add()
        {
            DateTime dateTime = DateTime.Now;
            ViewBag.dateTime = dateTime;
            int orderCode = new Random().Next(1000, 2000);
            string orderCodeGenerate = dateTime.ToString("yyyyMMdd");
            ViewBag.orderCode = orderCode + orderCodeGenerate;

            var categories = categoryRepository.GetAll(null).Select(x => new { Id = x.id, Name = x.name }).ToList();
            List<SelectListItem> categoryitems = new List<SelectListItem>
            {
                new SelectListItem{Text="Choose Category",Value=""}
            };
            foreach (var item in categories)
            {
                categoryitems.Add(new SelectListItem { Text = item.Name, Value = item.Id.ToString() });
            }
            ViewBag.CategoryList = categoryitems;

            var products = productRepository.GetAll(null).Select(x => new { Id = x.id, Name = x.productName }).ToList();
            List<SelectListItem> productItems = new List<SelectListItem>
            {
                new SelectListItem{Text="Products",Value=""}
            };
            foreach (var item in products)
            {
                productItems.Add(new SelectListItem { Text = item.Name, Value = item.Id.ToString() });
            }
            ViewBag.ProductList = productItems;

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] OrderMasterDetailsVM model)
        {
            Order order = new Order()
            {
                CustomerName = model.CustomerName,
                dateAdded = Convert.ToDateTime(model.dateAdded),
                order_code = model.orderCode

            };
            var res = await orderRepository.Add(order);
            if (res)
            {
                foreach (var item in model.productsList)
                {
                    OrderItems orderItem = new OrderItems()
                    {
                        OrderId = order.id,
                        Price = float.Parse(item.price),
                        QuantityOfProduct = int.Parse(item.quantity),
                        productId = int.Parse(item.product_Id)
                    };
                    var result = await orderItemsRepository.Add(orderItem);
                }

                return CreatedAtAction(nameof(Create), new { id = order.id });

            }
            return BadRequest("Failed to create order");
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var model = await orderRepository.GetById(id);
            OrderMasterDetailsVM orderMaster = new()
            {
                CustomerName = model.CustomerName,
                orderCode = model.order_code,
                dateAdded = model.dateAdded.ToString()
            };
            IEnumerable<OrderItems> products = orderItemsRepository.GetAll(new string[] { "product", "order" }).ToList();
            products = products.Where(x => x.OrderId == model.id).ToList();
            List<listOfProducts> list = new List<listOfProducts>();
            decimal price = 0;
            foreach (var item in products)
            {
                list.Add(
                   new listOfProducts
                   {
                       price = item.Price.ToString(),
                       product_Id = item.productId.ToString(),
                       quantity = item.QuantityOfProduct.ToString(),
                       product_Name = item.product.productName
                   });
                price += (decimal)(item.Price) * (item.QuantityOfProduct);
            }
            orderMaster.productsList = list;
            orderMaster.Total_Price = price;
            return View(orderMaster);
        }
        [HttpPost]
        public IActionResult Edit(OrderMasterDetailsVM model)
        {
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var _result = await orderRepository.Delete(id);
            return RedirectToAction("All");
        }
        [HttpPost]
        public IActionResult Search(string search)
        {
            IEnumerable<Order> orders = orderRepository
                .GetAll()
                .Where(
                x => x.order_code == search ||
                x.CustomerName.ToLower()
                .Contains(search.ToLower()))
                .ToList();
            return View("All", orders);
        }
    }
}
/*
 * Customer Name
 * Quantity
 * Price
 * Total Price on UI
 * Order =>
 * customer name
 * orderItem =>
 * 
        public int QuantityOfProduct 
        public float Price 
 * Remove => Remove From OrderItem Table Only
 */