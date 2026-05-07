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
    }
}
