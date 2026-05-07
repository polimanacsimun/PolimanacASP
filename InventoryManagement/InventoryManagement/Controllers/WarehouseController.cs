using Microsoft.AspNetCore.Mvc;
using InventoryManagement.DAL.Repositories;

namespace InventoryManagement.Controllers
{
    public class WarehouseController : Controller
    {
        private readonly WarehouseEfRepository _repository;

        public WarehouseController(WarehouseEfRepository repository)
        {
            _repository = repository;
        }

        public IActionResult Index()
        {
            var warehouses = _repository.GetAll();
            return View(warehouses);
        }

        public IActionResult Details(int id)
        {
            var warehouse = _repository.GetById(id);
            if (warehouse == null)
            {
                return NotFound();
            }

            return View(warehouse);
        }
    }
}