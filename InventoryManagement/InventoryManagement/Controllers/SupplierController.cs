using Microsoft.AspNetCore.Mvc;
using InventoryManagement.Repositories.Mock;

namespace InventoryManagement.Controllers
{
    public class SupplierController : Controller
    {
        private readonly SupplierMockRepository _repository;

        public SupplierController(SupplierMockRepository repository)
        {
            _repository = repository;
        }

        public IActionResult Index()
        {
            var suppliers = _repository.GetAll();
            return View(suppliers);
        }

        public IActionResult Details(int id)
        {
            var supplier = _repository.GetById(id);
            if (supplier == null)
            {
                return NotFound();
            }

            return View(supplier);
        }
    }
}
