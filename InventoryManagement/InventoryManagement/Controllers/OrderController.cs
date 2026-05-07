using Microsoft.AspNetCore.Mvc;
using InventoryManagement.DAL.Repositories;

namespace InventoryManagement.Controllers
{
    public class OrderController : Controller
    {
        private readonly OrderEfRepository _repository;

        public OrderController(OrderEfRepository repository)
        {
            _repository = repository;
        }

        public IActionResult Index()
        {
            var orders = _repository.GetAll();
            return View(orders);
        }

        public IActionResult Details(int id)
        {
            var order = _repository.GetById(id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }
    }
}