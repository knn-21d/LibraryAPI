using LibraryAPI.Data;
using LibraryAPI.Data.DTOs;
using LibraryAPI.Data.Repositories;
using System.Web.Http;
using System.Linq;


namespace LibraryAPI.Services
{
    public class StorageManagementService
    {
        private readonly CopiesRepository _copiesRepository;
        private readonly BooksRepository _booksRepository;
        private readonly PublishersRepository _publisherRepository;
        private readonly AuthorsRepository _authorsRepository;

        public StorageManagementService(CopiesRepository copiesRepository, BooksRepository booksRepository, PublishersRepository publishersRepository, AuthorsRepository authorsRepository)
        {
            _copiesRepository = copiesRepository;
            _booksRepository = booksRepository;
            _publisherRepository = publishersRepository;
            _authorsRepository = authorsRepository;
        }

        public async Task DeleteCopy(int id)
        {
            Copy copy = await _copiesRepository.GetCopy(id) ?? throw new HttpResponseException(System.Net.HttpStatusCode.NotFound);

            await _copiesRepository.DeleteCopy(copy);
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
            var publisher = book.Publisher is not null ? await this._publisherRepository.AddPublisher(new Publisher
            {
                Address = book.Publisher.Address,
                City = book.Publisher.City,
                Country = book.Publisher.Country,
                Name = book.Publisher.Name,
                Phone = book.Publisher.Phone,
            }) : null;
            var publisherId = publisher is not null ? publisher.Id : book.PublisherId;
            var authors = book.Author?.Length > 0
                ? book.Author.Select(
                    async (b) => await _authorsRepository.AddAuthor(new Author { FirstName = b.FirstName, LastName = b.LastName, Patronymic = b.Patronymic })
                ) .Select(it => it.Result)
                : null;
            var authorsIds = (book.AuthorId is not null ? book.AuthorId.ToList()! : new List<int> { })
                .Concat(authors is not null ? authors.Select(a => a.Id).ToList() : new List<int> { });

            var newBook = await _booksRepository.AddBook(new Book {
                Isbn = book.Isbn,
                Pages = book.Pages,
                Price = book.Price,
                PublisherId = (int)publisherId!,
                Year = book.Year, 
                Title = book.Title,
                Publisher = publisher is not null ? publisher : null,
                // AuthorBooks = authorBooks is not null ? authorBooks : new List<AuthorBook> { },
            });

            var authorBooks = authorsIds
                .Select(async (id) => await _authorsRepository.AddAuthorBook(new AuthorBook { AuthorId = id, Isbn = newBook.Isbn }))
                .Select(it => it.Result).ToList();

            return newBook;
        }
    }
}
