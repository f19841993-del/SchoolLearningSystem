namespace SchoolLearningSystem.Domain.Entities
{
    public class CourseStudent
    {
        // مفاتيح الربط (تُشكل معاً المفتاح الأساسي المركب)
        public int CourseId { get; set; }
        public Course Course { get; set; } = null!;

        public int StudentId { get; set; }
        public Student Student { get; set; } = null!;

        // بيانات الاشتراك الأساسية
        public DateTime EnrolledAt { get; set; } = DateTime.UtcNow;
        public bool IsActive { get; set; } = true;

        // 💡 إضافات لخدمة الذكاء الاصطناعي (Adaptive Learning)
        // معرفة نسبة التقدم تسمح للـ AI بتحديد ما إذا كان الطالب "متعثراً" أو "متفوقاً"
        public double ProgressPercentage { get; set; } = 0.0;

        // معرفة آخر وقت دخل فيه الطالب للكورس يساعد الـ AI في التنبؤ بـ "فقدان الاهتمام" (Churn Prediction)
        public DateTime LastAccessedAt { get; set; } = DateTime.UtcNow;
    }
}