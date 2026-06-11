using InventoryManagement.DAL;
using InventoryManagement.Domain.Models;
using InventoryManagement.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace InventoryManagement.Controllers.Api
{
    [ApiController]
    [Route("api/inventory-items")]
    [Authorize(Roles = "Admin,Manager")]
    public class InventoryItemApiController : ControllerBase
    {
        private readonly InventoryManagementDbContext _dbContext;

        public InventoryItemApiController(InventoryManagementDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        // GET: /api/inventory-items
        // GET: /api/inventory-items?q=laptop
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<InventoryItemDto>>> GetAll([FromQuery] string? q)
        {
            IQueryable<InventoryItem> query = _dbContext.InventoryItems
                .AsNoTracking()
                .Include(i => i.Product)
                .Include(i => i.Warehouse);

            if (!string.IsNullOrWhiteSpace(q))
            {
                q = q.Trim();

                query = query.Where(i =>
                    (i.ShelfLocation ?? string.Empty).Contains(q) ||
                    (i.Product != null && (i.Product.Name ?? string.Empty).Contains(q)) ||
                    (i.Warehouse != null && (i.Warehouse.Name ?? string.Empty).Contains(q)));
            }

            var inventoryItems = await query
                .OrderBy(i => i.Warehouse.Name)
                .ThenBy(i => i.Product.Name)
                .ToListAsync();

            return Ok(inventoryItems.Select(i => i.ToDto()));
        }

        // GET: /api/inventory-items/5
        [HttpGet("{id:int}")]
        [AllowAnonymous]
        public async Task<ActionResult<InventoryItemDto>> GetById(int id)
        {
            var inventoryItem = await _dbContext.InventoryItems
                .AsNoTracking()
                .Include(i => i.Product)
                .Include(i => i.Warehouse)
                .FirstOrDefaultAsync(i => i.Id == id);

            if (inventoryItem == null)
            {
                return NotFound();
            }

            return Ok(inventoryItem.ToDto());
        }

        // POST: /api/inventory-items
        [HttpPost]
        public async Task<ActionResult<InventoryItemDto>> Create([FromBody] InventoryItemRequestDto model)
        {
            ValidateQuantities(model);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var productExists = await _dbContext.Products
                .AnyAsync(p => p.Id == model.ProductId!.Value);

            if (!productExists)
            {
                ModelState.AddModelError(nameof(model.ProductId), "Selected product does not exist.");
            }

            var warehouseExists = await _dbContext.Warehouses
                .AnyAsync(w => w.Id == model.WarehouseId!.Value);

            if (!warehouseExists)
            {
                ModelState.AddModelError(nameof(model.WarehouseId), "Selected warehouse does not exist.");
            }

            var duplicateExists = await _dbContext.InventoryItems
                .AnyAsync(i =>
                    i.ProductId == model.ProductId!.Value &&
                    i.WarehouseId == model.WarehouseId!.Value);

            if (duplicateExists)
            {
                ModelState.AddModelError(
                    string.Empty,
                    "This product already exists in the selected warehouse.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var inventoryItem = new InventoryItem
            {
                QuantityInStock = model.QuantityInStock!.Value,
                MinimumQuantity = model.MinimumQuantity!.Value,
                MaximumQuantity = model.MaximumQuantity!.Value,
                ShelfLocation = model.ShelfLocation ?? string.Empty,
                LastCheckedAt = model.LastCheckedAt ?? DateTime.UtcNow,
                ProductId = model.ProductId!.Value,
                WarehouseId = model.WarehouseId!.Value
            };

            _dbContext.InventoryItems.Add(inventoryItem);
            await _dbContext.SaveChangesAsync();

            var createdItem = await _dbContext.InventoryItems
                .AsNoTracking()
                .Include(i => i.Product)
                .Include(i => i.Warehouse)
                .FirstAsync(i => i.Id == inventoryItem.Id);

            return CreatedAtAction(
                nameof(GetById),
                new { id = inventoryItem.Id },
                createdItem.ToDto());
        }

        // PUT: /api/inventory-items/5
        [HttpPut("{id:int}")]
        public async Task<ActionResult<InventoryItemDto>> Update(int id, [FromBody] InventoryItemRequestDto model)
        {
            ValidateQuantities(model);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var inventoryItem = await _dbContext.InventoryItems
                .FirstOrDefaultAsync(i => i.Id == id);

            if (inventoryItem == null)
            {
                return NotFound();
            }

            var productExists = await _dbContext.Products
                .AnyAsync(p => p.Id == model.ProductId!.Value);

            if (!productExists)
            {
                ModelState.AddModelError(nameof(model.ProductId), "Selected product does not exist.");
            }

            var warehouseExists = await _dbContext.Warehouses
                .AnyAsync(w => w.Id == model.WarehouseId!.Value);

            if (!warehouseExists)
            {
                ModelState.AddModelError(nameof(model.WarehouseId), "Selected warehouse does not exist.");
            }

            var duplicateExists = await _dbContext.InventoryItems
                .AnyAsync(i =>
                    i.Id != id &&
                    i.ProductId == model.ProductId!.Value &&
                    i.WarehouseId == model.WarehouseId!.Value);

            if (duplicateExists)
            {
                ModelState.AddModelError(
                    string.Empty,
                    "This product already exists in the selected warehouse.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            inventoryItem.QuantityInStock = model.QuantityInStock!.Value;
            inventoryItem.MinimumQuantity = model.MinimumQuantity!.Value;
            inventoryItem.MaximumQuantity = model.MaximumQuantity!.Value;
            inventoryItem.ShelfLocation = model.ShelfLocation ?? string.Empty;
            inventoryItem.LastCheckedAt = model.LastCheckedAt ?? inventoryItem.LastCheckedAt;
            inventoryItem.ProductId = model.ProductId!.Value;
            inventoryItem.WarehouseId = model.WarehouseId!.Value;

            await _dbContext.SaveChangesAsync();

            var updatedItem = await _dbContext.InventoryItems
                .AsNoTracking()
                .Include(i => i.Product)
                .Include(i => i.Warehouse)
                .FirstAsync(i => i.Id == inventoryItem.Id);

            return Ok(updatedItem.ToDto());
        }

        // DELETE: /api/inventory-items/5
        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var inventoryItem = await _dbContext.InventoryItems
                .FirstOrDefaultAsync(i => i.Id == id);

            if (inventoryItem == null)
            {
                return NotFound();
            }

            _dbContext.InventoryItems.Remove(inventoryItem);
            await _dbContext.SaveChangesAsync();

            return NoContent();
        }

        private void ValidateQuantities(InventoryItemRequestDto model)
        {
            if (model.MinimumQuantity.HasValue &&
                model.MaximumQuantity.HasValue &&
                model.MinimumQuantity.Value > model.MaximumQuantity.Value)
            {
                ModelState.AddModelError(
                    nameof(model.MinimumQuantity),
                    "Minimum quantity cannot be greater than maximum quantity.");
            }

            if (model.QuantityInStock.HasValue &&
                model.MaximumQuantity.HasValue &&
                model.QuantityInStock.Value > model.MaximumQuantity.Value)
            {
                ModelState.AddModelError(
                    nameof(model.QuantityInStock),
                    "Quantity in stock cannot be greater than maximum quantity.");
            }
        }
    }
}