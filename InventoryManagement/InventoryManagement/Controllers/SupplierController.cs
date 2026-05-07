using Microsoft.AspNetCore.Mvc;
using InventoryManagement.DAL.Repositories;

namespace InventoryManagement.Controllers
{
    public class SupplierController : Controller
    {
        private readonly SupplierEfRepository _repository;

        public SupplierController(SupplierEfRepository repository)
        {
            _repository = repository;
        }

        [Route("/vendors")]
        public IActionResult Index()
        {
            var suppliers = _repository.GetAll();
            return View(suppliers);
        }

        [Route("/vendors/{id:int}")]
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