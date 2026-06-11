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
    [Route("api/users")]
    [Authorize(Roles = "Admin,Manager")]
    public class UserApiController : ControllerBase
    {
        private readonly InventoryManagementDbContext _dbContext;

        public UserApiController(InventoryManagementDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        // GET: /api/users
        // GET: /api/users?q=toni
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetAll([FromQuery] string? q)
        {
            IQueryable<User> query = _dbContext.BusinessUsers
                .AsNoTracking()
                .Include(u => u.Orders);

            if (!string.IsNullOrWhiteSpace(q))
            {
                q = q.Trim();

                var hasUserRole = Enum.TryParse<UserRole>(
                    q,
                    ignoreCase: true,
                    out var parsedUserRole);

                query = query.Where(u =>
                    (u.FirstName ?? string.Empty).Contains(q) ||
                    (u.LastName ?? string.Empty).Contains(q) ||
                    (u.Email ?? string.Empty).Contains(q) ||
                    (hasUserRole && u.Role == parsedUserRole));
            }

            var users = await query
                .OrderBy(u => u.LastName)
                .ThenBy(u => u.FirstName)
                .ToListAsync();

            return Ok(users.Select(u => u.ToDto()));
        }

        // GET: /api/users/5
        [HttpGet("{id:int}")]
        [AllowAnonymous]
        public async Task<ActionResult<UserDto>> GetById(int id)
        {
            var user = await _dbContext.BusinessUsers
                .AsNoTracking()
                .Include(u => u.Orders)
                .FirstOrDefaultAsync(u => u.Id == id);

            if (user == null)
            {
                return NotFound();
            }

            return Ok(user.ToDto());
        }

        // POST: /api/users
        [HttpPost]
        public async Task<ActionResult<UserDto>> Create([FromBody] UserRequestDto model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var emailExists = await _dbContext.BusinessUsers
                .AnyAsync(u => u.Email == model.Email);

            if (emailExists)
            {
                ModelState.AddModelError(nameof(model.Email), "A user with this email already exists.");
                return BadRequest(ModelState);
            }

            var user = new User
            {
                FirstName = model.FirstName!,
                LastName = model.LastName!,
                Email = model.Email!,
                Role = model.Role!.Value,
                RegistrationDate = model.RegistrationDate ?? DateTime.UtcNow,
                IsActive = model.IsActive ?? true
            };

            _dbContext.BusinessUsers.Add(user);
            await _dbContext.SaveChangesAsync();

            return CreatedAtAction(
                nameof(GetById),
                new { id = user.Id },
                user.ToDto());
        }

        // PUT: /api/users/5
        [HttpPut("{id:int}")]
        public async Task<ActionResult<UserDto>> Update(int id, [FromBody] UserRequestDto model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _dbContext.BusinessUsers
                .FirstOrDefaultAsync(u => u.Id == id);

            if (user == null)
            {
                return NotFound();
            }

            var emailExists = await _dbContext.BusinessUsers
                .AnyAsync(u => u.Id != id && u.Email == model.Email);

            if (emailExists)
            {
                ModelState.AddModelError(nameof(model.Email), "A user with this email already exists.");
                return BadRequest(ModelState);
            }

            user.FirstName = model.FirstName!;
            user.LastName = model.LastName!;
            user.Email = model.Email!;
            user.Role = model.Role!.Value;
            user.RegistrationDate = model.RegistrationDate ?? user.RegistrationDate;
            user.IsActive = model.IsActive ?? user.IsActive;

            await _dbContext.SaveChangesAsync();

            return Ok(user.ToDto());
        }

        // DELETE: /api/users/5
        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var user = await _dbContext.BusinessUsers
                .Include(u => u.Orders)
                .FirstOrDefaultAsync(u => u.Id == id);

            if (user == null)
            {
                return NotFound();
            }

            if (user.Orders.Any())
            {
                return Conflict(new
                {
                    message = "User cannot be deleted because they have one or more orders."
                });
            }

            _dbContext.BusinessUsers.Remove(user);
            await _dbContext.SaveChangesAsync();

            return NoContent();
        }
    }
}