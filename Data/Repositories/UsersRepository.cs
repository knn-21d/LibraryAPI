using Microsoft.EntityFrameworkCore;

namespace LibraryAPI.Data.Repositories
{
    public class UsersRepository
    {
        private readonly LibraryDbContext _context;

        public UsersRepository(LibraryDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<User>> GetAllUsers()
        {
            return await _context.Users.OrderBy(x => x.Id).ToListAsync();
        }

        public async Task<User?> GetUserByLogin(string login)
        {
            return await _context.Users.FirstOrDefaultAsync(x => x.Login == login);
        }

        public async Task<User?> GetUser(int id)
        {
            return await _context.Users.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<User> AddUser(User user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            return user;
        }

        internal async Task<User> EditUser(int id, User user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
            return user;
        }

        internal async Task<User> DeleteUser(int id)
        {
            await _context.Users.Where(x => x.Id == id).ExecuteDeleteAsync();
            return await _context.Users.FirstAsync(x => x.Id == id);
        }
    }
}
