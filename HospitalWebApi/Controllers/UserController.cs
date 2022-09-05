using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Domain.IServices;
using Domain.Repositories;
using HospitalWebApi.Models;
using Domain.Entities;

namespace HospitalWebApi.Controllers
{
    [Route("api")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IMessageCreator _messageCreator;
        private readonly IEmailSend _emailSender;

        private readonly IDoctorRepository _doctorRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly IPositionRepository _positionRepository;
        private readonly IMapper _mapper;

		public UserController(IDoctorRepository doctorRepository, IOrderRepository orderRepository, IPositionRepository positionRepository,
                              IMapper mapper, IMessageCreator messageCreator, IEmailSend emailSender)
        {
            _orderRepository = orderRepository;
            _doctorRepository = doctorRepository;
            _mapper = mapper;
            _positionRepository = positionRepository;
            _messageCreator = messageCreator;
            _emailSender = emailSender;
        }

		[HttpGet("{id:int}/doctor")]
        public async Task<IActionResult> GetDoctor([FromRoute]int id)
        {
            var doctor = await _doctorRepository.GetByIdAsync(id);
            return Ok(doctor);
        }

        [HttpGet("{name}/doctor")]
        public async Task<IActionResult> GetByNameAsync([FromRoute] string name)
        {
            var doctor = await _doctorRepository.GetByNameAsync(name);
            return Ok(doctor);
        }

        [HttpGet("position")]
        public async Task<IActionResult> GetPositions()
        {
            var position = await _positionRepository.GetPositions();
            return Ok(position);
        }

        [HttpGet("{id:int}/position")]
        public async Task<IActionResult> GetPositionsById([FromRoute] int id)
        {
            var position = await _positionRepository.GetPositionByIdAsync(id);
            return Ok(position);
        }

        [HttpGet("order")]
        public async Task<IActionResult> GetAllAsync()
        {
            var position = await _orderRepository.GetAllAsync();
            return Ok(position);
        }

        [HttpPost("position/create")]
        public async Task<IActionResult> AddPositionAsync([FromBody] PositionModel model)
        {
            var position = _mapper.Map<Position>(model);
            await _positionRepository.AddPositionAsync(position);

            return Ok();
        }

        [HttpPost("order/create")]
		public async Task<IActionResult> CreateOrder([FromBody] OrderModel model)
		{
			var order = this._mapper.Map<Order>(model);
			var doctor = await _doctorRepository.GetByIdAsync(order.Doctor_Id);
            if(doctor == null)
			{
                return NotFound("Doctor not found");
			}
			await _orderRepository.CreateOrderAsync(order);

			var message = _messageCreator.MessageCreate(order, doctor);
			//await _emailSender.SendEmail(message, order.Email);

			return Ok();
		}

        [HttpPut("{id:int}/order/edit")]
        public async Task<IActionResult> UpdateOrder([FromBody] OrderModel model, [FromRoute] int id)
        {
            var order = this._mapper.Map<Order>(model);
            var doctor = await _doctorRepository.GetByIdAsync(order.Doctor_Id);
            var res = await UpdateAsync(order, id);

            var message = _messageCreator.MessageEdit(order, doctor);
            //await _emailSender.SendEmail(message, order.Email);

            return res ? Ok() : NotFound();
        }

        private async Task<bool> UpdateAsync(Order order, int id)
        {
            var toUpdate = await _orderRepository.GetByIdAsync(id);
            if (toUpdate != null)
            {
                toUpdate.Firstname = order.Firstname;
                toUpdate.Lastname = order.Lastname;
                toUpdate.Email = order.Email;
                toUpdate.Doctor_Id = order.Doctor_Id;
                toUpdate.StartDate = order.StartDate;

                await _orderRepository.UpdateOrderAsync(toUpdate);

                return true;
            }

            return false;
        }

        [HttpDelete("{id:int}/order/delete")]
        public async Task<IActionResult> DeleteOrder([FromRoute]int id)
        {
            var toDel = await _orderRepository.GetByIdAsync(id);
            if (toDel != null)
            {
                var message = _messageCreator.MessageDelete(toDel);
                var email = toDel.Email;
                await _orderRepository.DeleteOrderAsync(id);
                
                //await _emailSender.SendEmail(message, email);

                return Ok();
            }

            return NotFound();
        }
    }
}
