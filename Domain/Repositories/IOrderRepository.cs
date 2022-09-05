using Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Domain.Repositories
{
    public interface IOrderRepository
    {
        Task<IEnumerable<Order>> GetAllAsync();
        Task<Order> GetByNameAsync(string name, string lastName);
        Task<Order> GetByIdAsync(int id);
        Task<ICollection<Order>> GetOrdersByDoctor(Doctor doctor);
        Task CreateOrderAsync(Order order);
        Task UpdateOrderAsync(Order order);
        Task DeleteOrderAsync(int id);
        Task<bool> UpdateAsync(Order order, int id);
    }
}
