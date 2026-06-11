using InventoryManagement.Domain.Models;
using Microsoft.EntityFrameworkCore;

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
            return _context.BusinessUsers
                .AsNoTracking()
                .Include(u => u.Orders)
                .ToList();
        }

        public User? GetById(int id)
        {
            return _context.BusinessUsers
                .AsNoTracking()
                .Include(u => u.Orders)
                .FirstOrDefault(u => u.Id == id);
        }

        public void Add(User user)
        {
            _context.BusinessUsers.Add(user);
            _context.SaveChanges();
        }

        public void Update(User user)
        {
            _context.BusinessUsers.Update(user);
            _context.SaveChanges();
        }

        public bool Delete(int id)
        {
            if (!Exists(id))
            {
                return false;
            }

            if (!CanDelete(id))
            {
                return false;
            }

            var user = _context.BusinessUsers.Find(id);

            if (user == null)
            {
                return false;
            }

            _context.BusinessUsers.Remove(user);
            _context.SaveChanges();

            return true;
        }

        public bool Exists(int id)
        {
            return _context.BusinessUsers.Any(u => u.Id == id);
        }

        public bool CanDelete(int id)
        {
            var user = _context.BusinessUsers
                .AsNoTracking()
                .Include(u => u.Orders)
                .FirstOrDefault(u => u.Id == id);

            if (user == null)
            {
                return false;
            }

            return !user.Orders.Any();
        }

        public List<User> Search(string? term, int maxResults = 50)
        {
            IQueryable<User> query = _context.BusinessUsers
                .AsNoTracking()
                .Include(u => u.Orders);

            if (string.IsNullOrWhiteSpace(term))
            {
                return query
                    .OrderBy(u => u.LastName)
                    .ThenBy(u => u.FirstName)
                    .Take(maxResults)
                    .ToList();
            }

            term = term.ToLower();

            return query
                .Where(u =>
                    u.FirstName.ToLower().Contains(term) ||
                    u.LastName.ToLower().Contains(term) ||
                    u.Email.ToLower().Contains(term) ||
                    u.Role.ToString().ToLower().Contains(term))
                .OrderBy(u => u.LastName)
                .ThenBy(u => u.FirstName)
                .Take(maxResults)
                .ToList();
        }
    }
}