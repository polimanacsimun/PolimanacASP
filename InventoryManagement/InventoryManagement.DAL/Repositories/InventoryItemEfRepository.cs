using InventoryManagement.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace InventoryManagement.DAL.Repositories
{
    /// <summary>
    /// Entity Framework repository for InventoryItem CRUD operations and search.
    /// Manages inventory item records in warehouse locations.
    /// </summary>
    public class InventoryItemEfRepository
    {
        private readonly InventoryManagementDbContext _context;

        public InventoryItemEfRepository(InventoryManagementDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Gets all inventory items from the database.
        /// Includes related Product and Warehouse entities.
        /// Returns no-tracking query for read-only operations.
        /// </summary>
        /// <returns>List of all inventory items ordered by Product name, then Warehouse name.</returns>
        public List<InventoryItem> GetAll()
        {
            return _context.InventoryItems
                .Include(ii => ii.Product)
                .Include(ii => ii.Warehouse)
                .AsNoTracking()
                .OrderBy(ii => ii.Product!.Name)
                .ThenBy(ii => ii.Warehouse!.Name)
                .ToList();
        }

        /// <summary>
        /// Gets a single inventory item by ID.
        /// Includes related Product and Warehouse entities.
        /// </summary>
        /// <param name="id">The inventory item ID.</param>
        /// <returns>The inventory item if found; otherwise null.</returns>
        public InventoryItem? GetById(int id)
        {
            return _context.InventoryItems
                .Include(ii => ii.Product)
                .Include(ii => ii.Warehouse)
                .FirstOrDefault(ii => ii.Id == id);
        }

        /// <summary>
        /// Adds a new inventory item to the database.
        /// </summary>
        /// <param name="inventoryItem">The inventory item to add.</param>
        public void Add(InventoryItem inventoryItem)
        {
            _context.InventoryItems.Add(inventoryItem);
            _context.SaveChanges();
        }

        /// <summary>
        /// Updates an existing inventory item in the database.
        /// </summary>
        /// <param name="inventoryItem">The inventory item with updated values.</param>
        public void Update(InventoryItem inventoryItem)
        {
            _context.InventoryItems.Update(inventoryItem);
            _context.SaveChanges();
        }

        /// <summary>
        /// Deletes an inventory item from the database.
        /// Returns false if the item does not exist.
        /// </summary>
        /// <param name="id">The ID of the inventory item to delete.</param>
        /// <returns>True if deletion succeeded; false if item not found.</returns>
        public bool Delete(int id)
        {
            if (!Exists(id))
            {
                return false;
            }

            var inventoryItem = _context.InventoryItems.Find(id);
            if (inventoryItem != null)
            {
                _context.InventoryItems.Remove(inventoryItem);
                _context.SaveChanges();
                return true;
            }

            return false;
        }

        /// <summary>
        /// Checks if an inventory item exists by ID.
        /// </summary>
        /// <param name="id">The inventory item ID.</param>
        /// <returns>True if the item exists; otherwise false.</returns>
        public bool Exists(int id)
        {
            return _context.InventoryItems.Any(ii => ii.Id == id);
        }

        /// <summary>
        /// Checks if an inventory item can be deleted.
        /// Currently, inventory items can always be deleted (no dependent entities).
        /// </summary>
        /// <param name="id">The inventory item ID.</param>
        /// <returns>True if the item exists and can be deleted; otherwise false.</returns>
        public bool CanDelete(int id)
        {
            return Exists(id);
        }

        /// <summary>
        /// Searches for inventory items by term.
        /// If term is empty, returns the first maxResults items ordered by Product name then Warehouse name.
        /// If term has value, searches by Product name, Warehouse name, and Shelf location (case-insensitive).
        /// Includes related Product and Warehouse entities.
        /// </summary>
        /// <param name="term">The search term (optional).</param>
        /// <param name="maxResults">The maximum number of results to return (default 50).</param>
        /// <returns>List of inventory items matching the search criteria.</returns>
        public List<InventoryItem> Search(string? term, int maxResults = 50)
        {
            IQueryable<InventoryItem> query = _context.InventoryItems
                .Include(ii => ii.Product)
                .Include(ii => ii.Warehouse);

            if (string.IsNullOrWhiteSpace(term))
            {
                // No search term: return first maxResults ordered by Product name, then Warehouse name
                return query
                    .OrderBy(ii => ii.Product!.Name)
                    .ThenBy(ii => ii.Warehouse!.Name)
                    .Take(maxResults)
                    .ToList();
            }

            // Search term provided: filter by Product name, Warehouse name, or Shelf location
            var searchTerm = term.ToLower();
            return query
                .Where(ii =>
                    ii.Product!.Name.ToLower().Contains(searchTerm) ||
                    ii.Warehouse!.Name.ToLower().Contains(searchTerm) ||
                    ii.ShelfLocation.ToLower().Contains(searchTerm))
                .OrderBy(ii => ii.Product!.Name)
                .ThenBy(ii => ii.Warehouse!.Name)
                .Take(maxResults)
                .ToList();
        }
    }
}
