using Microsoft.AspNetCore.Mvc;
using InventoryManagement.Repositories.Mock;

namespace InventoryManagement.Controllers
{
    public class UserController : Controller
    {
        private readonly UserMockRepository _repository;

        public UserController(UserMockRepository repository)
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
