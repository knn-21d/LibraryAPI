using LibraryAPI.Data;
using LibraryAPI.Data.DTOs;
using LibraryAPI.Data.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace LibraryAPI.Services
{
    public class BooksService
    {
        private readonly BooksRepository _booksRepositry;

        public BooksService(BooksRepository booksRepository) { 
            _booksRepositry = booksRepository;
        }

        public async Task<ActionResult<List<Book>>> SearchBooks(BooksSearchDto booksSearch)
        {
            return await _booksRepositry.SearchBooks(booksSearch);
        }
    }
}