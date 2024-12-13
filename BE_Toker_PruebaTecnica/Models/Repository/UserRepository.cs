using Microsoft.EntityFrameworkCore;

namespace BE_Toker_PruebaTecnica.Models.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;

        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<User> AddUser(User user)
        {
            _context.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task deleteUser(User user)
        {
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
        }

        public async Task<List<User>> GetListUser()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<User?> GetUser(int id)
        {
            return await _context.Users.FindAsync(id);
        }

        public async Task UpdateUser(User user)
        {
            var userItem = await _context.Users.FirstOrDefaultAsync(x => x.Id == user.Id);

            if (userItem != null)
            {
                userItem.Name = user.Name;
                userItem.Telefono = user.Telefono;

                await _context.SaveChangesAsync();
            }
        }
    }
}
