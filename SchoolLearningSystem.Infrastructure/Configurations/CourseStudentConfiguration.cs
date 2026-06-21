using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SchoolLearningSystem.Domain.Entities;

namespace SchoolLearningSystem.Infrastructure.Configurations
{
    public class CourseStudentConfiguration : IEntityTypeConfiguration<CourseStudent>
    {
        public void Configure(EntityTypeBuilder<CourseStudent> builder)
        {
            // إعداد المفتاح المركب
            builder.HasKey(cs => new { cs.CourseId, cs.StudentId });

            // منع الحذف المتتالي (Cascade)
            builder.HasOne(cs => cs.Course)
                .WithMany(c => c.CourseStudents)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}