using Microsoft.AspNetCore.Mvc;
using InventoryManagement.Repositories.Mock;

namespace InventoryManagement.Controllers
{
    public class ProductController : Controller
    {
        private readonly ProductMockRepository _repository;

        public ProductController(ProductMockRepository repository)
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
