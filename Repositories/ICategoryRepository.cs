using AdvertisingBoard.Data;
using Microsoft.EntityFrameworkCore;

namespace AdvertisingBoard.Repositories
{
    public interface ICategoryRepository
    {
        Task<Category?> GetByIdAsync(int id);
        Task<IEnumerable<Category>> GetAllParentsCategories();
        Task CreateCategory(Category category);
        Task UpdateCategory(int id, Category category);
        Task DeleteAsync(Category category);
        Task<IEnumerable<Category>> GetCategoriesByParentId (int parentId);
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

        public async Task CreateCategory(Category category)
        {
            
            await _categories.AddAsync(category);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Category category)
        {
            _categories.Remove(category);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Category>> GetAllParentsCategories()
        {
            return await _categories.Where(c => c.ParentCategoryId == null).ToListAsync();
            
        }

        public async Task<Category?> GetByIdAsync(int id)
        {
            return await _categories.FindAsync(id);
        }

        public async Task<IEnumerable<Category>> GetCategoriesByParentId(int parentId)
        {
            return await _categories.Where(c => c.ParentCategoryId == parentId).ToListAsync();
        }

        public async Task UpdateCategory(int id, Category category)
        {
            var originalCategory = await _categories.FindAsync(id);
            originalCategory.Name = category.Name;
            originalCategory.ParentCategoryId = category.ParentCategoryId;
            _categories.Update(originalCategory);
            await _context.SaveChangesAsync();
        }
    }
}
