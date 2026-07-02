using System;

namespace SchoolLearningSystem.Domain.Entities
{
    // حذفنا BaseEntity هنا لأن هذا جدول ربط (Bridge Table) بمفاتيح مركبة
    public class StudentQuestionProgress
    {
        // مفاتيح الربط (سيتم تعريفها كمفتاح مركب في الـ AppDbContext)
        public int StudentId { get; set; }
        public int QuestionId { get; set; }

        // العلاقات
        public Student Student { get; set; } = null!;
        public Question Question { get; set; } = null!;

        // 🧠 حقول الذكاء الاصطناعي (SRS - SM-2 Algorithm)
        public DateTime NextReviewDate { get; set; } = DateTime.UtcNow;

        public int RepetitionLevel { get; set; } = 0; // التكرارات المتتالية الصحيحة

        public double EaseFactor { get; set; } = 2.5; // القيمة المعيارية للسهولة في SM-2

        // 🔹 التعديل: إضافة حقل الفاصل الزمني
        public int Interval { get; set; } = 0; // الفاصل الزمني الحالي بالأيام

        // 📊 حقول التحليل والإحصائيات
        public int TotalAttempts { get; set; } = 0;
        public int CorrectAttempts { get; set; } = 0;
        public DateTime LastReviewedAt { get; set; } = DateTime.UtcNow;
    }
}