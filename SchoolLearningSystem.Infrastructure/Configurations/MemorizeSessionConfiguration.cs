using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SchoolLearningSystem.Domain.Entities;

namespace SchoolLearningSystem.Infrastructure.Configurations
{
    public class MemorizeSessionConfiguration : IEntityTypeConfiguration<MemorizeSession>
    {
        public void Configure(EntityTypeBuilder<MemorizeSession> builder)
        {
            builder.ToTable("MemorizeSessions");

            // 🔹 1. إعداد العلاقات مع الكيانات الأساسية (حماية التاريخ التعليمي)

            // العلاقة مع الطالب: منع الحذف (لحماية إحصائيات الطالب)
            builder.HasOne(ms => ms.Student)
                   .WithMany(s => s.MemorizeSessions)
                   .HasForeignKey(ms => ms.StudentId)
                   .OnDelete(DeleteBehavior.Restrict); // 👈 تعديل هندسي

            // العلاقة مع الدرس: منع الحذف (لحماية سجل الجلسات المرتبطة بالدرس)
            builder.HasOne(ms => ms.Lesson)
                   .WithMany(l => l.MemorizeSessions)
                   .HasForeignKey(ms => ms.LessonId)
                   .OnDelete(DeleteBehavior.Restrict); // 👈 تعديل هندسي

            // العلاقة مع التمرين (اختيارية): تصفير الحقل إذا تم حذف التمرين
            builder.HasOne(ms => ms.Exercise)
                   .WithMany() // بافتراض أنك لم تضف قائمة جلسات داخل كيان التمرين
                   .HasForeignKey(ms => ms.ExerciseId)
                   .OnDelete(DeleteBehavior.SetNull); // 👈 تعامل صحيح مع الحقل الاختياري (int?)

            // 🔹 2. العلاقة مع تفاصيل الإجابات (علاقة الكل بالجزء)
            // هنا Cascade منطقي جداً، لأنه إذا حُذفت الجلسة (لسبب ما)، فمن الطبيعي أن تُحذف إجاباتها المرتبطة بها
            builder.HasMany(ms => ms.AnswerDetails)
                   .WithOne(ad => ad.MemorizeSession)
                   .HasForeignKey(ad => ad.MemorizeSessionId)
                   .OnDelete(DeleteBehavior.Cascade);

            // 🔹 3. الفلتر الشامل
            builder.HasQueryFilter(ms => !ms.IsDeleted);
        }
    }
}