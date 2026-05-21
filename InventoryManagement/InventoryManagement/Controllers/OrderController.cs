using Microsoft.AspNetCore.Mvc;
using InventoryManagement.DAL.Repositories;
using InventoryManagement.Domain.Models;
using InventoryManagement.Domain.Enums;
using InventoryManagement.ViewModels.Order;

namespace InventoryManagement.Controllers
{
    public class OrderController : Controller
    {
        private readonly OrderEfRepository _repository;

        public OrderController(OrderEfRepository repository)
        {
            _repository = repository;
        }

        [Route("/orders/history")]
        public IActionResult Index()
        {
            var orders = _repository.GetAll();
            return View(orders);
        }

        [Route("/orders/{id:int}/summary")]
        public IActionResult Details(int id)
        {
            var order = _repository.GetById(id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        [Route("/orders/create")]
        [HttpGet]
        public IActionResult Create()
        {
            var model = new OrderFormModel
            {
                OrderDate = DateTime.Today,
                Status = OrderStatus.Pending
            };
            return View(model);
        }

        [Route("/orders/create")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(OrderFormModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var order = new Order
            {
                OrderNumber = model.OrderNumber,
                OrderDate = model.OrderDate,
                TotalPrice = model.TotalPrice,
                Status = model.Status,
                DeliveryDate = model.DeliveryDate,
                Note = model.Note,
                UserId = model.UserId.Value
            };

            _repository.Add(order);
            return RedirectToRoute(new { controller = "Order", action = "Details", id = order.Id });
        }

        [Route("/orders/{id:int}/edit")]
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var order = _repository.GetById(id);
            if (order == null)
            {
                return NotFound();
            }

            var model = new OrderFormModel
            {
                Id = order.Id,
                OrderNumber = order.OrderNumber,
                OrderDate = order.OrderDate,
                TotalPrice = order.TotalPrice,
                Status = order.Status,
                DeliveryDate = order.DeliveryDate,
                Note = order.Note,
                UserId = order.UserId,
                UserDisplayName = order.User != null 
                    ? $"{order.User.FirstName} {order.User.LastName} ({order.User.Email})" 
                    : null
            };

            return View(model);
        }

        [Route("/orders/{id:int}/edit")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, OrderFormModel model)
        {
            if (id != model.Id)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var order = _repository.GetById(id);
            if (order == null)
            {
                return NotFound();
            }

            order.OrderNumber = model.OrderNumber;
            order.OrderDate = model.OrderDate;
            order.TotalPrice = model.TotalPrice;
            order.Status = model.Status;
            order.DeliveryDate = model.DeliveryDate;
            order.Note = model.Note;
            order.UserId = model.UserId.Value;

            _repository.Update(order);
            return RedirectToRoute(new { controller = "Order", action = "Details", id = order.Id });
        }

        [Route("/orders/{id:int}/delete")]
        [HttpGet]
        public IActionResult Delete(int id)
        {
            var order = _repository.GetById(id);
            if (order == null)
            {
                return NotFound();
            }

            ViewBag.CanDelete = _repository.CanDelete(id);
            return View(order);
        }

        [Route("/orders/{id:int}/delete")]
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var order = _repository.GetById(id);
            if (order == null)
            {
                return NotFound();
            }

            if (!_repository.Delete(id))
            {
                ViewBag.CanDelete = false;
                ModelState.AddModelError("", "Cannot delete this order because it contains one or more order items. Please remove all items from this order before attempting to delete it.");
                return View("Delete", order);
            }

            return RedirectToRoute(new { controller = "Order", action = "Index" });
        }

        [Route("/orders/search")]
        [HttpGet]
        public IActionResult Search(string? term)
        {
            var orders = _repository.Search(term);
            return PartialView("_OrderTableRows", orders);
        }

        // Legacy routes for backward compatibility
        [Route("/Order")]
        [Route("/Order/Index")]
        public IActionResult LegacyIndex()
        {
            return RedirectToRoute(new { controller = "Order", action = "Index" });
        }

        [Route("/Order/Details/{id:int}")]
        public IActionResult LegacyDetails(int id)
        {
            return RedirectToRoute(new { controller = "Order", action = "Details", id });
        }

        [Route("/Order/Create")]
        public IActionResult LegacyCreate()
        {
            return RedirectToRoute(new { controller = "Order", action = "Create" });
        }

        [Route("/Order/Edit/{id:int}")]
        public IActionResult LegacyEdit(int id)
        {
            return RedirectToRoute(new { controller = "Order", action = "Edit", id });
        }

        [Route("/Order/Delete/{id:int}")]
        public IActionResult LegacyDelete(int id)
        {
            return RedirectToRoute(new { controller = "Order", action = "Delete", id });
        }
    }
}