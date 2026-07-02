using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SchoolLearningSystem.Domain.Entities;

namespace SchoolLearningSystem.Infrastructure.Configurations
{
    public class CourseConfiguration : IEntityTypeConfiguration<Course>
    {
        public void Configure(EntityTypeBuilder<Course> builder)
        {
            // 1. تحديد اسم الجدول
            builder.ToTable("Courses");

            // 2. إعداد الخصائص الأساسية (Properties)
            builder.Property(c => c.Title)
                .IsRequired()
                .HasMaxLength(200); // 200 حرف كافية جداً لعنوان الكورس

            builder.Property(c => c.Description)
                .HasMaxLength(2000); // الوصف قد يكون طويلاً، 2000 حرف مناسبة

            builder.Property(c => c.Image)
                .HasMaxLength(500); // 👈 اللمسة الهندسية: تقييد طول مسار الصورة (URL)

            // (اختياري) يمكنك جعل قيمة افتراضية للترتيب إذا أردت
            builder.Property(c => c.Order)
                .HasDefaultValue(0);

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
                .OnDelete(DeleteBehavior.Restrict); // يمنع حذف المنهج إذا كان لديه كورسات

            // 4. تجاوز الكورسات المحذوفة من الاستعلامات (Soft Delete)
            builder.HasQueryFilter(c => !c.IsDeleted);
        }
    }
}