using InventoryManagement.DAL;
using InventoryManagement.Domain.Models;
using InventoryManagement.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace InventoryManagement.Controllers.Api
{
    [ApiController]
    [Route("api/suppliers")]
    [Authorize(Roles = "Admin,Manager")]
    public class SupplierApiController : ControllerBase
    {
        private readonly InventoryManagementDbContext _dbContext;

        public SupplierApiController(InventoryManagementDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        // GET: /api/suppliers
        // GET: /api/suppliers?q=alpha
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<SupplierDto>>> GetAll([FromQuery] string? q)
        {
            IQueryable<Supplier> query = _dbContext.Suppliers
                .AsNoTracking()
                .Include(s => s.Products);

            if (!string.IsNullOrWhiteSpace(q))
            {
                q = q.Trim();

                query = query.Where(s =>
                    (s.Name ?? string.Empty).Contains(q) ||
                    (s.Address ?? string.Empty).Contains(q) ||
                    (s.Phone ?? string.Empty).Contains(q) ||
                    (s.Email ?? string.Empty).Contains(q) ||
                    (s.ContactPerson ?? string.Empty).Contains(q));
            }

            var suppliers = await query
                .OrderBy(s => s.Name)
                .ToListAsync();

            return Ok(suppliers.Select(s => s.ToDto()));
        }

        // GET: /api/suppliers/5
        [HttpGet("{id:int}")]
        [AllowAnonymous]
        public async Task<ActionResult<SupplierDto>> GetById(int id)
        {
            var supplier = await _dbContext.Suppliers
                .AsNoTracking()
                .Include(s => s.Products)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (supplier == null)
            {
                return NotFound();
            }

            return Ok(supplier.ToDto());
        }

        // POST: /api/suppliers
        [HttpPost]
        public async Task<ActionResult<SupplierDto>> Create([FromBody] SupplierRequestDto model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var supplier = new Supplier
            {
                Name = model.Name!,
                Address = model.Address ?? string.Empty,
                Phone = model.Phone ?? string.Empty,
                Email = model.Email ?? string.Empty,
                ContactPerson = model.ContactPerson ?? string.Empty,
                RegistrationDate = model.RegistrationDate ?? DateTime.UtcNow,
                IsActive = model.IsActive ?? true
            };

            _dbContext.Suppliers.Add(supplier);
            await _dbContext.SaveChangesAsync();

            return CreatedAtAction(
                nameof(GetById),
                new { id = supplier.Id },
                supplier.ToDto());
        }

        // PUT: /api/suppliers/5
        [HttpPut("{id:int}")]
        public async Task<ActionResult<SupplierDto>> Update(int id, [FromBody] SupplierRequestDto model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var supplier = await _dbContext.Suppliers
                .FirstOrDefaultAsync(s => s.Id == id);

            if (supplier == null)
            {
                return NotFound();
            }

            supplier.Name = model.Name!;
            supplier.Address = model.Address ?? string.Empty;
            supplier.Phone = model.Phone ?? string.Empty;
            supplier.Email = model.Email ?? string.Empty;
            supplier.ContactPerson = model.ContactPerson ?? string.Empty;
            supplier.RegistrationDate = model.RegistrationDate ?? supplier.RegistrationDate;
            supplier.IsActive = model.IsActive ?? supplier.IsActive;

            await _dbContext.SaveChangesAsync();

            return Ok(supplier.ToDto());
        }

        // DELETE: /api/suppliers/5
        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var supplier = await _dbContext.Suppliers
                .Include(s => s.Products)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (supplier == null)
            {
                return NotFound();
            }

            if (supplier.Products.Any())
            {
                return Conflict(new
                {
                    message = "Supplier cannot be deleted because it is used by one or more products."
                });
            }

            _dbContext.Suppliers.Remove(supplier);
            await _dbContext.SaveChangesAsync();

            return NoContent();
        }
    }
}