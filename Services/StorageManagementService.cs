using LibraryAPI.Data;
using LibraryAPI.Data.DTOs;
using LibraryAPI.Data.Repositories;
using System.Web.Http;

namespace LibraryAPI.Services
{
    public class StorageManagementService
    {
        private readonly CopiesRepository _copiesRepository;
        private readonly BooksRepository _booksRepository;
        private readonly PublishersRepository _publisherRepository;

        public StorageManagementService(CopiesRepository copiesRepository, BooksRepository booksRepository, PublishersRepository publishersRepository)
        {
            _copiesRepository = copiesRepository;
            _booksRepository = booksRepository;
            _publisherRepository = publishersRepository;
        }

        public async Task<Copy?> DeleteCopy(int id)
        {
            Copy copy = await _copiesRepository.GetCopy(id) ?? throw new HttpResponseException(System.Net.HttpStatusCode.NotFound);

            await _copiesRepository.DeleteCopy(copy);

            return null;
        }

        public async Task<Copy> AddCopy(string isbn)
        {
            return await _copiesRepository.AddCopy(new Copy { Isbn = isbn });
        }

        public async Task<IEnumerable<Copy>> AddNCopies(string isbn, int n)
        {
            List<Copy> result = new();

            for (int i = 0; i < n; i++)
            {
                result.Add(await AddCopy(isbn));
            }

            return result;
        }

        public async Task<Book?> AddBoock(BookDto book)
        {

            if (book.PublisherId is not null)
            {
                var newBook = new Book{ Isbn = book.Isbn, Pages = book.Pages, Price = book.Price, PublisherId = (int)book.PublisherId, Year = book.Year, Title = book.Title };
                return await this._booksRepository.AddBook(newBook);
            } else if (book.Publisher is not null)
            {
                var publisher = new Publisher{
                    Address = book.Publisher.Address,
                    City = book.Publisher.City,
                    Country = book.Publisher.Country,
                    Name = book.Publisher.Name,
                    Phone = book.Publisher.Phone,
                };
                var newPublisher = await this._publisherRepository.AddPublisher(publisher);
                var newBook = new Book{ Isbn = book.Isbn, Pages = book.Pages, Price = book.Price, PublisherId = newPublisher.Id, Year = book.Year, Title = book.Title };
                return await this._booksRepository.AddBook(newBook);
            }

            return null;
        }
    }
}
