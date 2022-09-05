using AutoMapper;
using Domain.Entities;
using HospitalWebApi.Models;

namespace HospitalWebApi.Mappings
{
	public class MappingOrder : Profile
	{
		public MappingOrder()
		{
			CreateMap<Order, OrderModel>();
			CreateMap<OrderModel, Order>();
		}
	}
}
