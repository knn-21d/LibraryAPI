﻿using LibraryAPI.Data.DTOs;
using Microsoft.EntityFrameworkCore;

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
            return (await _context.Books.ToListAsync()).Where(book =>
            {
                bool isMeetsCondition = true;
                isMeetsCondition = query.Name is not null ? book.Title.ToLower().Contains(query.Name.ToLower()) : true;
                isMeetsCondition = query.PagesFrom is not null ? book.Pages >= query.PagesFrom : true;
                isMeetsCondition = query.PagesTo is not null ? book.Pages <= query.PagesTo : true;
                isMeetsCondition = query.PublisherName is not null ? book.Publisher.Name.ToLower().Contains(query.PublisherName.ToLower()) : true;
                isMeetsCondition = query.Country is not null ? book.Publisher.Country.ToLower().Contains(query.Country.ToLower()) : true;
                isMeetsCondition = query.Year is not null ? book.Year == query.Year : true;
                isMeetsCondition = query.PriceFrom is not null ? book.Price >= query.PriceFrom : true;
                isMeetsCondition = query.PriceTo is not null ? query.PriceTo <= query.PriceTo : true;
                var auuthor = _authorsRepository.GetAuthorByBook(book.Isbn);
                isMeetsCondition = query.Author is not null && auuthor is not null 
                    ? (auuthor?.FirstName.ToLower() + " " + auuthor?.LastName.ToLower() + " " + auuthor?.Patronymic?.ToLower()).Contains(query.Author.ToLower()) 
                    : true;
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
