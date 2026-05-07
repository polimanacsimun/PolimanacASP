using Microsoft.AspNetCore.Mvc;
using InventoryManagement.DAL.Repositories;

namespace InventoryManagement.Controllers
{
    public class UserController : Controller
    {
        private readonly UserEfRepository _repository;

        public UserController(UserEfRepository repository)
        {
            _repository = repository;
        }

        public IActionResult Index()
        {
            var users = _repository.GetAll();
            return View(users);
        }

        public IActionResult Details(int id)
        {
            var user = _repository.GetById(id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }
    }
}