using Microsoft.AspNetCore.Mvc;
using InventoryManagement.DAL.Repositories;
using InventoryManagement.Domain.Models;
using InventoryManagement.ViewModels.Warehouse;

namespace InventoryManagement.Controllers
{
    public class WarehouseController : Controller
    {
        private readonly WarehouseEfRepository _repository;

        public WarehouseController(WarehouseEfRepository repository)
        {
            _repository = repository;
        }

        [Route("/storage")]
        public IActionResult Index()
        {
            var warehouses = _repository.GetAll();
            return View(warehouses);
        }

        [Route("/storage/{id:int}")]
        public IActionResult Details(int id)
        {
            var warehouse = _repository.GetById(id);
            if (warehouse == null)
            {
                return NotFound();
            }

            return View(warehouse);
        }

        [Route("/storage/create")]
        [HttpGet]
        public IActionResult Create()
        {
            var model = new WarehouseFormModel
            {
                OpeningDate = DateTime.Today,
                IsActive = true
            };
            return View(model);
        }

        [Route("/storage/create")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(WarehouseFormModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var warehouse = new Warehouse
            {
                Name = model.Name,
                Address = model.Address,
                Capacity = model.Capacity,
                Phone = model.Phone,
                Email = model.Email,
                Manager = model.Manager,
                OpeningDate = model.OpeningDate,
                IsActive = model.IsActive,
                Type = model.Type
            };

            _repository.Add(warehouse);
            return RedirectToRoute(new { controller = "Warehouse", action = "Details", id = warehouse.Id });
        }

        [Route("/storage/{id:int}/edit")]
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var warehouse = _repository.GetById(id);
            if (warehouse == null)
            {
                return NotFound();
            }

            var model = new WarehouseFormModel
            {
                Id = warehouse.Id,
                Name = warehouse.Name,
                Address = warehouse.Address,
                Capacity = warehouse.Capacity,
                Phone = warehouse.Phone,
                Email = warehouse.Email,
                Manager = warehouse.Manager,
                OpeningDate = warehouse.OpeningDate,
                IsActive = warehouse.IsActive,
                Type = warehouse.Type
            };

            return View(model);
        }

        [Route("/storage/{id:int}/edit")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, WarehouseFormModel model)
        {
            if (id != model.Id)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var warehouse = _repository.GetById(id);
            if (warehouse == null)
            {
                return NotFound();
            }

            warehouse.Name = model.Name;
            warehouse.Address = model.Address;
            warehouse.Capacity = model.Capacity;
            warehouse.Phone = model.Phone;
            warehouse.Email = model.Email;
            warehouse.Manager = model.Manager;
            warehouse.OpeningDate = model.OpeningDate;
            warehouse.IsActive = model.IsActive;
            warehouse.Type = model.Type;

            _repository.Update(warehouse);
            return RedirectToRoute(new { controller = "Warehouse", action = "Details", id = warehouse.Id });
        }

        [Route("/storage/{id:int}/delete")]
        [HttpGet]
        public IActionResult Delete(int id)
        {
            var warehouse = _repository.GetById(id);
            if (warehouse == null)
            {
                return NotFound();
            }

            ViewBag.CanDelete = _repository.CanDelete(id);
            return View(warehouse);
        }

        [Route("/storage/{id:int}/delete")]
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var warehouse = _repository.GetById(id);
            if (warehouse == null)
            {
                return NotFound();
            }

            if (!_repository.Delete(id))
            {
                ViewBag.CanDelete = false;
                ModelState.AddModelError("", "Cannot delete this warehouse because it is currently used by one or more inventory items. Please remove all inventory items from this warehouse before attempting to delete it.");
                return View("Delete", warehouse);
            }

            return RedirectToRoute(new { controller = "Warehouse", action = "Index" });
        }

        [Route("/storage/search")]
        [HttpGet]
        public IActionResult Search(string? term)
        {
            var warehouses = _repository.Search(term);
            return PartialView("_WarehouseTableRows", warehouses);
        }

        // Legacy routes for backward compatibility
        [Route("/Warehouse")]
        [Route("/Warehouse/Index")]
        public IActionResult LegacyIndex()
        {
            return RedirectToRoute(new { controller = "Warehouse", action = "Index" });
        }

        [Route("/Warehouse/Details/{id:int}")]
        public IActionResult LegacyDetails(int id)
        {
            return RedirectToRoute(new { controller = "Warehouse", action = "Details", id });
        }

        [Route("/Warehouse/Create")]
        public IActionResult LegacyCreate()
        {
            return RedirectToRoute(new { controller = "Warehouse", action = "Create" });
        }

        [Route("/Warehouse/Edit/{id:int}")]
        public IActionResult LegacyEdit(int id)
        {
            return RedirectToRoute(new { controller = "Warehouse", action = "Edit", id });
        }

        [Route("/Warehouse/Delete/{id:int}")]
        public IActionResult LegacyDelete(int id)
        {
            return RedirectToRoute(new { controller = "Warehouse", action = "Delete", id });
        }
    }
}