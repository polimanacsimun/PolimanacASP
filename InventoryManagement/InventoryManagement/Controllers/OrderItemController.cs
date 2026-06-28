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

        [AllowAnonymous]
        [Route("/order-items")]
        [Route("/OrderItem")]
        [Route("/OrderItem/Index")]
        [HttpGet]
        public IActionResult Index()
        {
            var items = _repository.GetAll();
            return View(items);
        }

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

        [Authorize(Roles = "Admin")]
        [Route("/order-items/create")]
        [Route("/OrderItem/Create")]
        [HttpGet]
        public IActionResult Create()
        {
            var model = new OrderItemFormModel
            {
                CreatedAt = DateTime.Now
            };
            return View(model);
        }

        [Authorize(Roles = "Admin")]
        [Route("/order-items/create")]
        [Route("/OrderItem/Create")]
        [HttpPost]
        [ValidateAntiForgeryToken]
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
            return RedirectToAction(nameof(Details), new { id = orderItem.Id });
        }

        [Authorize(Roles = "Admin")]
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

        [Authorize(Roles = "Admin")]
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
            return RedirectToAction(nameof(Details), new { id });
        }

        [Authorize(Roles = "Admin")]
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

        [Authorize(Roles = "Admin")]
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

            return RedirectToAction(nameof(Index));
        }

        [AllowAnonymous]
        [Route("/order-items/search")]
        [HttpGet]
        public IActionResult Search(string? term)
        {
            var items = _repository.Search(term);
            return PartialView("_OrderItemTableRows", items);
        }
    }
}
