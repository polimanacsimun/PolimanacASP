using InventoryManagement.DAL;
using InventoryManagement.Domain.Enums;
using InventoryManagement.Domain.Models;
using InventoryManagement.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace InventoryManagement.Controllers.Api
{
    [ApiController]
    [Route("api/warehouses")]
    [Authorize(Roles = "Admin,Manager")]
    public class WarehouseApiController : ControllerBase
    {
        private readonly InventoryManagementDbContext _dbContext;

        public WarehouseApiController(InventoryManagementDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        // GET: /api/warehouses
        // GET: /api/warehouses?q=main
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<WarehouseDto>>> GetAll([FromQuery] string? q)
        {
            IQueryable<Warehouse> query = _dbContext.Warehouses
                .AsNoTracking()
                .Include(w => w.InventoryItems);

            if (!string.IsNullOrWhiteSpace(q))
            {
                q = q.Trim();

                var hasWarehouseType = Enum.TryParse<WarehouseType>(
                    q,
                    ignoreCase: true,
                    out var parsedWarehouseType);

                query = query.Where(w =>
                    (w.Name ?? string.Empty).Contains(q) ||
                    (w.Address ?? string.Empty).Contains(q) ||
                    (w.Phone ?? string.Empty).Contains(q) ||
                    (w.Email ?? string.Empty).Contains(q) ||
                    (w.Manager ?? string.Empty).Contains(q) ||
                    (hasWarehouseType && w.Type == parsedWarehouseType));
            }

            var warehouses = await query
                .OrderBy(w => w.Name)
                .ToListAsync();

            return Ok(warehouses.Select(w => w.ToDto()));
        }

        // GET: /api/warehouses/5
        [HttpGet("{id:int}")]
        [AllowAnonymous]
        public async Task<ActionResult<WarehouseDto>> GetById(int id)
        {
            var warehouse = await _dbContext.Warehouses
                .AsNoTracking()
                .Include(w => w.InventoryItems)
                    .ThenInclude(ii => ii.Product)
                .FirstOrDefaultAsync(w => w.Id == id);

            if (warehouse == null)
            {
                return NotFound();
            }

            return Ok(warehouse.ToDto());
        }

        // POST: /api/warehouses
        [HttpPost]
        public async Task<ActionResult<WarehouseDto>> Create([FromBody] WarehouseRequestDto model)
        {
            if (model.Capacity == null)
            {
                ModelState.AddModelError(nameof(model.Capacity), "Capacity is required.");
            }

            if (model.Type == null)
            {
                ModelState.AddModelError(nameof(model.Type), "Warehouse type is required.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var warehouse = new Warehouse
            {
                Name = model.Name!,
                Address = model.Address ?? string.Empty,
                Capacity = model.Capacity!.Value,
                Phone = model.Phone ?? string.Empty,
                Email = model.Email ?? string.Empty,
                Manager = model.Manager ?? string.Empty,
                OpeningDate = model.OpeningDate ?? DateTime.UtcNow,
                IsActive = model.IsActive ?? true,
                Type = model.Type!.Value
            };

            _dbContext.Warehouses.Add(warehouse);
            await _dbContext.SaveChangesAsync();

            return CreatedAtAction(
                nameof(GetById),
                new { id = warehouse.Id },
                warehouse.ToDto());
        }

        // PUT: /api/warehouses/5
        [HttpPut("{id:int}")]
        public async Task<ActionResult<WarehouseDto>> Update(int id, [FromBody] WarehouseRequestDto model)
        {
            if (model.Capacity == null)
            {
                ModelState.AddModelError(nameof(model.Capacity), "Capacity is required.");
            }

            if (model.Type == null)
            {
                ModelState.AddModelError(nameof(model.Type), "Warehouse type is required.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var warehouse = await _dbContext.Warehouses
                .FirstOrDefaultAsync(w => w.Id == id);

            if (warehouse == null)
            {
                return NotFound();
            }

            warehouse.Name = model.Name!;
            warehouse.Address = model.Address ?? string.Empty;
            warehouse.Capacity = model.Capacity!.Value;
            warehouse.Phone = model.Phone ?? string.Empty;
            warehouse.Email = model.Email ?? string.Empty;
            warehouse.Manager = model.Manager ?? string.Empty;
            warehouse.OpeningDate = model.OpeningDate ?? warehouse.OpeningDate;
            warehouse.IsActive = model.IsActive ?? warehouse.IsActive;
            warehouse.Type = model.Type!.Value;

            await _dbContext.SaveChangesAsync();

            return Ok(warehouse.ToDto());
        }

        // DELETE: /api/warehouses/5
        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var warehouse = await _dbContext.Warehouses
                .Include(w => w.InventoryItems)
                .FirstOrDefaultAsync(w => w.Id == id);

            if (warehouse == null)
            {
                return NotFound();
            }

            if (warehouse.InventoryItems.Any())
            {
                return Conflict(new
                {
                    message = "Warehouse cannot be deleted because it is used by one or more inventory items."
                });
            }

            _dbContext.Warehouses.Remove(warehouse);
            await _dbContext.SaveChangesAsync();

            return NoContent();
        }
    }
}