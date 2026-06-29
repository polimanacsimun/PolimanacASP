using InventoryManagement.DAL.Repositories;
using InventoryManagement.Domain.Models;
using InventoryManagement.ViewModels.InventoryItem;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InventoryManagement.Controllers
{
    [Authorize]
    public class InventoryItemController : Controller
    {
        private readonly InventoryItemEfRepository _repository;

        public InventoryItemController(InventoryItemEfRepository repository)
        {
            _repository = repository;
        }

        [AllowAnonymous]
        [Route("/inventory")]
        [Route("/InventoryItem")]
        [Route("/InventoryItem/Index")]
        [HttpGet]
        public IActionResult Index()
        {
            var items = _repository.GetAll();
            return View(items);
        }

        [Route("/inventory/{id:int}")]
        [Route("/InventoryItem/Details/{id:int}")]
        [HttpGet]
        public IActionResult Details(int id)
        {
            var item = _repository.GetById(id);
            if (item == null)
            {
                return NotFound();
            }

            return View(item);
        }

        [Authorize(Roles = "Admin")]
        [Route("/inventory/create")]
        [Route("/InventoryItem/Create")]
        [HttpGet]
        public IActionResult Create()
        {
            var model = new InventoryItemFormModel
            {
                LastCheckedAt = DateTime.Now
            };
            return View(model);
        }

        [Authorize(Roles = "Admin")]
        [Route("/inventory/create")]
        [Route("/InventoryItem/Create")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(InventoryItemFormModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            if (model.ProductId is not int productId)
            {
                ModelState.AddModelError(nameof(model.ProductId), "Product is required.");
                return View(model);
            }

            if (model.WarehouseId is not int warehouseId)
            {
                ModelState.AddModelError(nameof(model.WarehouseId), "Warehouse is required.");
                return View(model);
            }

            var inventoryItem = new InventoryItem
            {
                QuantityInStock = model.QuantityInStock,
                MinimumQuantity = model.MinimumQuantity,
                MaximumQuantity = model.MaximumQuantity,
                ShelfLocation = model.ShelfLocation,
                LastCheckedAt = model.LastCheckedAt,
                ProductId = productId,
                WarehouseId = warehouseId
            };

            _repository.Add(inventoryItem);
            return RedirectToAction(nameof(Details), new { id = inventoryItem.Id });
        }

        [Authorize(Roles = "Admin")]
        [Route("/inventory/{id:int}/edit")]
        [Route("/InventoryItem/Edit/{id:int}")]
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var item = _repository.GetById(id);
            if (item == null)
            {
                return NotFound();
            }

            var model = new InventoryItemFormModel
            {
                Id = item.Id,
                QuantityInStock = item.QuantityInStock,
                MinimumQuantity = item.MinimumQuantity,
                MaximumQuantity = item.MaximumQuantity,
                ShelfLocation = item.ShelfLocation,
                LastCheckedAt = item.LastCheckedAt,
                ProductId = item.ProductId,
                ProductDisplayName = item.Product?.Name,
                WarehouseId = item.WarehouseId,
                WarehouseDisplayName = item.Warehouse?.Name
            };

            return View(model);
        }

        [Authorize(Roles = "Admin")]
        [Route("/inventory/{id:int}/edit")]
        [Route("/InventoryItem/Edit/{id:int}")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, InventoryItemFormModel model)
        {
            if (id != model.Id)
            {
                return BadRequest("ID mismatch");
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            if (!_repository.Exists(id))
            {
                return NotFound();
            }

            if (model.ProductId is not int productId)
            {
                ModelState.AddModelError(nameof(model.ProductId), "Product is required.");
                return View(model);
            }

            if (model.WarehouseId is not int warehouseId)
            {
                ModelState.AddModelError(nameof(model.WarehouseId), "Warehouse is required.");
                return View(model);
            }

            var inventoryItem = new InventoryItem
            {
                Id = model.Id,
                QuantityInStock = model.QuantityInStock,
                MinimumQuantity = model.MinimumQuantity,
                MaximumQuantity = model.MaximumQuantity,
                ShelfLocation = model.ShelfLocation,
                LastCheckedAt = model.LastCheckedAt,
                ProductId = productId,
                WarehouseId = warehouseId
            };

            _repository.Update(inventoryItem);
            return RedirectToAction(nameof(Details), new { id });
        }

        [Authorize(Roles = "Admin")]
        [Route("/inventory/{id:int}/delete")]
        [Route("/InventoryItem/Delete/{id:int}")]
        [HttpGet]
        public IActionResult Delete(int id)
        {
            var item = _repository.GetById(id);
            if (item == null)
            {
                return NotFound();
            }

            ViewBag.CanDelete = _repository.CanDelete(id);
            return View(item);
        }

        [Authorize(Roles = "Admin")]
        [Route("/inventory/{id:int}/delete")]
        [Route("/InventoryItem/Delete/{id:int}")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            if (!_repository.Delete(id))
            {
                var item = _repository.GetById(id);
                ViewBag.CanDelete = false;
                ViewBag.ErrorMessage = "Cannot delete this inventory item. Please check for dependencies.";
                return View("Delete", item);
            }

            return RedirectToAction(nameof(Index));
        }

        [AllowAnonymous]
        [Route("/inventory/search")]
        [HttpGet]
        public IActionResult Search(string? term)
        {
            var items = _repository.Search(term);
            return PartialView("_InventoryItemTableRows", items);
        }
    }
}
