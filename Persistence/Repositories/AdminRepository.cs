using Domain.Entities;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using Persistence.DataAccess;
using System.Linq;
using System.Threading.Tasks;

namespace Persistence.Repositories
{
    public class AdminRepository : IAdminRepository
    {
        private readonly DoctorShopContext _context;

        public AdminRepository(DoctorShopContext context)
        {
            _context = context;
        }

        public async Task<bool> CheckAdminAsync(string login, string password)
        {
            var admin = await _context.Admins.Where(x => x.Login == login && x.Password == password).FirstOrDefaultAsync();

            if (admin != null)
            {
                return true;
            }

            return false;
        }

        public async Task AddAdminAsync(Admin admin)
		{
            await _context.Admins.AddAsync(admin);
            await _context.SaveChangesAsync();
		}

    }
}
