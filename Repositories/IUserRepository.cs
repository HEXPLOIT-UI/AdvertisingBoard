using AdvertisingBoard.Data;
using Microsoft.EntityFrameworkCore;

namespace AdvertisingBoard.Repositories
{
    public interface IUserRepository
    {
        Task<User?> GetByIdAsync(int id);
        Task<User?> GetUserByLoginAsync(string login);
        Task<IEnumerable<User>> GetAllAsync();
        Task AddAsync(User user);
        Task UpdateAsync(User user);
        Task DeleteAsync(User user);
    }

    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly DbSet<User> _users;

        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
            _users = context.Set<User>();
        }

        public async Task<User?> GetByIdAsync(int id)
        {
            return await _users.FindAsync(id);
        }

        public async Task<User?> GetUserByLoginAsync(string login)
        {
            return await _users.FirstOrDefaultAsync(u => u.Login == login);
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            return await _users.ToListAsync();
        }

        public async Task AddAsync(User user)
        {
            await _users.AddAsync(user);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(User user)
        {
            _users.Update(user);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(User user)
        {
            _users.Remove(user);
            await _context.SaveChangesAsync();
        }
    }
}
