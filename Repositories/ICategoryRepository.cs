using AdvertisingBoard.Data;
using Microsoft.EntityFrameworkCore;

namespace AdvertisingBoard.Repositories
{
    public interface ICategoryRepository
    {
        Task<Category?> GetByIdAsync(int id);
        Task<Category?> GetByNameAsync(string name);
        Task<IEnumerable<Category>> GetAllAsync();
        Task AddAsync(Category category);
        Task UpdateAsync(Category category);
        Task DeleteAsync(Category category);
    }

    public class CategoryRepository : ICategoryRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly DbSet<Category> _categories;

        public CategoryRepository(ApplicationDbContext context)
        {
            _context = context;
            _categories = context.Set<Category>();
        }

        public async Task AddAsync(Category category)
        {
            await _categories.AddAsync(category);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Category category)
        {
            _categories.Remove(category);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Category>> GetAllAsync()
        {
            return await _categories.ToListAsync();
        }

        public async Task<Category?> GetByIdAsync(int id)
        {
            return await _categories.FindAsync(id);
        }

        public async Task<Category?> GetByNameAsync(string name)
        {
            return await _categories.FirstOrDefaultAsync(u => u.Name == name);
        }

        public async Task UpdateAsync(Category category)
        {
            _categories.Update(category);
            await _context.SaveChangesAsync();
        }
    }
}
