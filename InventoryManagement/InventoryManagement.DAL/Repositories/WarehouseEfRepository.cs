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

        public void Add(Warehouse warehouse)
        {
            _context.Warehouses.Add(warehouse);
            _context.SaveChanges();
        }

        public void Update(Warehouse warehouse)
        {
            _context.Warehouses.Update(warehouse);
            _context.SaveChanges();
        }

        public bool Delete(int id)
        {
            if (!Exists(id))
                return false;

            if (!CanDelete(id))
                return false;

            var warehouse = _context.Warehouses.Find(id);
            if (warehouse != null)
            {
                _context.Warehouses.Remove(warehouse);
                _context.SaveChanges();
                return true;
            }

            return false;
        }

        public bool Exists(int id)
        {
            return _context.Warehouses.Any(w => w.Id == id);
        }

        public bool CanDelete(int id)
        {
            var warehouse = _context.Warehouses
                .AsNoTracking()
                .Include(w => w.InventoryItems)
                .FirstOrDefault(w => w.Id == id);

            if (warehouse == null)
                return false;

            return !warehouse.InventoryItems.Any();
        }

        public List<Warehouse> Search(string? term, int maxResults = 50)
        {
            var query = _context.Warehouses.AsNoTracking().Include(w => w.InventoryItems);

            if (string.IsNullOrWhiteSpace(term))
            {
                return query
                    .OrderBy(w => w.Name)
                    .Take(maxResults)
                    .ToList();
            }

            term = term.ToLower();
            return query
                .Where(w =>
                    w.Name.ToLower().Contains(term) ||
                    w.Address.ToLower().Contains(term) ||
                    w.Phone.ToLower().Contains(term) ||
                    w.Email.ToLower().Contains(term) ||
                    w.Manager.ToLower().Contains(term) ||
                    w.Type.ToString().ToLower().Contains(term))
                .OrderBy(w => w.Name)
                .Take(maxResults)
                .ToList();
        }
    }
}
