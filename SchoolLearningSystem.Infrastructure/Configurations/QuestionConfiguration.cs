using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SchoolLearningSystem.Domain.Entities;

namespace SchoolLearningSystem.Infrastructure.Configurations
{
    public class QuestionConfiguration : IEntityTypeConfiguration<Question>
    {
        public void Configure(EntityTypeBuilder<Question> builder)
        {
            builder.ToTable("Questions");

            builder.Property(q => q.Text).IsRequired().HasMaxLength(2000);
            builder.Property(q => q.Answer).IsRequired().HasMaxLength(2000);

            // العلاقة مع الدرس: إجبارية وحذف متتالي
            builder.HasOne(q => q.Lesson)
                   .WithMany(l => l.Questions)
                   .HasForeignKey(q => q.LessonId)
                   .OnDelete(DeleteBehavior.Restrict);

            // العلاقة مع الامتحان: اختيارية (تصفير عند الحذف)
            builder.HasOne(q => q.Exam)
                   .WithMany(e => e.Questions)
                   .HasForeignKey(q => q.ExamId)
                   .OnDelete(DeleteBehavior.SetNull);

            builder.HasQueryFilter(q => !q.IsDeleted);
        }
    }
}