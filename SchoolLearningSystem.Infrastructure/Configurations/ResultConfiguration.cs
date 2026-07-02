using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SchoolLearningSystem.Domain.Entities;

namespace SchoolLearningSystem.Infrastructure.Configurations
{
    public class ResultConfiguration : IEntityTypeConfiguration<Result>
    {
        public void Configure(EntityTypeBuilder<Result> builder)
        {
            builder.ToTable("Results");

            builder.Property(r => r.ResultType).HasMaxLength(50);

            // 1. العلاقة مع الطالب: إجبارية وحماية (Restrict)
            builder.HasOne(r => r.Student)
                   .WithMany(s => s.Results)
                   .HasForeignKey(r => r.StudentId)
                   .OnDelete(DeleteBehavior.Restrict); // يمنع حذف الطالب إذا كان له درجات تاريخية

            // 2. 💡 التعديل الجديد: العلاقة مع الدرس أصبحت اختيارية (SetNull)
            builder.HasOne(r => r.Lesson)
                   .WithMany(l => l.Results)
                   .HasForeignKey(r => r.LessonId)
                   .OnDelete(DeleteBehavior.SetNull); // 👈 إذا حُذف الدرس، لا تُمحى درجة الطالب بل تصبح قيمة الدرس Null

            // 3. العلاقة مع الامتحان: اختيارية وحماية (Restrict)
            builder.HasOne(r => r.Exam)
                   .WithMany(e => e.Results)
                   .HasForeignKey(r => r.ExamId)
                   .OnDelete(DeleteBehavior.Restrict); // يمنع حذف الامتحان إذا كان هناك طلاب قد امتحنوا وحصلوا على نتائج

            builder.HasQueryFilter(r => !r.IsDeleted);
        }
    }
}