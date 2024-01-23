using Microsoft.AspNetCore.Mvc;
using SalesInvoice.IRepositoryPattern;
using SalesInvoice.Models.ViewModels;
using SalesInvoice.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace SalesInvoice.Controllers
{
    public class ProductController : Controller
    {
        private readonly ILogger<ProductController> _logger;
        private readonly IRepository<Product> repository;
        private readonly IRepository<Category> categoryRepo;

        public ProductController(ILogger<ProductController> logger, IRepository<Product> repository, IRepository<Category> categoryRepo)
        {
            _logger = logger;
            this.repository = repository;
            this.categoryRepo = categoryRepo;
        }
        [HttpGet("Product/All")]
        public IActionResult ProductList()
        {
            var ProductList = repository.GetAll(new string[] { "category" }).ToList();
            return View(ProductList);
        }
        [HttpGet]
        public IActionResult Add()
        {
            var categories = categoryRepo.GetAll().ToList();
            var selectlist = new List<SelectListItem>
            {
                new SelectListItem { Text = "Choose Category", Value = "" }
            };
            foreach(var item in categories)
            {
                selectlist.Add(new SelectListItem { Text=item.name,Value=item.id.ToString()});
            }
            ViewData["Options"] = selectlist;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Add(ProductVM model)
        {
            if (ModelState.IsValid)
            {
                Product newProduct = new()
                {
                    productName = model.productName,
                    categoryId = model.categoryId,
                    dateAdded= DateTime.Now,
                    price= model.price,
                    Quantity = model.Quantity
                };
                try
                {
                    var done = await repository.Add(newProduct);
                    if (done)
                    {
                        return RedirectToAction("ProductList");
                    }

                }
                catch (Exception ex)
                {
                    return Content("Exception");
                }
            }
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var modelVM = await repository.GetById(id);
            return View(modelVM);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(Product model)
        {
            var found = await repository.GetById(model.id);
            if (found != null)
            {
                found.price = model.price;
                found.productName = model.productName;
                found.Quantity = model.Quantity;
                found.dateAdded = model.dateAdded;
                found.categoryId = model.categoryId;

                var done = await repository.Update(found);
                if (done)
                {
                    return RedirectToAction("ProductList");
                }
            }
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var res = await repository.Delete(id);

            return RedirectToAction("ProductList");
        }
        [HttpGet]
        public JsonResult GetByCategoryID(int categoryId)
        {
            var products = repository.GetAll(null).Where(x => x.categoryId == categoryId).ToList();

            List<SelectListItem> allProducts = new()
            {
                new SelectListItem { Text="Choose Products..." , Value=""}
            };
            foreach (var item in products)
            {
                allProducts.Add(new SelectListItem { Text = item.productName, Value = item.id.ToString() });
            }
            return Json(allProducts);
        }
        [HttpGet]
        public async Task<JsonResult> GetPrice(int productId)
        {
            var product = await repository.GetById(productId);
            decimal? price = product?.price;
            double? quantity = product?.Quantity;
            var data = new
            {
                price,
                quantity
            };
            return Json(data);
        }
    }
}
