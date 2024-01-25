using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SalesInvoice.IRepositoryPattern;
using SalesInvoice.Models;
using SalesInvoice.Models.ViewModels;

namespace SalesInvoice.Controllers
{
    public class OrderController : Controller
    {
        private readonly IUnitOfWork unitRepository;
        private readonly IMapper mapper;

        public OrderController(IUnitOfWork unitRepository, IMapper mapper)
        {
            this.unitRepository = unitRepository;
            this.mapper = mapper;
        }
        [HttpGet]
        public IActionResult All()
        {
            List<Order> orders = unitRepository.order.GetAll().ToList();
            return View(orders);
        }
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var orderDetails = await unitRepository.order.GetById(id);
            IEnumerable<OrderItems> products = unitRepository.orderItems.GetAll(new string[] { "product" }).ToList();
            products = products.Where(x => x.OrderId == orderDetails.id).ToList();
            OrderMasterDetailsVM details = mapper.Map<OrderMasterDetailsVM>(orderDetails);
            //    new()
            //{
            //    CustomerName = orderDetails.CustomerName,
            //    dateAdded = orderDetails.dateAdded.ToString(),
            //    orderCode = orderDetails.order_code,
            //};
            List<listOfProducts> list = new List<listOfProducts>();
            decimal price = 0;
            foreach (var item in products)
            {
                var mapped = mapper.Map<listOfProducts>(item);
                mapper.Map(item.product, mapped);
                list.Add(mapped);
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

            var categories = unitRepository.category.GetAll(null).Select(x => new { Id = x.id, Name = x.name }).ToList();
            List<SelectListItem> categoryitems = new List<SelectListItem>
            {
                new SelectListItem{Text="Choose Category",Value=""}
            };
            foreach (var item in categories)
            {
                categoryitems.Add(new SelectListItem { Text = item.Name, Value = item.Id.ToString() });
            }
            ViewBag.CategoryList = categoryitems;

            var products = unitRepository.product.GetAll(null).Select(x => new { Id = x.id, Name = x.productName }).ToList();
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
            Order order = mapper.Map<Order>(model);

            var res = await unitRepository.order.Add(order);
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
                    var result = await unitRepository.orderItems.Add(orderItem);
                }

                return CreatedAtAction(nameof(Create), new { id = order.id });

            }
            return BadRequest("Failed to create order");
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var model = await unitRepository.order.GetById(id);
            OrderMasterDetailsVM orderMaster = mapper.Map<OrderMasterDetailsVM>(model);
            IEnumerable<OrderItems> products = unitRepository.orderItems.GetAll(new string[] { "product", "order" }).ToList();
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
            var _result = await unitRepository.order.Delete(id);
            return RedirectToAction("All");
        }
        [HttpPost]
        public IActionResult Search(string search)
        {
            IEnumerable<Order> orders = unitRepository
                .order
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