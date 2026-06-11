using InventoryManagement.DAL.Repositories;
using InventoryManagement.Domain.Models;
using InventoryManagement.ViewModels.Supplier;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InventoryManagement.Controllers
{
    [Authorize]
    public class SupplierController : Controller
    {
        private readonly SupplierEfRepository _repository;

        public SupplierController(SupplierEfRepository repository)
        {
            _repository = repository;
        }

        [AllowAnonymous]
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

        [Authorize(Roles = "Admin,Manager")]
        [Route("/vendors/create")]
        [Route("Supplier/Create")]
        public IActionResult Create()
        {
            var model = new SupplierFormModel();
            return View(model);
        }

        [Authorize(Roles = "Admin,Manager")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("/vendors/create")]
        [Route("Supplier/Create")]
        public IActionResult Create(SupplierFormModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var supplier = new Supplier
            {
                Name = model.Name,
                Address = model.Address,
                Phone = model.Phone,
                Email = model.Email,
                ContactPerson = model.ContactPerson,
                RegistrationDate = model.RegistrationDate,
                IsActive = model.IsActive
            };

            _repository.Add(supplier);
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Admin,Manager")]
        [Route("/vendors/{id:int}/edit")]
        [Route("Supplier/Edit/{id:int}")]
        public IActionResult Edit(int id)
        {
            var supplier = _repository.GetById(id);
            if (supplier == null)
            {
                return NotFound();
            }

            var model = new SupplierFormModel
            {
                Id = supplier.Id,
                Name = supplier.Name,
                Address = supplier.Address,
                Phone = supplier.Phone,
                Email = supplier.Email,
                ContactPerson = supplier.ContactPerson,
                RegistrationDate = supplier.RegistrationDate,
                IsActive = supplier.IsActive
            };

            return View(model);
        }

        [Authorize(Roles = "Admin,Manager")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("/vendors/{id:int}/edit")]
        [Route("Supplier/Edit/{id:int}")]
        public IActionResult Edit(int id, SupplierFormModel model)
        {
            if (id != model.Id)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var supplier = _repository.GetById(id);
            if (supplier == null)
            {
                return NotFound();
            }

            supplier.Name = model.Name;
            supplier.Address = model.Address;
            supplier.Phone = model.Phone;
            supplier.Email = model.Email;
            supplier.ContactPerson = model.ContactPerson;
            supplier.RegistrationDate = model.RegistrationDate;
            supplier.IsActive = model.IsActive;

            _repository.Update(supplier);
            return RedirectToAction(nameof(Details), new { id });
        }

        [Authorize(Roles = "Admin")]
        [Route("/vendors/{id:int}/delete")]
        [Route("Supplier/Delete/{id:int}")]
        public IActionResult Delete(int id)
        {
            var supplier = _repository.GetById(id);
            if (supplier == null)
            {
                return NotFound();
            }

            ViewBag.CanDelete = _repository.CanDelete(id);
            return View(supplier);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Delete")]
        [Route("/vendors/{id:int}/delete")]
        [Route("Supplier/Delete/{id:int}")]
        public IActionResult DeleteConfirmed(int id)
        {
            var deleted = _repository.Delete(id);

            if (!deleted)
            {
                var supplier = _repository.GetById(id);
                if (supplier != null)
                {
                    ViewBag.CanDelete = false;
                    ModelState.AddModelError("", "Cannot delete this supplier because it is used by one or more products.");
                    return View(nameof(Delete), supplier);
                }
                return NotFound();
            }

            return RedirectToAction(nameof(Index));
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("/vendors/search")]
        public IActionResult Search(string? term)
        {
            var suppliers = _repository.Search(term);
            return PartialView("_SupplierTableRows", suppliers);
        }
    }
}
