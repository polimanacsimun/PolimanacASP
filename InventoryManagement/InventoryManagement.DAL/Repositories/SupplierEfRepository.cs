using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using InventoryManagement.Domain.Models;

namespace InventoryManagement.DAL.Repositories
{
    public class SupplierEfRepository
    {
        private readonly InventoryManagementDbContext _context;

        public SupplierEfRepository(InventoryManagementDbContext context)
        {
            _context = context;
        }

        public List<Supplier> GetAll()
        {
            return _context.Suppliers
                .AsNoTracking()
                .Include(s => s.Products)
                .ToList();
        }

        public Supplier? GetById(int id)
        {
            return _context.Suppliers
                .AsNoTracking()
                .Include(s => s.Products)
                .FirstOrDefault(s => s.Id == id);
        }

        public List<Supplier> Search(string? term, int maxResults = 50)
        {
            var query = _context.Suppliers.AsNoTracking();

            if (!string.IsNullOrWhiteSpace(term))
            {
                query = query.Where(s =>
                    s.Name.Contains(term) ||
                    s.Address.Contains(term) ||
                    s.Phone.Contains(term) ||
                    s.Email.Contains(term) ||
                    s.ContactPerson.Contains(term));
            }

            return query.OrderBy(s => s.Name).Take(maxResults).ToList();
        }

        public void Add(Supplier supplier)
        {
            _context.Suppliers.Add(supplier);
            _context.SaveChanges();
        }

        public void Update(Supplier supplier)
        {
            _context.Suppliers.Update(supplier);
            _context.SaveChanges();
        }

        public bool Delete(int id)
        {
            var supplier = _context.Suppliers.FirstOrDefault(s => s.Id == id);

            if (supplier == null)
                return false;

            if (!CanDelete(id))
                return false;

            _context.Suppliers.Remove(supplier);
            _context.SaveChanges();
            return true;
        }

        public bool Exists(int id)
        {
            return _context.Suppliers.Any(s => s.Id == id);
        }

        public bool CanDelete(int id)
        {
            var supplier = _context.Suppliers
                .Include(s => s.Products)
                .FirstOrDefault(s => s.Id == id);

            if (supplier == null)
                return false;

            return !supplier.Products.Any();
        }
    }
}
