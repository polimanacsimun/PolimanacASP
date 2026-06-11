using InventoryManagement.DAL.Repositories;
using InventoryManagement.Domain.Models;
using InventoryManagement.ViewModels.Product;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InventoryManagement.Controllers
{
    [Authorize]
    public class ProductController : Controller
    {
        private readonly ProductEfRepository _repository;

        public ProductController(ProductEfRepository repository)
        {
            _repository = repository;
        }

        [AllowAnonymous]
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

        [Authorize(Roles = "Admin,Manager")]
        [Route("catalog/create")]
        [Route("Product/Create")]
        public IActionResult Create()
        {
            var model = new ProductFormModel
            {
                CreatedAt = DateTime.Today,
                IsActive = true
            };

            return View(model);
        }

        [Authorize(Roles = "Admin,Manager")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("catalog/create")]
        [Route("Product/Create")]
        public IActionResult Create(ProductFormModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var product = new Product
            {
                Name = model.Name,
                Description = model.Description,
                Price = model.Price,
                UnitOfMeasure = model.UnitOfMeasure,
                MinimumStock = model.MinimumStock,
                CreatedAt = model.CreatedAt,
                IsActive = model.IsActive,
                Type = model.Type,
                CategoryId = model.CategoryId,
                SupplierId = model.SupplierId
            };

            _repository.Add(product);
            TempData["ToastMessage"] = "Product created successfully.";
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Admin,Manager")]
        [Route("catalog/{id:int}/edit")]
        [Route("Product/Edit/{id:int}")]
        public IActionResult Edit(int id)
        {
            var product = _repository.GetById(id);
            if (product == null)
            {
                return NotFound();
            }

            var model = new ProductFormModel
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                UnitOfMeasure = product.UnitOfMeasure,
                MinimumStock = product.MinimumStock,
                CreatedAt = product.CreatedAt,
                IsActive = product.IsActive,
                Type = product.Type,
                CategoryId = product.CategoryId,
                CategoryDisplayName = product.Category?.Name,
                SupplierId = product.SupplierId,
                SupplierDisplayName = product.Supplier?.Name
            };

            return View(model);
        }

        [Authorize(Roles = "Admin,Manager")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("catalog/{id:int}/edit")]
        [Route("Product/Edit/{id:int}")]
        public IActionResult Edit(int id, ProductFormModel model)
        {
            if (id != model.Id)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var product = _repository.GetById(id);
            if (product == null)
            {
                return NotFound();
            }

            product.Name = model.Name;
            product.Description = model.Description;
            product.Price = model.Price;
            product.UnitOfMeasure = model.UnitOfMeasure;
            product.MinimumStock = model.MinimumStock;
            product.CreatedAt = model.CreatedAt;
            product.IsActive = model.IsActive;
            product.Type = model.Type;
            product.CategoryId = model.CategoryId;
            product.SupplierId = model.SupplierId;

            _repository.Update(product);
            TempData["ToastMessage"] = "Product updated successfully.";
            return RedirectToAction(nameof(Details), new { id });
        }

        [Authorize(Roles = "Admin")]
        [Route("catalog/{id:int}/delete")]
        [Route("Product/Delete/{id:int}")]
        public IActionResult Delete(int id)
        {
            var product = _repository.GetById(id);
            if (product == null)
            {
                return NotFound();
            }

            ViewBag.CanDelete = _repository.CanDelete(id);
            return View(product);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Delete")]
        [Route("catalog/{id:int}/delete")]
        [Route("Product/Delete/{id:int}")]
        public IActionResult DeleteConfirmed(int id)
        {
            var deleted = _repository.Delete(id);

            if (!deleted)
            {
                var product = _repository.GetById(id);
                if (product != null)
                {
                    ViewBag.CanDelete = false;
                    ModelState.AddModelError("", "Cannot delete this product because it has related orders or inventory items.");
                    return View(nameof(Delete), product);
                }

                return NotFound();
            }

            TempData["ToastMessage"] = "Product deleted successfully.";
            return RedirectToAction(nameof(Index));
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("catalog/search")]
        public IActionResult Search(string? term)
        {
            var products = _repository.Search(term);
            return PartialView("_ProductTableRows", products);
        }
    }
}
