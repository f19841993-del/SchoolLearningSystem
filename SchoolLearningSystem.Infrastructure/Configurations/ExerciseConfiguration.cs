using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SchoolLearningSystem.Domain.Entities;

namespace SchoolLearningSystem.Infrastructure.Configurations
{
    public class ExerciseConfiguration : IEntityTypeConfiguration<Exercise>
    {
        public void Configure(EntityTypeBuilder<Exercise> builder)
        {
            builder.ToTable("Exercises");

            builder.Property(e => e.Question).IsRequired().HasMaxLength(2000);
            builder.Property(e => e.Answer).IsRequired().HasMaxLength(2000);

            // العلاقة مع الدرس: إجبارية وحذف متتالي
            builder.HasOne(e => e.Lesson)
                   .WithMany(l => l.Exercises)
                   .HasForeignKey(e => e.LessonId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasQueryFilter(e => !e.IsDeleted);
        }
    }
}