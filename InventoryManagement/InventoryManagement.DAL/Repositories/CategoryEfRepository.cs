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

        public void Add(Category category)
        {
            _context.Categories.Add(category);
            _context.SaveChanges();
        }

        public void Update(Category category)
        {
            _context.Categories.Update(category);
            _context.SaveChanges();
        }

        public bool Delete(int id)
        {
            var category = _context.Categories.FirstOrDefault(c => c.Id == id);

            if (category == null)
                return false;

            if (!CanDelete(id))
                return false;

            _context.Categories.Remove(category);
            _context.SaveChanges();
            return true;
        }

        public bool Exists(int id)
        {
            return _context.Categories.Any(c => c.Id == id);
        }

        public bool CanDelete(int id)
        {
            var category = _context.Categories
                .Include(c => c.Products)
                .FirstOrDefault(c => c.Id == id);

            if (category == null)
                return false;

            return !category.Products.Any();
        }
        
    }
}
