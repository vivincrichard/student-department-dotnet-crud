using CRUD_Application.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CRUD_Application.Configurations
{
    public class DepartmentConfiguration : IEntityTypeConfiguration<Department>
    {
        public void Configure(EntityTypeBuilder<Department> builder)
        {
            // Table Name
            builder.ToTable("Departments");

            // Primary Key
            builder.HasKey(d => d.DepartmentId);

            // Properties
            builder.Property(d => d.DepartmentName)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(d => d.DepartmentCode)
                   .IsRequired()
                   .HasMaxLength(10);

            builder.Property(d => d.Description)
                   .HasMaxLength(250);

            // Unique Index
            builder.HasIndex(d => d.DepartmentCode)
                   .IsUnique();
        }
    }
}
