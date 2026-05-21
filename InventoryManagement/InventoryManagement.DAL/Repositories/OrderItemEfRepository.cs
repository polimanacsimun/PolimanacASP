using InventoryManagement.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace InventoryManagement.DAL.Repositories
{
    /// <summary>
    /// Entity Framework repository for OrderItem CRUD operations and search.
    /// Manages order line items (products ordered in orders).
    /// </summary>
    public class OrderItemEfRepository
    {
        private readonly InventoryManagementDbContext _context;

        public OrderItemEfRepository(InventoryManagementDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Gets all order items from the database.
        /// Includes related Order and Product entities.
        /// Returns no-tracking query for read-only operations.
        /// Ordered by creation date in descending order.
        /// </summary>
        /// <returns>List of all order items ordered by creation date (newest first).</returns>
        public List<OrderItem> GetAll()
        {
            return _context.OrderItems
                .Include(oi => oi.Order)
                .Include(oi => oi.Product)
                .AsNoTracking()
                .OrderByDescending(oi => oi.CreatedAt)
                .ToList();
        }

        /// <summary>
        /// Gets a single order item by ID.
        /// Includes related Order and Product entities.
        /// </summary>
        /// <param name="id">The order item ID.</param>
        /// <returns>The order item if found; otherwise null.</returns>
        public OrderItem? GetById(int id)
        {
            return _context.OrderItems
                .Include(oi => oi.Order)
                .Include(oi => oi.Product)
                .FirstOrDefault(oi => oi.Id == id);
        }

        /// <summary>
        /// Adds a new order item to the database.
        /// </summary>
        /// <param name="orderItem">The order item to add.</param>
        public void Add(OrderItem orderItem)
        {
            _context.OrderItems.Add(orderItem);
            _context.SaveChanges();
        }

        /// <summary>
        /// Updates an existing order item in the database.
        /// </summary>
        /// <param name="orderItem">The order item with updated values.</param>
        public void Update(OrderItem orderItem)
        {
            _context.OrderItems.Update(orderItem);
            _context.SaveChanges();
        }

        /// <summary>
        /// Deletes an order item from the database.
        /// Returns false if the item does not exist.
        /// </summary>
        /// <param name="id">The ID of the order item to delete.</param>
        /// <returns>True if deletion succeeded; false if item not found.</returns>
        public bool Delete(int id)
        {
            if (!Exists(id))
            {
                return false;
            }

            var orderItem = _context.OrderItems.Find(id);
            if (orderItem != null)
            {
                _context.OrderItems.Remove(orderItem);
                _context.SaveChanges();
                return true;
            }

            return false;
        }

        /// <summary>
        /// Checks if an order item exists by ID.
        /// </summary>
        /// <param name="id">The order item ID.</param>
        /// <returns>True if the item exists; otherwise false.</returns>
        public bool Exists(int id)
        {
            return _context.OrderItems.Any(oi => oi.Id == id);
        }

        /// <summary>
        /// Checks if an order item can be deleted.
        /// Currently, order items can always be deleted (no dependent entities).
        /// </summary>
        /// <param name="id">The order item ID.</param>
        /// <returns>True if the item exists and can be deleted; otherwise false.</returns>
        public bool CanDelete(int id)
        {
            return Exists(id);
        }

        /// <summary>
        /// Searches for order items by term.
        /// If term is empty, returns the first maxResults items ordered by creation date descending.
        /// If term has value, searches by Order number and Product name (case-insensitive).
        /// Includes related Order and Product entities.
        /// </summary>
        /// <param name="term">The search term (optional).</param>
        /// <param name="maxResults">The maximum number of results to return (default 50).</param>
        /// <returns>List of order items matching the search criteria.</returns>
        public List<OrderItem> Search(string? term, int maxResults = 50)
        {
            IQueryable<OrderItem> query = _context.OrderItems
                .Include(oi => oi.Order)
                .Include(oi => oi.Product);

            if (string.IsNullOrWhiteSpace(term))
            {
                // No search term: return first maxResults ordered by creation date descending
                return query
                    .OrderByDescending(oi => oi.CreatedAt)
                    .Take(maxResults)
                    .ToList();
            }

            // Search term provided: filter by Order number or Product name
            var searchTerm = term.ToLower();
            return query
                .Where(oi =>
                    oi.Order!.OrderNumber.ToLower().Contains(searchTerm) ||
                    oi.Product!.Name.ToLower().Contains(searchTerm))
                .OrderByDescending(oi => oi.CreatedAt)
                .Take(maxResults)
                .ToList();
        }
    }
}
