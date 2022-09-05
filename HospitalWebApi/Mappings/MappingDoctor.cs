using AutoMapper;
using Domain.Entities;
using HospitalWebApi.Models;

namespace HospitalWebApi.Mappings
{
	public class MappingDoctor : Profile
	{
		public MappingDoctor()
		{
			CreateMap<DoctorModel, Doctor>();
			CreateMap<Doctor, DoctorModel>();
		}
	}
}
