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

        /// <summary>
        /// Displays list of all inventory items.
        /// Supports both semantic (/inventory) and legacy (/InventoryItem) routes.
        /// </summary>
        [Route("/inventory")]
        [Route("/InventoryItem")]
        [Route("/InventoryItem/Index")]
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Index()
        {
            var items = _repository.GetAll();
            return View(items);
        }

        /// <summary>
        /// Displays details for a specific inventory item.
        /// Supports both semantic (/inventory/{id}) and legacy (/InventoryItem/Details/{id}) routes.
        /// </summary>
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

        /// <summary>
        /// Displays the create form for a new inventory item.
        /// Initializes LastCheckedAt to current date/time.
        /// </summary>
        [Route("/inventory/create")]
        [Route("/InventoryItem/Create")]
        [HttpGet]
        [Authorize(Roles = "Admin,Manager")]
        public IActionResult Create()
        {
            var model = new InventoryItemFormModel
            {
                LastCheckedAt = DateTime.Now
            };
            return View(model);
        }

        /// <summary>
        /// Processes creation of a new inventory item.
        /// Maps form model to domain model and saves to database.
        /// Redirects to Details on success.
        /// </summary>
        [Route("/inventory/create")]
        [Route("/InventoryItem/Create")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Manager")]
        public IActionResult Create(InventoryItemFormModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var inventoryItem = new InventoryItem
            {
                QuantityInStock = model.QuantityInStock,
                MinimumQuantity = model.MinimumQuantity,
                MaximumQuantity = model.MaximumQuantity,
                ShelfLocation = model.ShelfLocation,
                LastCheckedAt = model.LastCheckedAt,
                ProductId = model.ProductId.Value,
                WarehouseId = model.WarehouseId.Value
            };

            _repository.Add(inventoryItem);
            return RedirectToAction("Details", new { id = inventoryItem.Id });
        }

        /// <summary>
        /// Displays the edit form for an existing inventory item.
        /// Populates display names from related Product and Warehouse.
        /// Returns NotFound if item doesn't exist.
        /// </summary>
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

        /// <summary>
        /// Processes updates to an existing inventory item.
        /// Validates that ID in URL matches form model ID.
        /// Redirects to Details on success.
        /// </summary>
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

            var inventoryItem = new InventoryItem
            {
                Id = model.Id,
                QuantityInStock = model.QuantityInStock,
                MinimumQuantity = model.MinimumQuantity,
                MaximumQuantity = model.MaximumQuantity,
                ShelfLocation = model.ShelfLocation,
                LastCheckedAt = model.LastCheckedAt,
                ProductId = model.ProductId.Value,
                WarehouseId = model.WarehouseId.Value
            };

            _repository.Update(inventoryItem);
            return RedirectToAction("Details", new { id });
        }

        /// <summary>
        /// Displays delete confirmation page for an inventory item.
        /// Sets ViewBag.CanDelete to control button visibility.
        /// Returns NotFound if item doesn't exist.
        /// </summary>
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

        /// <summary>
        /// Processes deletion of an inventory item.
        /// Returns false if item has dependent records; redisplays delete view with error.
        /// Redirects to Index on successful deletion.
        /// </summary>
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

            return RedirectToAction("Index");
        }

        /// <summary>
        /// AJAX endpoint for searching inventory items.
        /// Returns partial view with table rows matching the search term.
        /// Supports searching by Product name, Warehouse name, and Shelf location.
        /// </summary>
        [Route("/inventory/search")]
        [HttpGet]
        public IActionResult Search(string? term)
        {
            var items = _repository.Search(term);
            return PartialView("_InventoryItemTableRows", items);
        }
    }
}
