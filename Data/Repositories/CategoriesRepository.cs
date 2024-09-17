using Microsoft.EntityFrameworkCore;

namespace LibraryAPI.Data.Repositories
{
    public class CategoriesRepository
    {
        private readonly LibraryDbContext _context;

        public CategoriesRepository(LibraryDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Category>> GetAllCategories()
        {
            return await _context.Categories.OrderBy(x => x.Id).ToListAsync();
        }

        public async Task<Category?> GetCategory(int id)
        {
            return await _context.Categories.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Category> AddCategory(Category category)
        {
            await _context.Categories.AddAsync(category);
            await _context.SaveChangesAsync();
            return category;
        }

        public async Task<BookCategory> AddBookCategory(int categoryId, string bookId)
        {
            var bookCategory = new BookCategory { Isbn = bookId, CategoryId = categoryId };
            await _context.BookCategories.AddAsync(bookCategory);
            await _context.SaveChangesAsync();
            return bookCategory;
        }
    }
}
