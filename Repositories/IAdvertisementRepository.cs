using AdvertisingBoard.Data;
using Microsoft.EntityFrameworkCore;

namespace AdvertisingBoard.Repositories
{
    public interface IAdvertisementRepository
    {
        Task CreateAdvertisment(Advertisement advertisement);
        Task DeleteAdvertisment(Advertisement advertisement);
        Task UpdateAdvertisment(Advertisement advertisement);
        Task<Advertisement?> GetAdvertisementById(int id);
    }

    public class AdvertisementRepository : IAdvertisementRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly DbSet<Advertisement> _advertisements;

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

        public async Task UpdateAdvertisment(Advertisement advertisement)
        {
            _advertisements.Update(advertisement);
            await _context.SaveChangesAsync();
        }
    }
}
