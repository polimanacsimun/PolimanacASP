using Microsoft.AspNetCore.Mvc;
using InventoryManagement.Repositories.Mock;

namespace InventoryManagement.Controllers
{
    public class WarehouseController : Controller
    {
        private readonly WarehouseMockRepository _repository;

        public WarehouseController(WarehouseMockRepository repository)
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
