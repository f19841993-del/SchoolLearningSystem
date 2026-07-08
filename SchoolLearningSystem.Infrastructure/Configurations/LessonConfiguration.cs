using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SchoolLearningSystem.Domain.Entities;

namespace SchoolLearningSystem.Infrastructure.Configurations
{
    public class LessonConfiguration : IEntityTypeConfiguration<Lesson>
    {
        public void Configure(EntityTypeBuilder<Lesson> builder)
        {
            builder.ToTable("Lessons");

            // 🔹 1. إعداد الخصائص
            builder.Property(l => l.Title).IsRequired().HasMaxLength(150);
            builder.Property(l => l.Content).IsRequired().HasMaxLength(4000);
            builder.Property(l => l.VideoUrl).HasMaxLength(500);

            // 🔹 2. إعداد العلاقات (التي يملك الدرس مفتاحها الأجنبي)

            // علاقة الدرس بالكورس (الكل بالجزء): إذا انحذف الكورس، ينحذف الدرس
            builder.HasOne(l => l.Course)
                   .WithMany(c => c.Lessons)
                   .HasForeignKey(l => l.CourseId)
                   .OnDelete(DeleteBehavior.Cascade);

            // 🔹 3. حماية الكيانات التابعة للدرس (تدريع البيانات) 🛡️

            // 💡 ملاحظة: علاقة Lesson↔Result مُعرَّفة بالكامل بـ ResultConfiguration (SetNull)
            // 💡 ملاحظة: علاقة Lesson↔MemorizeSession تم حذفها بالكامل (راجع MemorizeSession.cs)

            // حماية بنك الأسئلة: يمنع حذف الدرس إذا كان يحتوي على أسئلة
            builder.HasMany(l => l.Questions)
                   .WithOne(q => q.Lesson)
                   .HasForeignKey(q => q.LessonId)
                   .OnDelete(DeleteBehavior.Restrict);

            // 🔹 4. الفلتر الشامل
            builder.HasQueryFilter(l => !l.IsDeleted && !l.Course.IsDeleted);
        }
    }
}