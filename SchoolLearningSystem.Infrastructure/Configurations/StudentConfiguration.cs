using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SchoolLearningSystem.Domain.Entities;

namespace SchoolLearningSystem.Infrastructure.Configurations
{
    public class StudentConfiguration : IEntityTypeConfiguration<Student>
    {
        public void Configure(EntityTypeBuilder<Student> builder)
        {
            builder.ToTable("Students");

            // 🔹 1. إعدادات الحقول (ممتازة جداً ولا غبار عليها)
            builder.Property(s => s.Name).IsRequired().HasMaxLength(150);
            builder.Property(s => s.Username).IsRequired().HasMaxLength(100);
            builder.Property(s => s.Email).IsRequired().HasMaxLength(150);
            builder.Property(s => s.Phone).HasMaxLength(20);
            builder.Property(s => s.Address).HasMaxLength(250);
            builder.Property(s => s.Bio).HasMaxLength(500);
            builder.Property(s => s.Education).HasMaxLength(200);
            builder.Property(s => s.ProfileImage).HasMaxLength(500);

            // 🔹 2. إعدادات العلاقات (تطبيق قوانين حماية البيانات)

            // أ) العلاقة مع جلسات المراجعة: حماية التاريخ التعليمي
            builder.HasMany(s => s.MemorizeSessions)
                   .WithOne(ms => ms.Student)
                   .HasForeignKey(ms => ms.StudentId)
                   .OnDelete(DeleteBehavior.Restrict); // 👈 تعديل لحماية البيانات

            // ب) العلاقة مع النتائج: حماية النزاهة الأكاديمية
            builder.HasMany(s => s.Results)
                   .WithOne(r => r.Student)
                   .HasForeignKey(r => r.StudentId)
                   .OnDelete(DeleteBehavior.Restrict); // 👈 تعديل لحماية الدرجات

            // ج) العلاقة مع تقدم الطالب (SRS): حماية عقل الذكاء الاصطناعي
            builder.HasMany(s => s.Progresses)
                   .WithOne(p => p.Student)
                   .HasForeignKey(p => p.StudentId)
                   .OnDelete(DeleteBehavior.Restrict); // 👈 إضافة لمنع التضارب وضياع البيانات

            // د) العلاقة مع الكورسات المشترك بها (CourseStudents)
            // هنا Cascade منطقي، لأنه إذا حُذف الطالب نهائياً، فلا داعي لبقاء سجل تسجيله في الكورس
            builder.HasMany(s => s.CourseStudents)
                   .WithOne(cs => cs.Student)
                   .HasForeignKey(cs => cs.StudentId)
                   .OnDelete(DeleteBehavior.Cascade);

            // 🔹 3. الفلتر الشامل (صمام الأمان الأساسي)
            builder.HasQueryFilter(s => !s.IsDeleted);
        }
    }
}