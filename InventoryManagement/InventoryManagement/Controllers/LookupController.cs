using Microsoft.AspNetCore.Mvc;
using InventoryManagement.DAL.Repositories;

namespace InventoryManagement.Controllers
{
    [Route("lookup")]
    [ApiController]
    public class LookupController : Controller
    {
        private readonly CategoryEfRepository _categoryRepository;
        private readonly SupplierEfRepository _supplierRepository;
        private readonly UserEfRepository _userRepository;

        public LookupController(CategoryEfRepository categoryRepository, SupplierEfRepository supplierRepository, UserEfRepository userRepository)
        {
            _categoryRepository = categoryRepository;
            _supplierRepository = supplierRepository;
            _userRepository = userRepository;
        }

        /// <summary>
        /// AJAX endpoint for category autocomplete search.
        /// Returns JSON array with id and text properties for use with jQuery Autocomplete or similar controls.
        /// </summary>
        [HttpGet("categories")]
        public JsonResult GetCategories(string? term)
        {
            var categories = _categoryRepository.Search(term);
            var result = categories.Select(c => new { id = c.Id, text = c.Name }).ToList();
            return Json(result);
        }

        /// <summary>
        /// AJAX endpoint for supplier autocomplete search.
        /// Returns JSON array with id and text properties for use with jQuery Autocomplete or similar controls.
        /// </summary>
        [HttpGet("suppliers")]
        public JsonResult GetSuppliers(string? term)
        {
            var suppliers = _supplierRepository.Search(term);
            var result = suppliers.Select(s => new { id = s.Id, text = s.Name }).ToList();
            return Json(result);
        }

        /// <summary>
        /// AJAX endpoint for user autocomplete search.
        /// Returns JSON array with id and text properties for use with jQuery Autocomplete or similar controls.
        /// Text format: "FirstName LastName (email@example.com)"
        /// </summary>
        [HttpGet("users")]
        public JsonResult GetUsers(string? term)
        {
            var users = _userRepository.Search(term, 10);
            var result = users.Select(u => new { id = u.Id, text = $"{u.FirstName} {u.LastName} ({u.Email})" }).ToList();
            return Json(result);
        }
    }
}
