using InventoryManagement.DAL.Repositories;
using InventoryManagement.Domain.Models;
using InventoryManagement.ViewModels.Search;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InventoryManagement.Controllers
{
    [AllowAnonymous]
    public class GlobalSearchController : Controller
    {
        private const int ResultsPerModule = 8;

        private readonly ProductEfRepository _productRepository;
        private readonly SupplierEfRepository _supplierRepository;
        private readonly CategoryEfRepository _categoryRepository;
        private readonly WarehouseEfRepository _warehouseRepository;
        private readonly UserEfRepository _userRepository;
        private readonly OrderEfRepository _orderRepository;
        private readonly InventoryItemEfRepository _inventoryItemRepository;
        private readonly OrderItemEfRepository _orderItemRepository;

        public GlobalSearchController(
            ProductEfRepository productRepository,
            SupplierEfRepository supplierRepository,
            CategoryEfRepository categoryRepository,
            WarehouseEfRepository warehouseRepository,
            UserEfRepository userRepository,
            OrderEfRepository orderRepository,
            InventoryItemEfRepository inventoryItemRepository,
            OrderItemEfRepository orderItemRepository)
        {
            _productRepository = productRepository;
            _supplierRepository = supplierRepository;
            _categoryRepository = categoryRepository;
            _warehouseRepository = warehouseRepository;
            _userRepository = userRepository;
            _orderRepository = orderRepository;
            _inventoryItemRepository = inventoryItemRepository;
            _orderItemRepository = orderItemRepository;
        }

        [HttpGet]
        [Route("/search")]
        public IActionResult Index(string? q)
        {
            var model = new GlobalSearchViewModel
            {
                Query = q?.Trim() ?? string.Empty
            };

            if (!model.HasQuery)
            {
                return View(model);
            }

            model.Results.AddRange(SearchProducts(model.Query));
            model.Results.AddRange(SearchSuppliers(model.Query));
            model.Results.AddRange(SearchCategories(model.Query));
            model.Results.AddRange(SearchWarehouses(model.Query));
            model.Results.AddRange(SearchUsers(model.Query));
            model.Results.AddRange(SearchOrders(model.Query));
            model.Results.AddRange(SearchInventoryItems(model.Query));
            model.Results.AddRange(SearchOrderItems(model.Query));

            return View(model);
        }

        private IEnumerable<GlobalSearchResultViewModel> SearchProducts(string query)
        {
            return _productRepository.Search(query, ResultsPerModule)
                .Select(product => new GlobalSearchResultViewModel
                {
                    Module = "Products",
                    Title = product.Name,
                    Description = $"{product.Category?.Name ?? "No category"} | {product.Supplier?.Name ?? "No supplier"} | {product.Price:C}",
                    Url = $"/catalog/{product.Id}"
                });
        }

        private IEnumerable<GlobalSearchResultViewModel> SearchSuppliers(string query)
        {
            return _supplierRepository.Search(query, ResultsPerModule)
                .Select(supplier => new GlobalSearchResultViewModel
                {
                    Module = "Suppliers",
                    Title = supplier.Name,
                    Description = $"{supplier.ContactPerson} | {supplier.Email} | {(supplier.IsActive ? "Active" : "Inactive")}",
                    Url = $"/vendors/{supplier.Id}"
                });
        }

        private IEnumerable<GlobalSearchResultViewModel> SearchCategories(string query)
        {
            return _categoryRepository.Search(query, ResultsPerModule)
                .Select(category => new GlobalSearchResultViewModel
                {
                    Module = "Categories",
                    Title = category.Name,
                    Description = string.IsNullOrWhiteSpace(category.Description) ? "Product category" : category.Description,
                    Url = $"/categories/{category.Id}"
                });
        }

        private IEnumerable<GlobalSearchResultViewModel> SearchWarehouses(string query)
        {
            return _warehouseRepository.Search(query, ResultsPerModule)
                .Select(warehouse => new GlobalSearchResultViewModel
                {
                    Module = "Warehouses",
                    Title = warehouse.Name,
                    Description = $"{warehouse.Manager} | {warehouse.Type} | Capacity {warehouse.Capacity}",
                    Url = $"/storage/{warehouse.Id}"
                });
        }

        private IEnumerable<GlobalSearchResultViewModel> SearchUsers(string query)
        {
            return _userRepository.Search(query, ResultsPerModule)
                .Select(user => new GlobalSearchResultViewModel
                {
                    Module = "Users",
                    Title = $"{user.FirstName} {user.LastName}",
                    Description = $"{user.Email} | {user.Role} | {(user.IsActive ? "Active" : "Inactive")}",
                    Url = $"/users/{user.Id}"
                });
        }

        private IEnumerable<GlobalSearchResultViewModel> SearchOrders(string query)
        {
            return _orderRepository.Search(query, ResultsPerModule)
                .Select(order => new GlobalSearchResultViewModel
                {
                    Module = "Orders",
                    Title = order.OrderNumber,
                    Description = $"{GetUserName(order.User)} | {order.Status} | {order.TotalPrice:C}",
                    Url = $"/orders/{order.Id}/summary"
                });
        }

        private IEnumerable<GlobalSearchResultViewModel> SearchInventoryItems(string query)
        {
            return _inventoryItemRepository.Search(query, ResultsPerModule)
                .Select(item => new GlobalSearchResultViewModel
                {
                    Module = "Inventory",
                    Title = item.Product?.Name ?? $"Inventory item #{item.Id}",
                    Description = $"{item.Warehouse?.Name ?? "No warehouse"} | Shelf {item.ShelfLocation} | Qty {item.QuantityInStock}",
                    Url = $"/inventory/{item.Id}"
                });
        }

        private IEnumerable<GlobalSearchResultViewModel> SearchOrderItems(string query)
        {
            return _orderItemRepository.Search(query, ResultsPerModule)
                .Select(item => new GlobalSearchResultViewModel
                {
                    Module = "Order Items",
                    Title = item.Product?.Name ?? $"Order item #{item.Id}",
                    Description = $"{item.Order?.OrderNumber ?? "No order"} | Qty {item.Quantity} | {item.TotalPrice:C}",
                    Url = $"/order-items/{item.Id}"
                });
        }

        private static string GetUserName(User? user)
        {
            if (user == null)
            {
                return "No user";
            }

            return $"{user.FirstName} {user.LastName}";
        }
    }
}
