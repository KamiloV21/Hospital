using Domain.Entities;
using System.Threading.Tasks;

namespace Domain.Repositories
{
    public interface IAdminRepository
    {
        public Task<bool> CheckAdminAsync(string login, string password);
        public Task AddAdminAsync(Admin admin);
    }
}
