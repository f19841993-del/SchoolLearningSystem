using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SchoolLearningSystem.Domain.Entities;

namespace SchoolLearningSystem.Infrastructure.Configurations
{
    public class TeacherConfiguration : IEntityTypeConfiguration<Teacher>
    {
        public void Configure(EntityTypeBuilder<Teacher> builder)
        {
            builder.ToTable("Teachers");

            builder.Property(t => t.Name).IsRequired().HasMaxLength(150);
            builder.Property(t => t.Subject).HasMaxLength(50);
            builder.Property(t => t.Bio).HasMaxLength(1000);
            builder.Property(t => t.ProfileImage).HasMaxLength(500);

            // العلاقة مع الكورسات: منع حذف المدرس إذا كان لديه كورسات مرتبطة
            builder.HasMany(t => t.Courses)
                   .WithOne(c => c.Teacher)
                   .HasForeignKey(c => c.TeacherId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasQueryFilter(t => !t.IsDeleted);
        }
    }
}