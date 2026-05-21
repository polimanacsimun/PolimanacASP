using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using InventoryManagement.Domain.Models;

namespace InventoryManagement.DAL.Repositories
{
    public class CategoryEfRepository
    {
        private readonly InventoryManagementDbContext _context;

        public CategoryEfRepository(InventoryManagementDbContext context)
        {
            _context = context;
        }

        public List<Category> GetAll()
        {
            return _context.Categories
                .AsNoTracking()
                .Include(c => c.Products)
                .ToList();
        }

        public Category? GetById(int id)
        {
            return _context.Categories
                .AsNoTracking()
                .Include(c => c.Products)
                .FirstOrDefault(c => c.Id == id);
        }

        public List<Category> Search(string term, int maxResults = 10)
        {
            var query = _context.Categories.AsNoTracking();

            if (!string.IsNullOrWhiteSpace(term))
            {
                query = query.Where(c => c.Name.Contains(term));
            }

            return query.OrderBy(c => c.Name).Take(maxResults).ToList();
        }
        
    }
}
