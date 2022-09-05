using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.DataAccess
{
    public class DoctorShopContext:DbContext
    {
        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<Position> Positions { get; set; }

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseInMemoryDatabase("DoctorShopContext");
		}
		protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new DoctorMapping());
            modelBuilder.ApplyConfiguration(new OrderMapping());
            modelBuilder.ApplyConfiguration(new AdminMapping());
            modelBuilder.ApplyConfiguration(new PositionMapping());
            base.OnModelCreating(modelBuilder);
        }
    }
}
