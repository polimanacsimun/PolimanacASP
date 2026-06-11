using InventoryManagement.DAL;
using InventoryManagement.Domain.Models;
using InventoryManagement.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InventoryManagement.Controllers.Api
{
    [ApiController]
    [Route("api/categories")]
    public class CategoryApiController : ControllerBase
    {
        private readonly InventoryManagementDbContext _dbContext;

        public CategoryApiController(InventoryManagementDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        // GET: /api/categories
        // GET: /api/categories?q=office
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoryDto>>> GetAll([FromQuery] string? q)
        {
            IQueryable<Category> query = _dbContext.Categories
                .Include(c => c.Products);

            if (!string.IsNullOrWhiteSpace(q))
            {
                query = query.Where(c =>
                    c.Name.Contains(q) ||
                    c.Description.Contains(q));
            }

            var categories = await query
                .OrderBy(c => c.Name)
                .ToListAsync();

            return Ok(categories.Select(c => c.ToDto()));
        }

        // GET: /api/categories/5
        [HttpGet("{id:int}")]
        public async Task<ActionResult<CategoryDto>> GetById(int id)
        {
            var category = await _dbContext.Categories
                .Include(c => c.Products)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (category == null)
            {
                return NotFound();
            }

            return Ok(category.ToDto());
        }

        // POST: /api/categories
        [HttpPost]
        public async Task<ActionResult<CategoryDto>> Create([FromBody] CategoryRequestDto model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var category = new Category
            {
                Name = model.Name!,
                Description = model.Description ?? string.Empty
            };

            _dbContext.Categories.Add(category);
            await _dbContext.SaveChangesAsync();

            return CreatedAtAction(
                nameof(GetById),
                new { id = category.Id },
                category.ToDto());
        }

        // PUT: /api/categories/5
        [HttpPut("{id:int}")]
        public async Task<ActionResult<CategoryDto>> Update(int id, [FromBody] CategoryRequestDto model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var category = await _dbContext.Categories
                .FirstOrDefaultAsync(c => c.Id == id);

            if (category == null)
            {
                return NotFound();
            }

            category.Name = model.Name!;
            category.Description = model.Description ?? string.Empty;

            await _dbContext.SaveChangesAsync();

            return Ok(category.ToDto());
        }

        // DELETE: /api/categories/5
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var category = await _dbContext.Categories
                .Include(c => c.Products)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (category == null)
            {
                return NotFound();
            }

            var isUsedByProducts = category.Products.Any();

            if (isUsedByProducts)
            {
                return Conflict(new
                {
                    message = "Category cannot be deleted because it is used by one or more products."
                });
            }

            _dbContext.Categories.Remove(category);
            await _dbContext.SaveChangesAsync();

            return NoContent();
        }
    }
}