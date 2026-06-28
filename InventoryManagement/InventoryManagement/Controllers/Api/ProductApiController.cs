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
    [Route("api/products")]
    [Authorize(Roles = "Admin,Manager")]
    public class ProductApiController : ControllerBase
    {
        private readonly InventoryManagementDbContext _dbContext;

        public ProductApiController(InventoryManagementDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        // GET: /api/products
        // GET: /api/products?q=laptop
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetAll([FromQuery] string? q)
        {
            IQueryable<Product> query = _dbContext.Products
                .AsNoTracking()
                .Include(p => p.Category)
                .Include(p => p.Supplier);

            if (!string.IsNullOrWhiteSpace(q))
            {
                q = q.Trim();

                var hasProductType = Enum.TryParse<ProductType>(
                    q,
                    ignoreCase: true,
                    out var parsedProductType);

                query = query.Where(p =>
                    (p.Name ?? string.Empty).Contains(q) ||
                    (p.Description ?? string.Empty).Contains(q) ||
                    (p.UnitOfMeasure ?? string.Empty).Contains(q) ||
                    (p.Category != null && (p.Category.Name ?? string.Empty).Contains(q)) ||
                    (p.Supplier != null && (p.Supplier.Name ?? string.Empty).Contains(q)) ||
                    (hasProductType && p.Type == parsedProductType));
            }

            var products = await query
                .OrderBy(p => p.Name)
                .ToListAsync();

            return Ok(products.Select(p => p.ToDto()));
        }

        // GET: /api/products/5
        [HttpGet("{id:int}")]
        [AllowAnonymous]
        public async Task<ActionResult<ProductDto>> GetById(int id)
        {
            var product = await _dbContext.Products
                .AsNoTracking()
                .Include(p => p.Category)
                .Include(p => p.Supplier)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (product == null)
            {
                return NotFound();
            }

            return Ok(product.ToDto());
        }

        // POST: /api/products
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ProductDto>> Create([FromBody] ProductRequestDto model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var categoryExists = await _dbContext.Categories
                .AnyAsync(c => c.Id == model.CategoryId!.Value);

            if (!categoryExists)
            {
                ModelState.AddModelError(nameof(model.CategoryId), "Selected category does not exist.");
            }

            var supplierExists = await _dbContext.Suppliers
                .AnyAsync(s => s.Id == model.SupplierId!.Value);

            if (!supplierExists)
            {
                ModelState.AddModelError(nameof(model.SupplierId), "Selected supplier does not exist.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var product = new Product
            {
                Name = model.Name!,
                Description = model.Description ?? string.Empty,
                Price = model.Price!.Value,
                UnitOfMeasure = model.UnitOfMeasure!,
                MinimumStock = model.MinimumStock ?? 0,
                CreatedAt = model.CreatedAt ?? DateTime.UtcNow,
                IsActive = model.IsActive ?? true,
                Type = model.Type!.Value,
                CategoryId = model.CategoryId!.Value,
                SupplierId = model.SupplierId!.Value
            };

            _dbContext.Products.Add(product);
            await _dbContext.SaveChangesAsync();

            var createdProduct = await _dbContext.Products
                .AsNoTracking()
                .Include(p => p.Category)
                .Include(p => p.Supplier)
                .FirstAsync(p => p.Id == product.Id);

            return CreatedAtAction(
                nameof(GetById),
                new { id = product.Id },
                createdProduct.ToDto());
        }

        // PUT: /api/products/5
        [HttpPut("{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ProductDto>> Update(int id, [FromBody] ProductRequestDto model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var product = await _dbContext.Products
                .FirstOrDefaultAsync(p => p.Id == id);

            if (product == null)
            {
                return NotFound();
            }

            var categoryExists = await _dbContext.Categories
                .AnyAsync(c => c.Id == model.CategoryId!.Value);

            if (!categoryExists)
            {
                ModelState.AddModelError(nameof(model.CategoryId), "Selected category does not exist.");
            }

            var supplierExists = await _dbContext.Suppliers
                .AnyAsync(s => s.Id == model.SupplierId!.Value);

            if (!supplierExists)
            {
                ModelState.AddModelError(nameof(model.SupplierId), "Selected supplier does not exist.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            product.Name = model.Name!;
            product.Description = model.Description ?? string.Empty;
            product.Price = model.Price!.Value;
            product.UnitOfMeasure = model.UnitOfMeasure!;
            product.MinimumStock = model.MinimumStock ?? 0;
            product.CreatedAt = model.CreatedAt ?? product.CreatedAt;
            product.IsActive = model.IsActive ?? product.IsActive;
            product.Type = model.Type!.Value;
            product.CategoryId = model.CategoryId!.Value;
            product.SupplierId = model.SupplierId!.Value;

            await _dbContext.SaveChangesAsync();

            var updatedProduct = await _dbContext.Products
                .AsNoTracking()
                .Include(p => p.Category)
                .Include(p => p.Supplier)
                .FirstAsync(p => p.Id == product.Id);

            return Ok(updatedProduct.ToDto());
        }

        // DELETE: /api/products/5
        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var product = await _dbContext.Products
                .Include(p => p.OrderItems)
                .Include(p => p.InventoryItems)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (product == null)
            {
                return NotFound();
            }

            if (product.OrderItems.Any())
            {
                return Conflict(new
                {
                    message = "Product cannot be deleted because it is used by one or more order items."
                });
            }

            if (product.InventoryItems.Any())
            {
                return Conflict(new
                {
                    message = "Product cannot be deleted because it is used by one or more inventory items."
                });
            }

            _dbContext.Products.Remove(product);
            await _dbContext.SaveChangesAsync();

            return NoContent();
        }
    }
}
