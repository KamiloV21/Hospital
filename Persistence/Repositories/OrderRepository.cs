using Domain.Entities;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using Persistence.DataAccess;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Persistence.Repositories
{
	public class OrderRepository : IOrderRepository
    {
        private readonly DoctorShopContext _doctorShopContext;

        public OrderRepository(DoctorShopContext doctorShopContext)
        {
            _doctorShopContext = doctorShopContext;
        }

        public async Task CreateOrderAsync(Order order)
        {
            await _doctorShopContext.Orders.AddAsync(order);
            await _doctorShopContext.SaveChangesAsync();
        }

        public async Task DeleteOrderAsync(int id)
        {
            var toRemove = await _doctorShopContext.Orders.Where(x=>x.Id==id).FirstAsync();
            _doctorShopContext.Remove(toRemove);
            await _doctorShopContext.SaveChangesAsync();
        }

        public async Task<ICollection<Order>> GetOrdersByDoctor(Doctor doctor)
        {
            return await _doctorShopContext.Orders.Where(x => x.Doctor_Id == doctor.Id).Select(x => x).ToListAsync();
        }
        public async Task<IEnumerable<Order>> GetAllAsync()
        {
            return await _doctorShopContext.Orders.ToListAsync();
        }

        public async Task<Order> GetByNameAsync(string name, string lastName)
        {
            return await _doctorShopContext.Orders.Where(x => x.Firstname == name && x.Lastname == lastName).FirstAsync();
             
        }

        public async Task<Order> GetByIdAsync(int id)
        {
            return await _doctorShopContext.Orders.Where(x => x.Id == id).FirstAsync();
        } 

        public async Task UpdateOrderAsync(Order order)
        {
            _doctorShopContext.Orders.Update(order);
            await _doctorShopContext.SaveChangesAsync();
        }

        public async Task<bool> UpdateAsync(Order order, int id)
        {
            var toUpdate = await GetByIdAsync(id);
            if (toUpdate!=null)
            {
                toUpdate.Firstname = order.Firstname;
                toUpdate.Lastname = order.Lastname;
                toUpdate.Email = order.Email;
                toUpdate.Doctor_Id = order.Doctor_Id;
                toUpdate.StartDate = order.StartDate;

                await UpdateOrderAsync(toUpdate);

                return true;
            }

            return false;
        }
	}
}
