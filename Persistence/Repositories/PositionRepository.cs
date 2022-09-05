using Domain.Entities;
using Domain.Repositories;
using Persistence.DataAccess;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Repositories
{
	public class PositionRepository : IPositionRepository
	{
		private readonly DoctorShopContext _doctorShopContext;

		public PositionRepository(DoctorShopContext doctorShopContext)
		{
			_doctorShopContext = doctorShopContext;
		}

		public async Task AddPositionAsync(Position position)
		{
			await _doctorShopContext.AddAsync(position);
			await _doctorShopContext.SaveChangesAsync();
		}

		public async Task<Position> GetPositionByIdAsync(int id)
		{
			return await _doctorShopContext.Positions.FirstOrDefaultAsync(x => x.Id == id);
		}

		public async Task<List<Position>> GetPositions()
		{
			return await _doctorShopContext.Positions.ToListAsync();
		}
	}
}
