using InventoryManagement.DAL;
using InventoryManagement.Domain.Models;
using InventoryManagement.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace InventoryManagement.Controllers.Api
{
    [ApiController]
    [Route("api/order-items")]
    [Authorize(Roles = "Admin,Manager")]
    public class OrderItemApiController : ControllerBase
    {
        private readonly InventoryManagementDbContext _dbContext;

        public OrderItemApiController(InventoryManagementDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        // GET: /api/order-items
        // GET: /api/order-items?q=ORD-2026
        // GET: /api/order-items?q=Business Laptop
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<OrderItemDto>>> GetAll([FromQuery] string? q)
        {
            IQueryable<OrderItem> query = _dbContext.OrderItems
                .AsNoTracking()
                .Include(oi => oi.Order)
                .Include(oi => oi.Product);

            if (!string.IsNullOrWhiteSpace(q))
            {
                q = q.Trim();

                query = query.Where(oi =>
                    (oi.Order != null && (oi.Order.OrderNumber ?? string.Empty).Contains(q)) ||
                    (oi.Product != null && (oi.Product.Name ?? string.Empty).Contains(q)));
            }

            var orderItems = await query
                .OrderByDescending(oi => oi.CreatedAt)
                .ToListAsync();

            return Ok(orderItems.Select(oi => oi.ToDto()));
        }

        // GET: /api/order-items/5
        [HttpGet("{id:int}")]
        [AllowAnonymous]
        public async Task<ActionResult<OrderItemDto>> GetById(int id)
        {
            var orderItem = await _dbContext.OrderItems
                .AsNoTracking()
                .Include(oi => oi.Order)
                .Include(oi => oi.Product)
                .FirstOrDefaultAsync(oi => oi.Id == id);

            if (orderItem == null)
            {
                return NotFound();
            }

            return Ok(orderItem.ToDto());
        }

        // POST: /api/order-items
        [HttpPost]
        public async Task<ActionResult<OrderItemDto>> Create([FromBody] OrderItemRequestDto model)
        {
            ValidateOrderItem(model);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var orderExists = await _dbContext.Orders
                .AnyAsync(o => o.Id == model.OrderId!.Value);

            if (!orderExists)
            {
                ModelState.AddModelError(nameof(model.OrderId), "Selected order does not exist.");
            }

            var productExists = await _dbContext.Products
                .AnyAsync(p => p.Id == model.ProductId!.Value);

            if (!productExists)
            {
                ModelState.AddModelError(nameof(model.ProductId), "Selected product does not exist.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var totalPrice = CalculateTotalPrice(
                model.Quantity!.Value,
                model.UnitPrice!.Value,
                model.Discount ?? 0m);

            var orderItem = new OrderItem
            {
                Quantity = model.Quantity!.Value,
                UnitPrice = model.UnitPrice!.Value,
                Discount = model.Discount ?? 0m,
                TotalPrice = totalPrice,
                CreatedAt = model.CreatedAt ?? DateTime.UtcNow,
                OrderId = model.OrderId!.Value,
                ProductId = model.ProductId!.Value
            };

            _dbContext.OrderItems.Add(orderItem);
            await _dbContext.SaveChangesAsync();

            await UpdateOrderTotalPrice(orderItem.OrderId);

            var createdItem = await _dbContext.OrderItems
                .AsNoTracking()
                .Include(oi => oi.Order)
                .Include(oi => oi.Product)
                .FirstAsync(oi => oi.Id == orderItem.Id);

            return CreatedAtAction(
                nameof(GetById),
                new { id = orderItem.Id },
                createdItem.ToDto());
        }

        // PUT: /api/order-items/5
        [HttpPut("{id:int}")]
        public async Task<ActionResult<OrderItemDto>> Update(int id, [FromBody] OrderItemRequestDto model)
        {
            ValidateOrderItem(model);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var orderItem = await _dbContext.OrderItems
                .FirstOrDefaultAsync(oi => oi.Id == id);

            if (orderItem == null)
            {
                return NotFound();
            }

            var oldOrderId = orderItem.OrderId;

            var orderExists = await _dbContext.Orders
                .AnyAsync(o => o.Id == model.OrderId!.Value);

            if (!orderExists)
            {
                ModelState.AddModelError(nameof(model.OrderId), "Selected order does not exist.");
            }

            var productExists = await _dbContext.Products
                .AnyAsync(p => p.Id == model.ProductId!.Value);

            if (!productExists)
            {
                ModelState.AddModelError(nameof(model.ProductId), "Selected product does not exist.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var newTotalPrice = CalculateTotalPrice(
                model.Quantity!.Value,
                model.UnitPrice!.Value,
                model.Discount ?? 0m);

            orderItem.Quantity = model.Quantity!.Value;
            orderItem.UnitPrice = model.UnitPrice!.Value;
            orderItem.Discount = model.Discount ?? 0m;
            orderItem.TotalPrice = newTotalPrice;
            orderItem.CreatedAt = model.CreatedAt ?? orderItem.CreatedAt;
            orderItem.OrderId = model.OrderId!.Value;
            orderItem.ProductId = model.ProductId!.Value;

            await _dbContext.SaveChangesAsync();

            await UpdateOrderTotalPrice(orderItem.OrderId);

            if (oldOrderId != orderItem.OrderId)
            {
                await UpdateOrderTotalPrice(oldOrderId);
            }

            var updatedItem = await _dbContext.OrderItems
                .AsNoTracking()
                .Include(oi => oi.Order)
                .Include(oi => oi.Product)
                .FirstAsync(oi => oi.Id == orderItem.Id);

            return Ok(updatedItem.ToDto());
        }

        // DELETE: /api/order-items/5
        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var orderItem = await _dbContext.OrderItems
                .FirstOrDefaultAsync(oi => oi.Id == id);

            if (orderItem == null)
            {
                return NotFound();
            }

            var orderId = orderItem.OrderId;

            _dbContext.OrderItems.Remove(orderItem);
            await _dbContext.SaveChangesAsync();

            await UpdateOrderTotalPrice(orderId);

            return NoContent();
        }

        private void ValidateOrderItem(OrderItemRequestDto model)
        {
            if (model.Quantity.HasValue && model.Quantity.Value <= 0)
            {
                ModelState.AddModelError(nameof(model.Quantity), "Quantity must be greater than zero.");
            }

            if (model.UnitPrice.HasValue && model.UnitPrice.Value <= 0)
            {
                ModelState.AddModelError(nameof(model.UnitPrice), "Unit price must be greater than zero.");
            }

            if (model.Discount.HasValue && model.Discount.Value < 0)
            {
                ModelState.AddModelError(nameof(model.Discount), "Discount cannot be negative.");
            }

            if (model.Quantity.HasValue &&
                model.UnitPrice.HasValue &&
                model.Discount.HasValue)
            {
                var subtotal = model.Quantity.Value * model.UnitPrice.Value;

                if (model.Discount.Value > subtotal)
                {
                    ModelState.AddModelError(nameof(model.Discount), "Discount cannot be greater than subtotal.");
                }
            }
        }

        private static decimal CalculateTotalPrice(int quantity, decimal unitPrice, decimal discount)
        {
            var subtotal = quantity * unitPrice;
            var totalPrice = subtotal - discount;

            return totalPrice < 0 ? 0 : totalPrice;
        }

        private async Task UpdateOrderTotalPrice(int orderId)
        {
            var order = await _dbContext.Orders
                .Include(o => o.OrderItems)
                .FirstOrDefaultAsync(o => o.Id == orderId);

            if (order == null)
            {
                return;
            }

            order.TotalPrice = order.OrderItems.Sum(oi => oi.TotalPrice);

            await _dbContext.SaveChangesAsync();
        }
    }
}