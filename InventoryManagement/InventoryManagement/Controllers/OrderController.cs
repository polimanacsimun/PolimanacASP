using Microsoft.AspNetCore.Mvc;
using InventoryManagement.Repositories.Mock;

namespace InventoryManagement.Controllers
{
    public class OrderController : Controller
    {
        private readonly OrderMockRepository _repository;

        public OrderController(OrderMockRepository repository)
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
