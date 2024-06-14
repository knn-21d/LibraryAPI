using LibraryAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryAPI.Data.Repositories
{
    public class PublishersRepository
    {
        private readonly LibraryDbContext _context;

        public PublishersRepository(LibraryDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Publisher>> GetAllPublishers()
        {
            return await _context.Publishers.OrderBy(x => x.Id).ToListAsync();
        }

        public async Task<Publisher?> GetPublisher(int id)
        {
            return await _context.Publishers.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Publisher> AddPublisher(Publisher publisher)
        {
            await _context.Publishers.AddAsync(publisher);
            await _context.SaveChangesAsync();
            return publisher;
        }

        internal Task<object?> EditPublisher(int id, Publisher publisher)
        {
            throw new NotImplementedException();
        }

        internal Task<object?> DeletePublisher(int id)
        {
            throw new NotImplementedException();
        }
    }
}
