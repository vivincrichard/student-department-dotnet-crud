using CRUD_Application.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CRUD_Application.Configurations
{
    public class StudentConfiguration : IEntityTypeConfiguration<Student>
    {
        public void Configure(EntityTypeBuilder<Student> builder)
        {
            // Table Name
            builder.ToTable("Students");

            // Primary Key
            builder.HasKey(s => s.StudentId);

            // Properties
            builder.Property(s => s.Name)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(s => s.Email)
                   .IsRequired()
                   .HasMaxLength(150);

            builder.Property(s => s.Age)
                   .IsRequired();

            builder.Property(s => s.DepartmentId)
                   .IsRequired();

            // Unique Index
            builder.HasIndex(s => s.Email)
                   .IsUnique();

            // Relationship
            builder.HasOne(s => s.Department)
                   .WithMany(d => d.Students)
                   .HasForeignKey(s => s.DepartmentId)
                   .HasConstraintName("FK_Student_Department")
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
