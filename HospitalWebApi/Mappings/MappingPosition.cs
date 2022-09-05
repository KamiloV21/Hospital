using AutoMapper;
using Domain.Entities;
using HospitalWebApi.Models;

namespace HospitalWebApi.Mappings
{
	public class MappingPosition : Profile
	{
		public MappingPosition()
		{
			CreateMap<Position, PositionModel>();
			CreateMap<PositionModel, Position>();
		}
	}
}
