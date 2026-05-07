using Microsoft.AspNetCore.Mvc;
using InventoryManagement.DAL.Repositories;

namespace InventoryManagement.Controllers
{
    public class CategoryController : Controller
    {
        private readonly CategoryEfRepository _repository;

        public CategoryController(CategoryEfRepository repository)
        {
            _repository = repository;
        }

        [Route("/categories")]
        public IActionResult Index()
        {
            var categories = _repository.GetAll();
            return View(categories);
        }

        [Route("/categories/{id:int}")]
        public IActionResult Details(int id)
        {
            var category = _repository.GetById(id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }
    }
}