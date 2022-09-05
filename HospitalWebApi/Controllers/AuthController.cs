using AutoMapper;
using Domain.Entities;
using Domain.Repositories;
using HospitalWebApi.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace HospitalWebApi.Controllers
{
	[Route("api")]
	[ApiController]
	public class AuthController : ControllerBase
	{
        private readonly IAdminRepository _adminRepository;
        private readonly IMapper _mapper;

		public AuthController(IAdminRepository adminRepository, IMapper mapper)
		{
			_adminRepository = adminRepository;
			_mapper = mapper;
		}

		[HttpPost("create/admin")]
        public async Task<IActionResult> CreateOrder([FromBody] AdminModel model)
        {
            var admin = this._mapper.Map<Admin>(model);
			await _adminRepository.AddAdminAsync(admin);
			return Ok();
		}
    }
}
