using AdvertisingBoard.Data;
using Microsoft.EntityFrameworkCore;
using static Microsoft.Extensions.Logging.EventSource.LoggingEventSource;

namespace AdvertisingBoard.Repositories
{
    public interface IAdvertisementRepository
    {
        Task CreateAdvertisment(Advertisement advertisement);
        Task DeleteAdvertisment(Advertisement advertisement);
        Task UpdateAdvertisment(Advertisement advertisement);
        Task<Advertisement?> GetAdvertisementById(int id);
        Task<IEnumerable<Advertisement>> GetAllAdvertisementOnPage(int page, int categoryId);
        Task<IEnumerable<Advertisement>> SearchAdvertisementByKeywordsOnPage(int page, string searchWords, int categoryId, string contacts);
        Task DeleteAll();
    }

    public class AdvertisementRepository : IAdvertisementRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly DbSet<Advertisement> _advertisements;
        private const int pageSize = 10;

        public AdvertisementRepository(ApplicationDbContext context)
        {
            _context = context;
            _advertisements = context.Set<Advertisement>();
        }

        public async Task CreateAdvertisment(Advertisement advertisement)
        {
            await _advertisements.AddAsync(advertisement);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAdvertisment(Advertisement advertisement)
        {
            _advertisements.Remove(advertisement);
            await _context.SaveChangesAsync();
        }

        public async Task<Advertisement?> GetAdvertisementById(int id)
        {
            return await _advertisements.FindAsync(id);
        }

        public async Task<IEnumerable<Advertisement>> GetAllAdvertisementOnPage(int page, int categoryId)
        {
            //var totalItems = _advertisements.Count();
            var advertisements = await _advertisements.Where(ad => (categoryId > 0 ? ad.CategoryId == categoryId : true))
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
            if (advertisements == null || advertisements.Count == 0)
            {
                return [];
            }
            //var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);
            return advertisements;
        }

        public async Task<IEnumerable<Advertisement>> SearchAdvertisementByKeywordsOnPage(int page, string keyword, int categoryId, string contacts)
        {
            var searchWords = keyword.ToLower().Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            var searchContacts = contacts.ToLower().Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            var advertisements = await _context.Advertisements
                .Where(ad =>
                    (categoryId > 0 ? ad.CategoryId == categoryId : true) &&
                    searchWords.Any(word => ad.Title.ToLower().Contains(word) || ad.Description.ToLower().Contains(word)) && 
                    searchContacts.Any(contact => ad.ContactInfo.ToLower().Contains(contact)))
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            if (advertisements == null || advertisements.Count == 0)
            {
                return [];
            }
            return advertisements;
        }

        public async Task UpdateAdvertisment(Advertisement advertisement)
        {
            _advertisements.Update(advertisement);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAll()
        {
            var photos = await _advertisements.ToListAsync();
            _advertisements.RemoveRange(photos);
            await _context.SaveChangesAsync();
        }
    }
}
