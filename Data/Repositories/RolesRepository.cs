using Microsoft.EntityFrameworkCore;

namespace LibraryAPI.Data.Repositories
{
    public class RolesRepository
    {
        private readonly LibraryDbContext _context;

        public RolesRepository(LibraryDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Role>> GetAllRoles()
        {
            return await _context.Roles.OrderBy(x => x.Id).ToListAsync();
        }

        public async Task<Role?> GetRole(int id)
        {
            return await _context.Roles.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Role> AddRole(Role role)
        {
            await _context.Roles.AddAsync(role);
            await _context.SaveChangesAsync();
            return role;
        }
    }
}
