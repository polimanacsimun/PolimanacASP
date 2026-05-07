using Microsoft.AspNetCore.Mvc;
using InventoryManagement.DAL.Repositories;

namespace InventoryManagement.Controllers
{
    public class ProductController : Controller
    {
        private readonly ProductEfRepository _repository;

public ProductController(ProductEfRepository repository)
{
    _repository = repository;
}

        [Route("catalog", Name = "ProductCatalog")]
        [Route("Product")]
        [Route("Product/Index")]        
        public IActionResult Index()
        {
            var products = _repository.GetAll();
            return View(products);
        }

        
[Route("Product/Details/{id:int}")]
[Route("catalog/{id:int}", Name = "ProductCatalogDetails")]
        public IActionResult Details(int id)
        {
            var product = _repository.GetById(id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }
    }
}
