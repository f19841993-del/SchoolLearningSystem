using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SchoolLearningSystem.Domain.Entities;

namespace SchoolLearningSystem.Infrastructure.Configurations
{
    public class CourseStudentConfiguration : IEntityTypeConfiguration<CourseStudent>
    {
        public void Configure(EntityTypeBuilder<CourseStudent> builder)
        {
            // 1. إعداد المفتاح المركب
            builder.HasKey(cs => new { cs.CourseId, cs.StudentId });

            // 2. العلاقة من جهة الكورس (الكفة الأولى)
            builder.HasOne(cs => cs.Course)
                .WithMany(c => c.CourseStudents)
                .HasForeignKey(cs => cs.CourseId) // 👈 من الجيد تحديد الـ FK صراحة
                .OnDelete(DeleteBehavior.Restrict); // يمنع حذف الكورس إذا كان فيه طلاب

            // 3. العلاقة من جهة الطالب (الكفة الثانية - الإضافة الجديدة 💡)
            builder.HasOne(cs => cs.Student)
                .WithMany(s => s.CourseStudents)
                .HasForeignKey(cs => cs.StudentId)
                .OnDelete(DeleteBehavior.Cascade); // إذا حُذف الطالب، يُحذف اشتراكه
        }
    }
}