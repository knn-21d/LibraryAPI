using LibraryAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryAPI.Data.Repositories
{
    public class BooksRepository
    {
        private readonly LibraryDbContext _context;

        public BooksRepository(LibraryDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Book>> GetAllBooks()
        {
            return await _context.Books.OrderBy(x => x.Isbn).ToListAsync();
        }

        public async Task<Book?> GetBook(string isbn)
        {
            return await _context.Books.FirstOrDefaultAsync(x => x.Isbn == isbn);
        }

        public async Task<Book> AddBook(Book book)
        {
            await _context.Books.AddAsync(book);
            await _context.SaveChangesAsync();
            return book;
        }

        public async Task<Book> SetTitle(Book book, string value)
        {
            book.Title = value;
            await _context.SaveChangesAsync();
            return book;
        }

        public async Task<Book> SetYear(Book book, int value)
        {
            book.Year = value;
            await _context.SaveChangesAsync();
            return book;
        }

        public async Task<Book> SetPages(Book book, int value)
        {
            book.Pages = value;
            await _context.SaveChangesAsync();
            return book;
        }

        internal async Task<Book?> DeleteBook(string isbn)
        {
            await _context.Books.Where(x => x.Isbn == isbn).ExecuteDeleteAsync();
            return null;
        }

        public async Task<Book> EditBook(string isbn, Book book)
        {
            _context.Books.Update(book);
            await _context.SaveChangesAsync();
            return book;
        }
    }
}
