using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Repositories
{
	public interface IPositionRepository
	{
		Task<List<Position>> GetPositions();
		Task AddPositionAsync(Position position);
		Task<Position> GetPositionByIdAsync(int id);
	}
}
