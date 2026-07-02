using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SchoolLearningSystem.Domain.Entities;

namespace SchoolLearningSystem.Infrastructure.Configurations
{
    public class CurriculumConfiguration : IEntityTypeConfiguration<Curriculum>
    {
        public void Configure(EntityTypeBuilder<Curriculum> builder)
        {
            // اسم الجدول (ملاحظة: جمع Curriculum لغوياً هو Curricula، لكن Curriculums مقبول برمجياً)
            builder.ToTable("Curriculums");

            // 🔹 1. إعدادات الحقول
            builder.Property(c => c.Name)
                   .IsRequired()
                   .HasMaxLength(150);

            builder.Property(c => c.Description)
                   .HasMaxLength(1000); // 👈 لمسة معمارية: تحديد حجم الوصف لعدم استهلاك الذاكرة

            // 🔹 2. إعدادات العلاقات (حماية البيانات من الحذف العشوائي)
            builder.HasMany(c => c.Courses)
                   .WithOne(course => course.Curriculum) // تأكد أن لديك public Curriculum Curriculum {get; set;} في كلاس Course
                   .HasForeignKey(course => course.CurriculumId)
                   .OnDelete(DeleteBehavior.Restrict); // 👈 صمام الأمان: يمنع حذف المنهج إذا كان بداخله كورسات

            // 🔹 3. تطبيق فلتر الحذف المنطقي
            builder.HasQueryFilter(c => !c.IsDeleted);
        }
    }
}