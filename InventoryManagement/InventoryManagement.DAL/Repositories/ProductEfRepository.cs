using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using InventoryManagement.Domain.Models;

namespace InventoryManagement.DAL.Repositories
{
    public class ProductEfRepository
    {
        private readonly InventoryManagementDbContext _context;

        public ProductEfRepository(InventoryManagementDbContext context)
        {
            _context = context;
        }

        public List<Product> GetAll()
        {
            return _context.Products
                .AsNoTracking()
                .Include(p => p.Category)
                .Include(p => p.Supplier)
                .ToList();
        }

        public Product? GetById(int id)
        {
            return _context.Products
                .AsNoTracking()
                .Include(p => p.Category)
                .Include(p => p.Supplier)
                .Include(p => p.InventoryItems)
                    .ThenInclude(ii => ii.Warehouse)
                .Include(p => p.OrderItems)
                    .ThenInclude(oi => oi.Order)
                .FirstOrDefault(p => p.Id == id);
        }
    }
}
