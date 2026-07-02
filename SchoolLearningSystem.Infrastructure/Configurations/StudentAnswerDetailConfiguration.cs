using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SchoolLearningSystem.Domain.Entities;

namespace SchoolLearningSystem.Infrastructure.Configurations
{
    public class StudentAnswerDetailConfiguration : IEntityTypeConfiguration<StudentAnswerDetail>
    {
        public void Configure(EntityTypeBuilder<StudentAnswerDetail> builder)
        {
            // 1. تحديد اسم الجدول
            builder.ToTable("StudentAnswerDetails");

            // 2. إعداد الخصائص
            builder.Property(sad => sad.SelectedAnswer)
                   .IsRequired()
                   .HasMaxLength(1000);

            // 3. إعداد العلاقات

            // أ) العلاقة مع الطالب: إجبارية ومنع الحذف (Restrict)
            // المنطق: لا يجوز مسح إجابة طالب لحماية سجلاته التاريخية حتى لو حاول أحدهم حذف الطالب فيزيائياً.
            builder.HasOne(sad => sad.Student)
                   .WithMany() // لا توجد قائمة في كيان الطالب تجنباً للضغط على الذاكرة
                   .HasForeignKey(sad => sad.StudentId)
                   .OnDelete(DeleteBehavior.Restrict);

            // ب) العلاقة مع السؤال: إجبارية ومنع الحذف (Restrict)
            // المنطق: السؤال هو أصل الإجابة، مسحه فيزيائياً سيدمر تاريخ الذكاء الاصطناعي.
            builder.HasOne(sad => sad.Question)
                   .WithMany() // لا توجد قائمة في كيان السؤال تجنباً للضغط على الذاكرة
                   .HasForeignKey(sad => sad.QuestionId)
                   .OnDelete(DeleteBehavior.Restrict);

            // ج) العلاقة مع الجلسة: إجبارية وحذف متتالي (Cascade) 👈 (تم التصحيح هنا)
            // المنطق: الإجابات هي "جزء" من الجلسة، إذا تم مسح الجلسة بالكامل، تُمحى تفاصيلها معها.
            builder.HasOne(sad => sad.MemorizeSession)
                   .WithMany(ms => ms.AnswerDetails)
                   .HasForeignKey(sad => sad.MemorizeSessionId)
                   .OnDelete(DeleteBehavior.Cascade);

            // 4. تطبيق فلتر الحذف المنطقي
            builder.HasQueryFilter(sad => !sad.IsDeleted);
        }
    }
}