using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using InventoryManagement.Domain.Models;

namespace InventoryManagement.DAL.Repositories
{
    public class WarehouseEfRepository
    {
        private readonly InventoryManagementDbContext _context;

        public WarehouseEfRepository(InventoryManagementDbContext context)
        {
            _context = context;
        }

        public List<Warehouse> GetAll()
        {
            return _context.Warehouses
                .AsNoTracking()
                .Include(w => w.InventoryItems)
                .ToList();
        }

        public Warehouse? GetById(int id)
        {
            return _context.Warehouses
                .AsNoTracking()
                .Include(w => w.InventoryItems)
                    .ThenInclude(ii => ii.Product)
                .FirstOrDefault(w => w.Id == id);
        }
    }
}
