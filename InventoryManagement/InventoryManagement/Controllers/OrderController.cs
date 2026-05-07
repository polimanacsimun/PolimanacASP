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
    }
}