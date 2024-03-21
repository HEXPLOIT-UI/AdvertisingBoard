using AdvertisingBoard.Data;
using Microsoft.EntityFrameworkCore;

namespace AdvertisingBoard.Repositories
{
    public interface IPhotoRepository
    {
        Task CreatePhoto(Photo photo);
        Task DeletePhoto(Photo photo);
        Task<IEnumerable<Photo>> GetPhotosByAdid(int adId);
        Task<Photo?> GetPhotoById(int id);

    }

    public class PhotoRepository : IPhotoRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly DbSet<Photo> _photos;
        public PhotoRepository(ApplicationDbContext context)
        {
            _context = context;
            _photos = _context.Set<Photo>();
        }

        public async Task CreatePhoto(Photo photo)
        {
            await _photos.AddAsync(photo);
            await _context.SaveChangesAsync();
        }

        public async Task DeletePhoto(Photo photo)
        {
            _photos.Remove(photo);
            await _context.SaveChangesAsync();
        }

        public async Task<Photo?> GetPhotoById(int id)
        {
            return await _photos.FindAsync(id);
        }

        public async Task<IEnumerable<Photo>> GetPhotosByAdid(int adId)
        {
            return await _photos.Where(p => p.AdvertisementId == adId).ToListAsync();
        }
    }
}
