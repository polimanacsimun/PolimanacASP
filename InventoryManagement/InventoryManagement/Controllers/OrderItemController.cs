using InventoryManagement.DAL.Repositories;
using InventoryManagement.Domain.Models;
using InventoryManagement.ViewModels.OrderItem;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InventoryManagement.Controllers
{
    [Authorize]
    public class OrderItemController : Controller
    {
        private readonly OrderItemEfRepository _repository;

        public OrderItemController(OrderItemEfRepository repository)
        {
            _repository = repository;
        }

        /// <summary>
        /// Displays list of all order items.
        /// Supports both semantic (/order-items) and legacy (/OrderItem) routes.
        /// </summary>
        [Route("/order-items")]
        [Route("/OrderItem")]
        [Route("/OrderItem/Index")]
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Index()
        {
            var items = _repository.GetAll();
            return View(items);
        }

        /// <summary>
        /// Displays details for a specific order item.
        /// Supports both semantic (/order-items/{id}) and legacy (/OrderItem/Details/{id}) routes.
        /// </summary>
        [Route("/order-items/{id:int}")]
        [Route("/OrderItem/Details/{id:int}")]
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
        /// Displays the create form for a new order item.
        /// Initializes CreatedAt to current date/time.
        /// </summary>
        [Route("/order-items/create")]
        [Route("/OrderItem/Create")]
        [HttpGet]
        [Authorize(Roles = "Admin,Manager")]
        public IActionResult Create()
        {
            var model = new OrderItemFormModel
            {
                CreatedAt = DateTime.Now
            };
            return View(model);
        }

        /// <summary>
        /// Processes creation of a new order item.
        /// Maps form model to domain model and saves to database.
        /// Redirects to Details on success.
        /// </summary>
        [Route("/order-items/create")]
        [Route("/OrderItem/Create")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Manager")]
        public IActionResult Create(OrderItemFormModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var orderItem = new OrderItem
            {
                Quantity = model.Quantity,
                UnitPrice = model.UnitPrice,
                TotalPrice = model.TotalPrice,
                Discount = model.Discount,
                CreatedAt = model.CreatedAt,
                OrderId = model.OrderId.Value,
                ProductId = model.ProductId.Value
            };

            _repository.Add(orderItem);
            return RedirectToAction("Details", new { id = orderItem.Id });
        }

        /// <summary>
        /// Displays the edit form for an existing order item.
        /// Populates display names from related Order and Product.
        /// Returns NotFound if item doesn't exist.
        /// </summary>
        [Route("/order-items/{id:int}/edit")]
        [Route("/OrderItem/Edit/{id:int}")]
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var item = _repository.GetById(id);
            if (item == null)
            {
                return NotFound();
            }

            var model = new OrderItemFormModel
            {
                Id = item.Id,
                Quantity = item.Quantity,
                UnitPrice = item.UnitPrice,
                TotalPrice = item.TotalPrice,
                Discount = item.Discount,
                CreatedAt = item.CreatedAt,
                OrderId = item.OrderId,
                OrderDisplayName = item.Order?.OrderNumber,
                ProductId = item.ProductId,
                ProductDisplayName = item.Product?.Name
            };

            return View(model);
        }

        /// <summary>
        /// Processes updates to an existing order item.
        /// Validates that ID in URL matches form model ID.
        /// Redirects to Details on success.
        /// </summary>
        [Route("/order-items/{id:int}/edit")]
        [Route("/OrderItem/Edit/{id:int}")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, OrderItemFormModel model)
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

            var orderItem = new OrderItem
            {
                Id = model.Id,
                Quantity = model.Quantity,
                UnitPrice = model.UnitPrice,
                TotalPrice = model.TotalPrice,
                Discount = model.Discount,
                CreatedAt = model.CreatedAt,
                OrderId = model.OrderId.Value,
                ProductId = model.ProductId.Value
            };

            _repository.Update(orderItem);
            return RedirectToAction("Details", new { id });
        }

        /// <summary>
        /// Displays delete confirmation page for an order item.
        /// Sets ViewBag.CanDelete to control button visibility.
        /// Returns NotFound if item doesn't exist.
        /// </summary>
        [Route("/order-items/{id:int}/delete")]
        [Route("/OrderItem/Delete/{id:int}")]
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
        /// Processes deletion of an order item.
        /// Returns false if item has dependent records; redisplays delete view with error.
        /// Redirects to Index on successful deletion.
        /// </summary>
        [Route("/order-items/{id:int}/delete")]
        [Route("/OrderItem/Delete/{id:int}")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            if (!_repository.Delete(id))
            {
                var item = _repository.GetById(id);
                ViewBag.CanDelete = false;
                ViewBag.ErrorMessage = "Cannot delete this order item. Please check for dependencies.";
                return View("Delete", item);
            }

            return RedirectToAction("Index");
        }

        /// <summary>
        /// AJAX endpoint for searching order items.
        /// Returns partial view with table rows matching the search term.
        /// Supports searching by order number and product name.
        /// </summary>
        [Route("/order-items/search")]
        [HttpGet]
        public IActionResult Search(string? term)
        {
            var items = _repository.Search(term);
            return PartialView("_OrderItemTableRows", items);
        }
    }
}
