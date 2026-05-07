using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using InventoryManagement.Domain.Models;

namespace InventoryManagement.DAL.Repositories
{
    public class UserEfRepository
    {
        private readonly InventoryManagementDbContext _context;

        public UserEfRepository(InventoryManagementDbContext context)
        {
            _context = context;
        }

        public List<User> GetAll()
        {
            return _context.Users
                .AsNoTracking()
                .Include(u => u.Orders)
                .ToList();
        }

        public User? GetById(int id)
        {
            return _context.Users
                .AsNoTracking()
                .Include(u => u.Orders)
                .FirstOrDefault(u => u.Id == id);
        }
    }
}
