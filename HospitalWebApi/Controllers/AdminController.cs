using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Collections.Generic;
using Domain.Repositories;
using HospitalWebApi.Models;
using Domain.Entities;

namespace HospitalWebApi.Controllers
{
    [Authorize]
    [Route("api/admin")]
    [ApiController]
    
    public class AdminController : ControllerBase
    {

        private readonly IDoctorRepository _doctorRepository;
        private readonly IMapper _mapper;

        public AdminController(IDoctorRepository doctorRepository, IMapper mapper)
        {
            _doctorRepository = doctorRepository;
            _mapper = mapper;
        }

        [HttpGet("doctors")]
        public async Task<IActionResult> UpdateAsync()
        {
            var doctors = await _doctorRepository.GetAllAsync();
            var doctorModels = new List<DoctorModel>();
            foreach (var doctor in doctors)
            {
                doctorModels.Add(_mapper.Map<DoctorModel>(doctor));
            }
            return Ok(doctorModels);
        }

        [HttpPost("doctor/create")]
        public async Task<IActionResult> AddDoctor([FromBody]DoctorModel model)
        {
            var doctor = _mapper.Map<Doctor>(model);
            await _doctorRepository.AddDoctorAsync(doctor);

            return Ok();
        }

        [HttpDelete("{id:int}/doctor/delete")]
        public async Task<IActionResult> DeleteDoctor([FromRoute] int id)
        {
            if(await _doctorRepository.GetByIdAsync(id) == null)
			{
                return NotFound("Doctor not found");
			}
            await _doctorRepository.DeleteByIdAsync(id);

            return Ok();
        }
    }
}
