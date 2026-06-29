using InventoryManagement.DAL.Repositories;
using InventoryManagement.Domain.Enums;
using InventoryManagement.Domain.Models;
using InventoryManagement.ViewModels.Order;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InventoryManagement.Controllers
{
    [Authorize]
    public class OrderController : Controller
    {
        private readonly OrderEfRepository _repository;

        public OrderController(OrderEfRepository repository)
        {
            _repository = repository;
        }

        [AllowAnonymous]
        [Route("/orders/history")]
        [Route("/Order")]
        [Route("/Order/Index")]
        public IActionResult Index()
        {
            var orders = _repository.GetAll();
            return View(orders);
        }

        [Route("/orders/{id:int}/summary")]
        [Route("/Order/Details/{id:int}")]
        public IActionResult Details(int id)
        {
            var order = _repository.GetById(id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        [Authorize(Roles = "Admin")]
        [Route("/orders/create")]
        [Route("/Order/Create")]
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

        [Authorize(Roles = "Admin")]
        [Route("/orders/create")]
        [Route("/Order/Create")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(OrderFormModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            if (model.UserId is not int userId)
            {
                ModelState.AddModelError(nameof(model.UserId), "User is required.");
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
                UserId = userId
            };

            _repository.Add(order);
            return RedirectToAction(nameof(Details), new { id = order.Id });
        }

        [Authorize(Roles = "Admin")]
        [Route("/orders/{id:int}/edit")]
        [Route("/Order/Edit/{id:int}")]
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

        [Authorize(Roles = "Admin")]
        [Route("/orders/{id:int}/edit")]
        [Route("/Order/Edit/{id:int}")]
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

            if (model.UserId is not int userId)
            {
                ModelState.AddModelError(nameof(model.UserId), "User is required.");
                return View(model);
            }

            order.OrderNumber = model.OrderNumber;
            order.OrderDate = model.OrderDate;
            order.TotalPrice = model.TotalPrice;
            order.Status = model.Status;
            order.DeliveryDate = model.DeliveryDate;
            order.Note = model.Note;
            order.UserId = userId;

            _repository.Update(order);
            return RedirectToAction(nameof(Details), new { id = order.Id });
        }

        [Authorize(Roles = "Admin")]
        [Route("/orders/{id:int}/delete")]
        [Route("/Order/Delete/{id:int}")]
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

        [Authorize(Roles = "Admin")]
        [Route("/orders/{id:int}/delete")]
        [Route("/Order/Delete/{id:int}")]
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

            return RedirectToAction(nameof(Index));
        }

        [AllowAnonymous]
        [Route("/orders/search")]
        [HttpGet]
        public IActionResult Search(string? term)
        {
            var orders = _repository.Search(term);
            return PartialView("_OrderTableRows", orders);
        }
    }
}
