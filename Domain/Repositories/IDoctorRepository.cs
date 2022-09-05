using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Domain.Repositories
{
    public interface IDoctorRepository
    {
        Task<List<Doctor>> GetAllAsync();
        Task<Doctor> GetByIdAsync(int id);
        Task<Doctor> GetByNameAsync(string name);
        Task AddDoctorAsync(Doctor doctor);
        Task DeleteByIdAsync(int id);
        Task UpdateAsync(Doctor doctor);
        Task GetByDateAsync(DateTime startDate);
    }
}
