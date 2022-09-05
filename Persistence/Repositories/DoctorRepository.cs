using Domain.Entities;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using Persistence.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Persistence.Repositories
{
    public class DoctorRepository : IDoctorRepository
    {
        private readonly DoctorShopContext _doctorShopContext;

        public DoctorRepository(DoctorShopContext doctorShopContext)
        {
            _doctorShopContext = doctorShopContext;
        }

        public async Task AddDoctorAsync(Doctor doctor)
        {
            await _doctorShopContext.AddAsync(doctor);
            await _doctorShopContext.SaveChangesAsync();
        }

        public async Task DeleteByIdAsync(int id)
        {
            var toRemove = await _doctorShopContext.Doctors.FirstOrDefaultAsync(x => x.Id == id);
            _doctorShopContext.Doctors.Remove(toRemove);
            await _doctorShopContext.SaveChangesAsync();
        }

        public async Task<List<Doctor>> GetAllAsync()
        {
            return await _doctorShopContext.Doctors.ToListAsync();
        }

        public async Task<Doctor> GetByIdAsync(int id)
        {
            return await _doctorShopContext.Doctors.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Doctor> GetByNameAsync(string name)
        {
            return await _doctorShopContext.Doctors.FirstOrDefaultAsync(x => x.Name == name);
        }

        public async Task UpdateAsync(Doctor doctor)
        {
            _doctorShopContext.Doctors.Update(doctor);
            await _doctorShopContext.SaveChangesAsync();
        }

		Task IDoctorRepository.GetByDateAsync(DateTime startDate)
		{
			throw new NotImplementedException();
		}
	}
}
