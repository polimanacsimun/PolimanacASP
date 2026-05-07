using InventoryManagement.Models;
using InventoryManagement.DAL.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Linq;

namespace InventoryManagement.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ProductEfRepository _productRepository;
         private readonly SupplierEfRepository _supplierRepository;
        private readonly WarehouseEfRepository _warehouseRepository;
        private readonly UserEfRepository _userRepository;
        private readonly OrderEfRepository _orderRepository;

        public HomeController(
            ILogger<HomeController> logger,
            ProductEfRepository productRepository,
            SupplierEfRepository supplierRepository,
            WarehouseEfRepository warehouseRepository,
            UserEfRepository userRepository,
            OrderEfRepository orderRepository)
        {
            _logger = logger;
            _productRepository = productRepository;
            _supplierRepository = supplierRepository;
            _warehouseRepository = warehouseRepository;
            _userRepository = userRepository;
            _orderRepository = orderRepository;
        }

        [Route("")]
        [Route("dashboard")]
        [Route("Home")]
        [Route("Home/Index")]
        public IActionResult Index()
{
    try
    {
        var allProducts = _productRepository.GetAll();
        var allSuppliers = _supplierRepository.GetAll();
        var allWarehouses = _warehouseRepository.GetAll();
        var allUsers = _userRepository.GetAll();
        var allOrders = _orderRepository.GetAll();

        int totalProducts = allProducts?.Count ?? 0;
        int activeSuppliers = allSuppliers?.Count(s => s.IsActive) ?? 0;
        int warehouseCapacity = allWarehouses?.Sum(w => w.Capacity) ?? 0;
        int totalUsers = allUsers?.Count ?? 0;
        int totalOrders = allOrders?.Count ?? 0;
        int warehouseCount = allWarehouses?.Count ?? 0;

        ViewBag.TotalProducts = totalProducts;
        ViewBag.ActiveSuppliers = activeSuppliers;
        ViewBag.WarehouseCapacity = warehouseCapacity;
        ViewBag.TotalUsers = totalUsers;
        ViewBag.RecentOrders = totalOrders;

        _logger.LogInformation(
            "Dashboard loaded successfully with {ProductCount} products, {SupplierCount} suppliers, {WarehouseCount} warehouses, {UserCount} users, and {OrderCount} orders.",
            totalProducts,
            activeSuppliers,
            warehouseCount,
            totalUsers,
            totalOrders
        );
    }
    catch (Exception ex)
    {
        _logger.LogError(ex, "Error loading dashboard metrics.");

        ViewBag.TotalProducts = 0;
        ViewBag.ActiveSuppliers = 0;
        ViewBag.WarehouseCapacity = 0;
        ViewBag.TotalUsers = 0;
        ViewBag.RecentOrders = 0;
    }

    return View();
}

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
