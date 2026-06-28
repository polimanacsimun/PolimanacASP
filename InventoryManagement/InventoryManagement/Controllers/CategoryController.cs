using InventoryManagement.DAL.Repositories;
using InventoryManagement.Domain.Models;
using InventoryManagement.ViewModels.Category;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InventoryManagement.Controllers
{
    [Authorize]
    public class CategoryController : Controller
    {
        private readonly CategoryEfRepository _repository;

        public CategoryController(CategoryEfRepository repository)
        {
            _repository = repository;
        }

        [AllowAnonymous]
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

        [Authorize(Roles = "Admin")]
        [Route("/categories/create")]
        [Route("Category/Create")]
        public IActionResult Create()
        {
            var model = new CategoryFormModel();
            return View(model);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("/categories/create")]
        [Route("Category/Create")]
        public IActionResult Create(CategoryFormModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var category = new Category
            {
                Name = model.Name,
                Description = model.Description
            };

            _repository.Add(category);
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Admin")]
        [Route("/categories/{id:int}/edit")]
        [Route("Category/Edit/{id:int}")]
        public IActionResult Edit(int id)
        {
            var category = _repository.GetById(id);
            if (category == null)
            {
                return NotFound();
            }

            var model = new CategoryFormModel
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description
            };

            return View(model);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("/categories/{id:int}/edit")]
        [Route("Category/Edit/{id:int}")]
        public IActionResult Edit(int id, CategoryFormModel model)
        {
            if (id != model.Id)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var category = _repository.GetById(id);
            if (category == null)
            {
                return NotFound();
            }

            category.Name = model.Name;
            category.Description = model.Description;

            _repository.Update(category);
            return RedirectToAction(nameof(Details), new { id });
        }

        [Authorize(Roles = "Admin")]
        [Route("/categories/{id:int}/delete")]
        [Route("Category/Delete/{id:int}")]
        public IActionResult Delete(int id)
        {
            var category = _repository.GetById(id);
            if (category == null)
            {
                return NotFound();
            }

            ViewBag.CanDelete = _repository.CanDelete(id);
            return View(category);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Delete")]
        [Route("/categories/{id:int}/delete")]
        [Route("Category/Delete/{id:int}")]
        public IActionResult DeleteConfirmed(int id)
        {
            var deleted = _repository.Delete(id);

            if (!deleted)
            {
                var category = _repository.GetById(id);
                if (category != null)
                {
                    ViewBag.CanDelete = false;
                    ModelState.AddModelError("", "Cannot delete this category because it is used by one or more products.");
                    return View(nameof(Delete), category);
                }
                return NotFound();
            }

            return RedirectToAction(nameof(Index));
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("/categories/search")]
        public IActionResult Search(string? term)
        {
            var categories = _repository.Search(term);
            return PartialView("_CategoryTableRows", categories);
        }
    }
}
