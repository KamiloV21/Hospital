using AutoMapper;
using Domain.Entities;
using HospitalWebApi.Models;

namespace HospitalWebApi.Mappings
{
	public class MappingAdmin : Profile
	{
		public MappingAdmin()
		{
			CreateMap<AdminModel, Admin>();
			CreateMap<Admin, AdminModel>();
		}
	}
}
