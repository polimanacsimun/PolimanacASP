using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using InventoryManagement.Domain.Models;

namespace InventoryManagement.DAL.Repositories
{
    public class OrderEfRepository
    {
        private readonly InventoryManagementDbContext _context;

        public OrderEfRepository(InventoryManagementDbContext context)
        {
            _context = context;
        }

        public List<Order> GetAll()
        {
            return _context.Orders
                .AsNoTracking()
                .Include(o => o.User)
                .Include(o => o.OrderItems)
                .ToList();
        }

        public Order? GetById(int id)
        {
            return _context.Orders
                .AsNoTracking()
                .Include(o => o.User)
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Product)
                .FirstOrDefault(o => o.Id == id);
        }
    }
}
