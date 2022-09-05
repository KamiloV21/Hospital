using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.DataAccess
{
    public class DoctorMapping : IEntityTypeConfiguration<Doctor>
    {
        public void Configure(EntityTypeBuilder<Doctor> builder)
        { 
            builder.ToTable("Doctor");
            builder.HasKey(x => x.Id);

            builder.HasOne(x => x.Position)
                .WithMany(x => x.Doctors)
                .HasForeignKey(x => x.Position_Id);
        }
    }
}
