using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SchoolLearningSystem.Domain.Entities;

namespace SchoolLearningSystem.Infrastructure.Configurations
{
    public class StudentQuestionProgressConfiguration : IEntityTypeConfiguration<StudentQuestionProgress>
    {
        public void Configure(EntityTypeBuilder<StudentQuestionProgress> builder)
        {
            builder.ToTable("StudentQuestionProgresses");

            // 1. إعداد المفتاح المركب لمحرك الـ SRS
            builder.HasKey(sqp => new { sqp.StudentId, sqp.QuestionId });

            // 2. إعداد العلاقة مع كيان الطالب (Student)
            builder.HasOne(sqp => sqp.Student)
                   .WithMany(s => s.Progresses)
                   .HasForeignKey(sqp => sqp.StudentId)
                   .OnDelete(DeleteBehavior.Cascade); // إذا حُذف الطالب نهائياً، تُحذف سجلات تقدمه (منطقي)

            // 3. إعداد العلاقة مع كيان السؤال (Question)
            builder.HasOne(sqp => sqp.Question)
                   .WithMany(q => q.QuestionStats)
                   .HasForeignKey(sqp => sqp.QuestionId)
                   .OnDelete(DeleteBehavior.Restrict); // 👈 حماية إحصائيات الذكاء الاصطناعي من الحذف الفيزيائي للسؤال

            // 4. إعدادات إضافية لحماية قيم الخوارزمية (SM-2)
            builder.Property(sqp => sqp.EaseFactor).IsRequired();
            builder.Property(sqp => sqp.NextReviewDate).IsRequired();
            builder.Property(sqp => sqp.Interval).IsRequired();
            builder.Property(sqp => sqp.RepetitionLevel).IsRequired();
        }
    }
}