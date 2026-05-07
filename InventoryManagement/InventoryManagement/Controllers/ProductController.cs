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

        public IActionResult Index()
        {
            var products = _repository.GetAll();
            return View(products);
        }

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
