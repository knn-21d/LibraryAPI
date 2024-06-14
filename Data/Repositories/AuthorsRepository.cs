using LibraryAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryAPI.Data.Repositories
{
    public class AuthorsRepository
    {
        private readonly LibraryDbContext _context;

        public AuthorsRepository(LibraryDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Author>> GetAllAuthors()
        {
            return await _context.Authors.OrderBy(x => x.Id).ToListAsync();
        }

        public async Task<Author?> GetAuthor(int id)
        {
            return await _context.Authors.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Author> AddAuthor(Author author)
        {
            await _context.Authors.AddAsync(author);
            await _context.SaveChangesAsync();
            return author;
        }

        public async Task<Author> SetFirstName(Author author, string value)
        {
            author.FirstName = value;
            await _context.SaveChangesAsync();
            return author;
        }

        public async Task<Author> SetPatronymic(Author author, string value)
        {
            author.Patronymic = value;
            await _context.SaveChangesAsync();
            return author;
        }

        public async Task<Author> SetLastName(Author author, string value)
        {
            author.LastName = value;
            await _context.SaveChangesAsync();
            return author;
        }

        public async Task<Author> EditAuthor(Author author)
        {
            _context.Authors.Update(author);
            await _context.SaveChangesAsync();
            return author;
        }

        internal async Task<Author?> DeleteAuthor(int id)
        {
            await _context.Authors.Where(x => x.Id == id).ExecuteDeleteAsync();
            return null;
        }
    }
}
