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
                .Include(p => p.Attachments)
                .Include(p => p.InventoryItems)
                    .ThenInclude(ii => ii.Warehouse)
                .Include(p => p.OrderItems)
                    .ThenInclude(oi => oi.Order)
                .FirstOrDefault(p => p.Id == id);
        }

        public void Add(Product product)
        {
            _context.Products.Add(product);
            _context.SaveChanges();
        }

        public void Update(Product product)
        {
            _context.Products.Update(product);
            _context.SaveChanges();
        }

        public bool Delete(int id)
        {
            var product = _context.Products.FirstOrDefault(p => p.Id == id);

            if (product == null)
                return false;

            if (!CanDelete(id))
                return false;

            _context.Products.Remove(product);
            _context.SaveChanges();
            return true;
        }

        public bool Exists(int id)
        {
            return _context.Products.Any(p => p.Id == id);
        }

        public bool CanDelete(int id)
        {
            var product = _context.Products
                .Include(p => p.OrderItems)
                .Include(p => p.InventoryItems)
                .FirstOrDefault(p => p.Id == id);

            if (product == null)
                return false;

            return !product.OrderItems.Any() && !product.InventoryItems.Any();
        }

        public List<Product> Search(string? term, int maxResults = 50)
        {
            IQueryable<Product> query = _context.Products
                .Include(p => p.Category)
                .Include(p => p.Supplier)
                .AsNoTracking();

            if (!string.IsNullOrWhiteSpace(term))
            {
                query = query.Where(p =>
                    p.Name.Contains(term) ||
                    (p.Description != null && p.Description.Contains(term)) ||
                    (p.Category != null && p.Category.Name.Contains(term)) ||
                    (p.Supplier != null && p.Supplier.Name.Contains(term)));
            }

            return query
                .OrderBy(p => p.Name)
                .Take(maxResults)
                .ToList();
        }
    }
}
