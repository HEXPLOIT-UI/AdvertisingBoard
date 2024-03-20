using AdvertisingBoard.Data;
using Microsoft.EntityFrameworkCore;

namespace AdvertisingBoard.Repositories
{
    public interface IAdvertisementRepository
    {
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
    }
}
