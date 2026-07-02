using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SchoolLearningSystem.Domain.Entities;

namespace SchoolLearningSystem.Infrastructure.Configurations
{
    public class ExamConfiguration : IEntityTypeConfiguration<Exam>
    {
        public void Configure(EntityTypeBuilder<Exam> builder)
        {
            builder.ToTable("Exams");

            builder.Property(e => e.Title).IsRequired().HasMaxLength(150);

            // العلاقة مع الكورس: إجبارية وحذف متتالي
            builder.HasOne(e => e.Course)
                   .WithMany(c => c.Exams)
                   .HasForeignKey(e => e.CourseId)
                   .OnDelete(DeleteBehavior.Cascade);

            // العلاقة مع الدرس: اختيارية (تصفير عند الحذف)
            builder.HasOne(e => e.Lesson)
                   .WithMany(l => l.Exams)
                   .HasForeignKey(e => e.LessonId)
                   .OnDelete(DeleteBehavior.NoAction);

            // العلاقة مع النتائج: منع الحذف (لحماية نتائج الطلاب)
            builder.HasMany(e => e.Results)
                   .WithOne(r => r.Exam)
                   .HasForeignKey(r => r.ExamId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasQueryFilter(e => !e.IsDeleted);
        }
    }
}