using InventoryManagement.DAL.Repositories;
using InventoryManagement.Domain.Models;
using InventoryManagement.ViewModels.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InventoryManagement.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        private readonly UserEfRepository _repository;

        public UserController(UserEfRepository repository)
        {
            _repository = repository;
        }

        [AllowAnonymous]
        [Route("users")]
        [Route("User")]
        [Route("User/Index")]
        public IActionResult Index()
        {
            var users = _repository.GetAll();
            return View(users);
        }

        [Route("users/{id:int}")]
        [Route("User/Details/{id:int}")]
        public IActionResult Details(int id)
        {
            var user = _repository.GetById(id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        [Authorize(Roles = "Admin")]
        [Route("users/create")]
        [Route("User/Create")]
        [HttpGet]
        public IActionResult Create()
        {
            var model = new UserFormModel
            {
                RegistrationDate = DateTime.Today,
                IsActive = true
            };
            return View(model);
        }

        [Authorize(Roles = "Admin")]
        [Route("users/create")]
        [Route("User/Create")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(UserFormModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = new User
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                Role = model.Role,
                RegistrationDate = model.RegistrationDate,
                IsActive = model.IsActive
            };

            _repository.Add(user);
            return RedirectToAction(nameof(Details), new { id = user.Id });
        }

        [Authorize(Roles = "Admin")]
        [Route("users/{id:int}/edit")]
        [Route("User/Edit/{id:int}")]
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var user = _repository.GetById(id);
            if (user == null)
            {
                return NotFound();
            }

            var model = new UserFormModel
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Role = user.Role,
                RegistrationDate = user.RegistrationDate,
                IsActive = user.IsActive
            };

            return View(model);
        }

        [Authorize(Roles = "Admin")]
        [Route("users/{id:int}/edit")]
        [Route("User/Edit/{id:int}")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, UserFormModel model)
        {
            if (id != model.Id)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = _repository.GetById(id);
            if (user == null)
            {
                return NotFound();
            }

            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.Email = model.Email;
            user.Role = model.Role;
            user.RegistrationDate = model.RegistrationDate;
            user.IsActive = model.IsActive;

            _repository.Update(user);
            return RedirectToAction(nameof(Details), new { id = user.Id });
        }

        [Authorize(Roles = "Admin")]
        [Route("users/{id:int}/delete")]
        [Route("User/Delete/{id:int}")]
        [HttpGet]
        public IActionResult Delete(int id)
        {
            var user = _repository.GetById(id);
            if (user == null)
            {
                return NotFound();
            }

            ViewBag.CanDelete = _repository.CanDelete(id);
            return View(user);
        }

        [Authorize(Roles = "Admin")]
        [Route("users/{id:int}/delete")]
        [Route("User/Delete/{id:int}")]
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var user = _repository.GetById(id);
            if (user == null)
            {
                return NotFound();
            }

            if (!_repository.Delete(id))
            {
                ViewBag.CanDelete = false;
                ModelState.AddModelError("", "Cannot delete this user because they have placed one or more orders. Please contact administration if this user needs to be removed.");
                return View("Delete", user);
            }

            return RedirectToAction(nameof(Index));
        }

        [AllowAnonymous]
        [Route("users/search")]
        [HttpGet]
        public IActionResult Search(string? term)
        {
            var users = _repository.Search(term);
            return PartialView("_UserTableRows", users);
        }
    }
}
