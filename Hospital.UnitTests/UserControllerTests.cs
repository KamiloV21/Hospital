using AutoMapper;
using Domain.Entities;
using Domain.IServices;
using Domain.Repositories;
using FluentAssertions;
using HospitalWebApi.Controllers;
using HospitalWebApi.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System.Net;
using System.Threading.Tasks;

namespace Hospital.UnitTests
{
	[TestFixture]
	public class UserControllerTests
	{
        private IMapper SetMapper()
        {
            var mapper = new Mock<IMapper>();

            mapper.Setup(x => x.Map<OrderModel>(It.IsAny<Order>()))
                .Returns((Order order) =>
                {
                    return new OrderModel
                    {
                        Firstname = order.Firstname,
                        Lastname = order.Lastname,
                        Doctor_Id = order.Doctor_Id,
                        Email = order.Email,
                        StartDate = order.StartDate
                    };
                });
            mapper.Setup(x => x.Map<Order>(It.IsAny<OrderModel>()))
               .Returns((OrderModel order) =>
               {
                   return new Order
                   {
                       Firstname = order.Firstname,
                       Lastname = order.Lastname,
                       Doctor_Id = order.Doctor_Id,
                       Email = order.Email,
                       StartDate = order.StartDate
                   };
               });
            mapper.Setup(x => x.Map<DoctorModel>(It.IsAny<Doctor>()))
               .Returns((Doctor doctor) =>
               {
                   return new DoctorModel
                   {
                       Name = doctor.Name,
                       Years = doctor.Years,
                       Id = doctor.Id,
                       Position_Id = doctor.Position_Id
                   };
               });
            mapper.Setup(x => x.Map<Doctor>(It.IsAny<DoctorModel>()))
               .Returns((Doctor doctor) =>
               {
                   return new Doctor
                   {
                       Name = doctor.Name,
                       Years = doctor.Years,
                       Id = doctor.Id,
                       Position_Id = doctor.Position_Id,
                   };
               });

            return mapper.Object;
        }
        [Test]
        public async Task GetDoctorByIdTest()
        {
            var doctorRepo = new Mock<IDoctorRepository>();
            doctorRepo.Setup(x => x.GetByIdAsync(1)).ReturnsAsync(new Doctor
            {
                Id = 1,
                Name = "Max"
            });

            var orderRepo = new Mock<IOrderRepository>();
            var posRepo = new Mock<IPositionRepository>();
            var emailSender = new Mock<IEmailSend>();
            var messageCreator = new Mock<IMessageCreator>();
            var mapper = SetMapper();

            var userController = new UserController(doctorRepo.Object, orderRepo.Object, posRepo.Object, mapper, messageCreator.Object, emailSender.Object);


            var result = await userController.GetDoctor(1);
            var okObject = result as OkObjectResult;

            okObject.StatusCode.Should().Be((int)HttpStatusCode.OK);
            okObject.Value.Should().BeEquivalentTo(new DoctorModel
            {
                Id = 1,
                Name = "Max"
            });
        }

        [Test]
        public async Task GetDoctorByName()
        {
            //Arrange
            var doctorRepo = new Mock<IDoctorRepository>();
            doctorRepo.Setup(x => x.GetByNameAsync("Max")).ReturnsAsync(new Doctor
            {
                Name = "Max"
            });

            var orderRepo = new Mock<IOrderRepository>();
            var messageCreator = new Mock<IMessageCreator>();
            var posRepo = new Mock<IPositionRepository>();
            var emailSender = new Mock<IEmailSend>();
            var mapper = SetMapper();

            var userController = new UserController(doctorRepo.Object, orderRepo.Object, posRepo.Object, mapper, messageCreator.Object, emailSender.Object);

            //Act
            var result = await userController.GetByNameAsync("Max");
            var okObject = result as OkObjectResult;

            //Assert
            okObject.StatusCode.Should().Be((int)HttpStatusCode.OK);
            okObject.Value.Should().BeEquivalentTo(new Doctor
            {
                Name = "Max"
            });
        }
        [Test]
        public async Task CreateOrder_DoctorNotFoundTest()
        {
            var orderModel = new OrderModel
            {
                Id = 1,
                Firstname = "",
                Lastname = "",
                Doctor_Id = 1,
                Email = "kalinm270603@gmail.com",
            };
            

            var doctorRepo = new Mock<IDoctorRepository>();
            doctorRepo.Setup(x => x.GetByIdAsync(1));
            var orderRepo = new Mock<IOrderRepository>();
            /*doctorRepo.Setup(x => x.GetByDateAsync(orderModel.StartDate))
                .ReturnsAsync(new List<Doctor>());*/
            var posRepo = new Mock<IPositionRepository>();
            var emailSender = new Mock<IEmailSend>();
            var messageCreator = new Mock<IMessageCreator>();
            var mapper = SetMapper();

            var userController = new UserController(doctorRepo.Object, orderRepo.Object, posRepo.Object, mapper, messageCreator.Object, emailSender.Object);


            var result = await userController.CreateOrder(orderModel);
            var notFoundResult = result as NotFoundObjectResult;

            notFoundResult.StatusCode.Should().Be((int)HttpStatusCode.NotFound);
            notFoundResult.Value.Should().Be($"Doctor not found");
        }

        [Test]
        public async Task CreateOrder_OkResult()
        {
            var order = new Order
            {
                Id = 1,
                Firstname = "",
                Lastname = "",
                Doctor_Id = 1,
                Email = "kalinm270603@gmail.com",
            };
            var doctor = new Doctor
            {
                Id = 1
            };
            var mapper = SetMapper();
            var doctorRepo = new Mock<IDoctorRepository>();
            doctorRepo.Setup(x => x.GetByIdAsync(1))
                .ReturnsAsync(doctor);
            var orderRepo = new Mock<IOrderRepository>();
            orderRepo.Setup(x => x.CreateOrderAsync(order));
            /*doctorRepo.Setup(x => x.GetByDateAsync(orderModel.StartDate))
                .ReturnsAsync(new List<Doctor>
                {
                    doctor
                });*/
            var posRepo = new Mock<IPositionRepository>();
            var emailSender = new Mock<IEmailSend>();
            var messageCreator = new Mock<IMessageCreator>();
            messageCreator.Setup(x => x.MessageCreate(order, doctor)).Returns("");
			emailSender.Setup(x => x.SendEmail("", order.Email));

			var userController = new UserController(doctorRepo.Object, orderRepo.Object, posRepo.Object, mapper, messageCreator.Object, emailSender.Object);


            var result = await userController.CreateOrder(mapper.Map<OrderModel>(order));
            var okResult = result as OkResult;

            okResult.StatusCode.Should().Be((int)HttpStatusCode.OK);
        }


        [Test]
        public async Task UpdateOrder_DoctorNotFound()
        {
            var order = new Order
            {
                Id = 1,
                Firstname = "",
                Lastname = "",
                Doctor_Id = 1,
                Email = "kalinm270603@gmail.com",
            };
            var doctor = new Doctor
            {
                Id = 1
            };

            var doctorRepo = new Mock<IDoctorRepository>();
            var orderRepo = new Mock<IOrderRepository>();
            var posRepo = new Mock<IPositionRepository>();
            var emailSender = new Mock<IEmailSend>();
            /*doctorRepo.Setup(x => x.GetByDateAsync(order.StartDate))
                .ReturnsAsync(new List<Doctor>
                {
                    doctor
                });*/
            orderRepo.Setup(x => x.GetByIdAsync(order.Id));
            orderRepo.Setup(x => x.UpdateOrderAsync(order));
            var messageCreator = new Mock<IMessageCreator>();
            var mapper = SetMapper();

            var userController = new UserController(doctorRepo.Object, orderRepo.Object, posRepo.Object, mapper, messageCreator.Object, emailSender.Object);

            var result = await userController.UpdateOrder(mapper.Map<OrderModel>(order), order.Id);
            var notFoundResult = result as NotFoundResult;

            notFoundResult.StatusCode.Should().Be((int)HttpStatusCode.NotFound);
        }

        [Test]
        public async Task UpdateOrder_WrongOrderId()
        {
            var order = new Order
            {
                Id = 1,
                Firstname = "",
                Lastname = "",
                Doctor_Id = 1,
                Email = "kalinm270603@gmail.com",
            };
            var doctor = new Doctor
            {
                Id = 1
            };

            var doctorRepo = new Mock<IDoctorRepository>();
            var orderRepo = new Mock<IOrderRepository>();
            /*doctorRepo.Setup(x => x.GetByDateAsync(order.StartDate))
                .ReturnsAsync(new List<Doctor>
                {
                    doctor
                });*/
            orderRepo.Setup(x => x.GetByIdAsync(1));

            var messageCreator = new Mock<IMessageCreator>();
            var posRepo = new Mock<IPositionRepository>();
            var emailSender = new Mock<IEmailSend>();
            var mapper = SetMapper();

            var userController = new UserController(doctorRepo.Object, orderRepo.Object, posRepo.Object, mapper, messageCreator.Object, emailSender.Object);

            var result = await userController.UpdateOrder(mapper.Map<OrderModel>(order), 2);
            var notFoundResult = result as NotFoundResult;

            notFoundResult.StatusCode.Should().Be((int)HttpStatusCode.NotFound);

        }

        [Test]
        public async Task UpdateOrder_OkResult()
        {
            var order = new Order
            {
                Id = 1,
                Firstname = "",
                Lastname = "",
                Doctor_Id = 1,
                Email = "kalinm270603@gmail.com",
            };
            var doctor = new Doctor
            {
                Id = 1
            };

            var doctorRepo = new Mock<IDoctorRepository>();
            var orderRepo = new Mock<IOrderRepository>();
            var posRepo = new Mock<IPositionRepository>();
            var emailSender = new Mock<IEmailSend>();
            /*doctorRepo.Setup(x => x.GetByDateAsync(order.StartDate))
                .ReturnsAsync(new List<Doctor>
                {
                    doctor
                });*/
            orderRepo.Setup(x => x.GetByIdAsync(order.Id)).
                ReturnsAsync(order);
            var messageCreator = new Mock<IMessageCreator>();
            var mapper = SetMapper();
            messageCreator.Setup(x => x.MessageEdit(order, doctor)).Returns("");
            emailSender.Setup(x => x.SendEmail("", order.Email));

            var userController = new UserController(doctorRepo.Object, orderRepo.Object, posRepo.Object, mapper, messageCreator.Object, emailSender.Object);

            var result = await userController.UpdateOrder(mapper.Map<OrderModel>(order), order.Id);
            var okResult = result as OkResult;

            okResult.StatusCode.Should().Be((int)HttpStatusCode.OK);

        }

        [Test]
        public async Task DeleteOrder_WrongOrderId()
        {
            var doctorRepo = new Mock<IDoctorRepository>();
            var orderRepo = new Mock<IOrderRepository>();
            orderRepo.Setup(x => x.GetByIdAsync(1));

            var posRepo = new Mock<IPositionRepository>();
            var emailSender = new Mock<IEmailSend>();
            emailSender.Setup(x => x.SendEmail("", "kalinm270603@gmail.com"));
            var messageCreator = new Mock<IMessageCreator>();
            messageCreator.Setup(x => x.MessageDelete(new Order
            {

            })).Returns("");
            var mapper = SetMapper();

            var userController = new UserController(doctorRepo.Object, orderRepo.Object, posRepo.Object, mapper, messageCreator.Object, emailSender.Object);

            var result = await userController.DeleteOrder(2);
            var notFoundResult = result as NotFoundResult;

            notFoundResult.StatusCode.Should().Be((int)HttpStatusCode.NotFound);

        }

        [Test]
        public async Task DeleteOrder_OkResult()
        {
            var order = new Order
            {
                Id = 1,
                Firstname = "",
                Lastname = "",
                Doctor_Id = 1,
                Email = "kalinm270603@gmail.com",
            };
            var doctorRepo = new Mock<IDoctorRepository>();
            var orderRepo = new Mock<IOrderRepository>();
            var posRepo = new Mock<IPositionRepository>();
            var emailSender = new Mock<IEmailSend>();
            orderRepo.Setup(x => x.GetByIdAsync(order.Id)).
                ReturnsAsync(order);
            var messageCreator = new Mock<IMessageCreator>();
            var mapper = SetMapper();

            var userController = new UserController(doctorRepo.Object, orderRepo.Object, posRepo.Object, mapper, messageCreator.Object, emailSender.Object);

            var result = await userController.DeleteOrder(order.Id);
            var okResult = result as OkResult;

            okResult.StatusCode.Should().Be((int)HttpStatusCode.OK);
        }
    }
}
