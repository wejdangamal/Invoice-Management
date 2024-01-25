using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SalesInvoice.IRepositoryPattern;
using SalesInvoice.Models;
using SalesInvoice.Models.ViewModels;
using System.Diagnostics;

namespace SalesInvoice.Controllers
{

    public class CategoryController : Controller
    {
        private readonly ILogger<CategoryController> _logger;
        private readonly IUnitOfWork repository;
        private readonly IMapper mapper;

        public CategoryController(ILogger<CategoryController> logger,IUnitOfWork repository,IMapper mapper)
        {
            _logger = logger;
            this.repository = repository;
            this.mapper = mapper;
        }
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet("Category/All/{page:int}/{pageSize:int}")]
        public IActionResult CategoryList(int page=1, int pageSize=10)
        {
            var categoryList = repository.category.GetAll(page, pageSize).ToList();
            return View(categoryList);
        }
        [HttpGet]
        public IActionResult Privacy()
        {
            return View();
        }
        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Add(CategoryVM model)
        {
            if (ModelState.IsValid)
            {
               var category= mapper.Map<Category>(model);
                try
                {
                    var done = await repository.category.Add(category);
                    if (done)
                    {
                        return RedirectToAction("CategoryList", new { page = 1, pageSize = 10 });
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
        public IActionResult Edit(int id)
        {

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Edit(CategoryVM model)
        {
            var found = await repository.category.GetById(model.id);
                   
            if (found != null)
            {
                mapper.Map(model,found);
                
                var done = await repository.category.Update(found);
                if (done)
                {
                    return RedirectToAction("CategoryList", new { page = 1, pageSize = 10 });
                }
            }
            return View();
        }
        
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var res = await repository.category.Delete(id);

            return RedirectToAction("CategoryList", new { page = 1, pageSize = 10 });

        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}