using LibraryAPI.Data.DTOs;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace LibraryAPI.Data.Repositories
{
    public class BooksRepository
    {
        private readonly LibraryDbContext _context;
        private readonly AuthorsRepository _authorsRepository;

        public BooksRepository(LibraryDbContext context, AuthorsRepository authorsRepository)
        {
            _context = context;
            _authorsRepository = authorsRepository;
        }

        public async Task<IEnumerable<Book>> GetAllBooks()  
        {
            return await _context.Books.OrderBy(x => x.Isbn).ToListAsync();
        }

        public async Task<List<Book>> SearchBooks(BooksSearchDto query)
        {
            return (await _context.Books.Include(b => b.Publisher).Include(b => b.AuthorBooks).ThenInclude(b => b.Author).ToListAsync()).Where(book =>
            {
                bool isMeetsCondition = true;
                isMeetsCondition = query.Name is not null ? book.Title.ToLower().Contains(query.Name.ToLower()) : true;
                isMeetsCondition = isMeetsCondition && (query.PagesFrom is not null ? book.Pages >= query.PagesFrom : true);
                isMeetsCondition = isMeetsCondition && (query.PagesTo is not null ? book.Pages <= query.PagesTo : true);
                isMeetsCondition = isMeetsCondition && (
                    query.PublisherName is not null ? book.Publisher.Name.ToLower().Contains(query.PublisherName.ToLower()) : true
                );
                isMeetsCondition = isMeetsCondition && (query.Country is not null ? book.Publisher.Country.ToLower().Contains(query.Country.ToLower()) : true);
                isMeetsCondition = isMeetsCondition && (query.Year is not null ? book.Year == query.Year : true);
                isMeetsCondition = isMeetsCondition && (query.PriceFrom is not null ? book.Price >= query.PriceFrom : true);
                isMeetsCondition = isMeetsCondition && (query.PriceTo is not null ? book.Price <= query.PriceTo : true);
                isMeetsCondition = isMeetsCondition && (query.Author is not null && book.AuthorBooks.Count > 0
                    ? book.AuthorBooks.Any(auuthor => (auuthor.Author?.FirstName.ToLower() + " " + auuthor.Author?.LastName.ToLower() + " " + auuthor.Author?.Patronymic?.ToLower()).Contains(query.Author.ToLower())) 
                    : true);
                return isMeetsCondition;
            }).ToList();
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
