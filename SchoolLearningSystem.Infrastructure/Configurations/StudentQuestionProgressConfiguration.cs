using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SchoolLearningSystem.Domain.Entities;

namespace SchoolLearningSystem.Infrastructure.Configurations
{
    public class StudentQuestionProgressConfiguration : IEntityTypeConfiguration<StudentQuestionProgress>
    {
        public void Configure(EntityTypeBuilder<StudentQuestionProgress> builder)
        {
            // إعداد المفتاح المركب لمحرك الـ SRS
            builder.HasKey(sqp => new { sqp.StudentId, sqp.QuestionId });
        }
    }
}