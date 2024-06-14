using LibraryAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryAPI.Data.Repositories
{
    public class CopiesRepository
    {
        private readonly LibraryDbContext _context;

        public CopiesRepository(LibraryDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Copy>> GetAllCopies()
        {
            return await _context.Copies.OrderBy(x => x.Id).ToListAsync();
        }

        public async Task<Copy?> GetCopy(int id)
        {
            return await _context.Copies.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<int> GetCopiesCount(Book book)
        {
            return await _context.Copies.Where(x => x.Isbn == book.Isbn).CountAsync();
        }

        public async Task<Copy?> GetAnyCopy(Book book)
        {
            return await _context.Copies.FirstOrDefaultAsync(x => x.Isbn == book.Isbn);
        }

        public async Task<Copy> AddCopy(Copy copy)
        {
            await _context.Copies.AddAsync(copy);
            await _context.SaveChangesAsync();
            return copy;
        }

        public async Task<Copy?> DeleteCopy(Copy copy)
        {
            _context.Copies.Remove(copy);
            await _context.SaveChangesAsync();

            return null;
        }
    }
}
