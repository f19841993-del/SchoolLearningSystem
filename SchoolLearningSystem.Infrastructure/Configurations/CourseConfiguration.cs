using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SchoolLearningSystem.Domain.Entities;

namespace SchoolLearningSystem.Infrastructure.Configurations
{
    public class CourseConfiguration : IEntityTypeConfiguration<Course>
    {
        public void Configure(EntityTypeBuilder<Course> builder)
        {
            // 1. تحديد اسم الجدول (اختياري، EF يفعل ذلك تلقائياً، لكنه ممارسة جيدة)
            builder.ToTable("Courses");

            // 2. إعداد الخصائص الأساسية (Properties)
            builder.Property(c => c.Title)
                .IsRequired()
                .HasMaxLength(100); // تحديد طول النص لحماية قاعدة البيانات

            builder.Property(c => c.Description)
                .HasMaxLength(1000);

            // 3. إعداد العلاقات (Relationships)

            // علاقة المدرس مع الكورس (1:N)
            builder.HasOne(c => c.Teacher)
                .WithMany(t => t.Courses)
                .HasForeignKey(c => c.TeacherId)
                .OnDelete(DeleteBehavior.Restrict); // يمنع حذف المدرس إذا كان لديه كورسات

            // علاقة المنهج مع الكورس (1:N)
            builder.HasOne(c => c.Curriculum)
                .WithMany(curr => curr.Courses)
                .HasForeignKey(c => c.CurriculumId)
                .OnDelete(DeleteBehavior.Restrict);
          
        }
    }
}