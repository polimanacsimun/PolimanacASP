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

        public void Add(Order order)
        {
            _context.Orders.Add(order);
            _context.SaveChanges();
        }

        public void Update(Order order)
        {
            _context.Orders.Update(order);
            _context.SaveChanges();
        }

        public bool Delete(int id)
        {
            if (!Exists(id))
                return false;

            if (!CanDelete(id))
                return false;

            var order = _context.Orders.Find(id);
            if (order != null)
            {
                _context.Orders.Remove(order);
                _context.SaveChanges();
                return true;
            }

            return false;
        }

        public bool Exists(int id)
        {
            return _context.Orders.Any(o => o.Id == id);
        }

        public bool CanDelete(int id)
        {
            var order = _context.Orders
                .AsNoTracking()
                .Include(o => o.OrderItems)
                .FirstOrDefault(o => o.Id == id);

            if (order == null)
                return false;

            return !order.OrderItems.Any();
        }

        public List<Order> Search(string? term, int maxResults = 50)
        {
            var query = _context.Orders.AsNoTracking()
                .Include(o => o.User)
                .Include(o => o.OrderItems);

            if (string.IsNullOrWhiteSpace(term))
            {
                return query
                    .OrderByDescending(o => o.OrderDate)
                    .Take(maxResults)
                    .ToList();
            }

            term = term.ToLower();
            return query
                .Where(o =>
                    o.OrderNumber.ToLower().Contains(term) ||
                    o.Note.ToLower().Contains(term) ||
                    o.Status.ToString().ToLower().Contains(term) ||
                    (o.User != null && (
                        o.User.FirstName.ToLower().Contains(term) ||
                        o.User.LastName.ToLower().Contains(term) ||
                        o.User.Email.ToLower().Contains(term))))
                .OrderByDescending(o => o.OrderDate)
                .Take(maxResults)
                .ToList();
        }
    }
}
