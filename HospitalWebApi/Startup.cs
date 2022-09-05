using Autofac;
using Domain.IServices;
using Domain.Repositories;
using HospitalWebApi.AutheticationHandler;
using HospitalWebApi.Mappings;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Persistence.DataAccess;
using Persistence.Repositories;
using Persistence.Services;
using System.Linq;

namespace HospitalWebApi
{
	public class Startup
	{
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			services.AddAutoMapper(x =>
			{
				x.AddProfile(new MappingDoctor());
			});
			services.AddAutoMapper(x =>
			{
				x.AddProfile(new MappingAdmin());
			});
			services.AddAutoMapper(x =>
			{
				x.AddProfile(new MappingOrder());
			});
			services.AddAutoMapper(x =>
			{
				x.AddProfile(new MappingPosition());
			});

			services.AddControllers();
			services.AddSwaggerGen(c =>
			{
				c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());

				c.SwaggerDoc("v1", new OpenApiInfo { Title = "HospitalWebApi", Version = "v1" });
				c.AddSecurityDefinition("basic", new OpenApiSecurityScheme
				{
					Name = "Authorization",
					Type = SecuritySchemeType.Http,
					Scheme = "basic",
					In = ParameterLocation.Header,
					Description = "Authorization"
				});
				c.AddSecurityRequirement(new OpenApiSecurityRequirement
				{
					{
						new OpenApiSecurityScheme
						{
							Reference=new OpenApiReference
							{
								Type = ReferenceType.SecurityScheme,
								Id = "basic"
							}

						},
						new string[]{}
					}
				}) ;
			});

			services.AddAuthentication("BasicAuthentication")
				.AddScheme<AuthenticationSchemeOptions, AuthenticationHandler>("BasicAuthentication", null);
		}
		public void ConfigureContainer(ContainerBuilder builder)
		{
			builder.RegisterType<DoctorShopContext>().AsSelf();
			builder.RegisterType<OrderRepository>().As<IOrderRepository>();
			builder.RegisterType<DoctorRepository>().As<IDoctorRepository>();
			builder.RegisterType<MessageCreator>().As<IMessageCreator>();
			builder.RegisterType<EmailSender>().As<IEmailSend>();
			builder.RegisterType<PositionRepository>().As<IPositionRepository>();
			builder.RegisterType<AdminRepository>().As<IAdminRepository>();
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
				app.UseSwagger();
				app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "HospitalWebApi v1"));
			}

			app.UseRouting();
			app.UseAuthentication();
			app.UseAuthorization();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllers();
				endpoints.MapControllerRoute(
					name: "default",
					pattern: "{controller}"
					);
			});
		}
	}
}
