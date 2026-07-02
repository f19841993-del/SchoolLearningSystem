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

            // علاقة الدرس بالكورس (الكل بالجزء): إذا انحذف الكورس، ينحذف الدرس (بشرط ألا يمتلك الدرس طلاباً)
            builder.HasOne(l => l.Course)
                   .WithMany(c => c.Lessons)
                   .HasForeignKey(l => l.CourseId)
                   .OnDelete(DeleteBehavior.Cascade);

            // 🔹 3. حماية الكيانات التابعة للدرس (تدريع البيانات) 🛡️

            // حماية النتائج: يمنع حذف الدرس إذا كان له نتائج
            builder.HasMany(l => l.Results)
                   .WithOne(r => r.Lesson)
                   .HasForeignKey(r => r.LessonId)
                   .OnDelete(DeleteBehavior.Restrict);

            // حماية جلسات المراجعة (AI Data): يمنع حذف الدرس إذا ذاكره الطلاب
            builder.HasMany(l => l.MemorizeSessions)
                   .WithOne(ms => ms.Lesson)
                   .HasForeignKey(ms => ms.LessonId)
                   .OnDelete(DeleteBehavior.Restrict);

            // حماية بنك الأسئلة: يمنع حذف الدرس إذا كان يحتوي على أسئلة
            builder.HasMany(l => l.Questions)
                   .WithOne(q => q.Lesson)
                   .HasForeignKey(q => q.LessonId)
                   .OnDelete(DeleteBehavior.Restrict);

            // علاقة الدرس بالتمارين اللحظية (كما كتبتها أنت، ويفضل لاحقاً نقلها لـ ExerciseConfiguration)
            //builder.HasMany(l => l.Exercises)
            //       .WithOne(e => e.Lesson)
            //       .HasForeignKey(e => e.LessonId)
            //       .OnDelete(DeleteBehavior.Cascade); // التمرين اللحظي يُحذف بحذف الدرس (منطقي جداً)

            // 🔹 4. الفلتر الشامل
            builder.HasQueryFilter(l => !l.IsDeleted);
        }
    }
}