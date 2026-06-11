using InventoryManagement.DAL;
using InventoryManagement.Domain.Enums;
using InventoryManagement.Domain.Models;
using InventoryManagement.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InventoryManagement.Controllers.Api
{
    [ApiController]
    [Route("api/orders")]
    public class OrderApiController : ControllerBase
    {
        private readonly InventoryManagementDbContext _dbContext;

        public OrderApiController(InventoryManagementDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        // GET: /api/orders
        // GET: /api/orders?q=ORD
        // GET: /api/orders?q=Pending
        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderDto>>> GetAll([FromQuery] string? q)
        {
            IQueryable<Order> query = _dbContext.Orders
                .AsNoTracking()
                .Include(o => o.User)
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Product);

            if (!string.IsNullOrWhiteSpace(q))
            {
                q = q.Trim();

                var hasOrderStatus = Enum.TryParse<OrderStatus>(
                    q,
                    ignoreCase: true,
                    out var parsedOrderStatus);

                query = query.Where(o =>
                    (o.OrderNumber ?? string.Empty).Contains(q) ||
                    (o.Note ?? string.Empty).Contains(q) ||
                    (o.User != null && (
                        (o.User.FirstName ?? string.Empty).Contains(q) ||
                        (o.User.LastName ?? string.Empty).Contains(q) ||
                        (o.User.Email ?? string.Empty).Contains(q))) ||
                    (hasOrderStatus && o.Status == parsedOrderStatus));
            }

            var orders = await query
                .OrderByDescending(o => o.OrderDate)
                .ToListAsync();

            return Ok(orders.Select(o => o.ToDto()));
        }

        // GET: /api/orders/5
        [HttpGet("{id:int}")]
        public async Task<ActionResult<OrderDto>> GetById(int id)
        {
            var order = await _dbContext.Orders
                .AsNoTracking()
                .Include(o => o.User)
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Product)
                .FirstOrDefaultAsync(o => o.Id == id);

            if (order == null)
            {
                return NotFound();
            }

            return Ok(order.ToDto());
        }

        // POST: /api/orders
        [HttpPost]
        public async Task<ActionResult<OrderDto>> Create([FromBody] OrderRequestDto model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userExists = await _dbContext.Users
                .AnyAsync(u => u.Id == model.UserId!.Value);

            if (!userExists)
            {
                ModelState.AddModelError(nameof(model.UserId), "Selected user does not exist.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var order = new Order
            {
                OrderNumber = model.OrderNumber!,
                OrderDate = model.OrderDate ?? DateTime.UtcNow,
                TotalPrice = model.TotalPrice!.Value,
                Status = model.Status!.Value,
                DeliveryDate = model.DeliveryDate,
                Note = model.Note ?? string.Empty,
                UserId = model.UserId!.Value
            };

            _dbContext.Orders.Add(order);
            await _dbContext.SaveChangesAsync();

            var createdOrder = await _dbContext.Orders
                .AsNoTracking()
                .Include(o => o.User)
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Product)
                .FirstAsync(o => o.Id == order.Id);

            return CreatedAtAction(
                nameof(GetById),
                new { id = order.Id },
                createdOrder.ToDto());
        }

        // PUT: /api/orders/5
        [HttpPut("{id:int}")]
        public async Task<ActionResult<OrderDto>> Update(int id, [FromBody] OrderRequestDto model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var order = await _dbContext.Orders
                .FirstOrDefaultAsync(o => o.Id == id);

            if (order == null)
            {
                return NotFound();
            }

            var userExists = await _dbContext.Users
                .AnyAsync(u => u.Id == model.UserId!.Value);

            if (!userExists)
            {
                ModelState.AddModelError(nameof(model.UserId), "Selected user does not exist.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            order.OrderNumber = model.OrderNumber!;
            order.OrderDate = model.OrderDate ?? order.OrderDate;
            order.TotalPrice = model.TotalPrice!.Value;
            order.Status = model.Status!.Value;
            order.DeliveryDate = model.DeliveryDate;
            order.Note = model.Note ?? string.Empty;
            order.UserId = model.UserId!.Value;

            await _dbContext.SaveChangesAsync();

            var updatedOrder = await _dbContext.Orders
                .AsNoTracking()
                .Include(o => o.User)
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Product)
                .FirstAsync(o => o.Id == order.Id);

            return Ok(updatedOrder.ToDto());
        }

        // DELETE: /api/orders/5
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var order = await _dbContext.Orders
                .Include(o => o.OrderItems)
                .FirstOrDefaultAsync(o => o.Id == id);

            if (order == null)
            {
                return NotFound();
            }

            if (order.OrderItems.Any())
            {
                return Conflict(new
                {
                    message = "Order cannot be deleted because it contains one or more order items."
                });
            }

            _dbContext.Orders.Remove(order);
            await _dbContext.SaveChangesAsync();

            return NoContent();
        }
    }
}