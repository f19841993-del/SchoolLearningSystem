namespace SchoolLearningSystem.Domain.Entities
{
    public class MemorizeSession : BaseEntity
    {
        // 1. من هو الطالب الذي قام بهذه الجلسة؟
        public int StudentId { get; set; }
        public Student Student { get; set; }

        // 2. ما هو الدرس الذي ذاكره في هذه الجلسة؟
        public int LessonId { get; set; }
        public Lesson Lesson { get; set; }

        // يمكن أن يكون اختيارياً إذا كانت الجلسة للدرس ككل 
        public int? ExerciseId { get; set; }
        public Exercise? Exercise { get; set; }

        // 🔹 حقول التحليل للجلسة
        public int TotalAttempts { get; set; }
        public double SuccessRate { get; set; }
        public int DurationInSeconds { get; set; }
        // 🔹 حقل جديد للتحكم بحالة الجلسة
        public bool IsCompleted { get; set; } = false;

        // 💡 ملاحظة: يفضل إضافة حقل يحدد تاريخ انتهاء الجلسة للـ AI
        public DateTime? CompletedAt { get; set; }

        // 🚀 التعديل الجوهري الذي اكتشفته أنت (حلقة الوصل المفقودة):
        // هذا الحقل سيحتوي على الـ 20 إجابة التي قام بها الطالب خلال هذه الجلسة
        public ICollection<StudentAnswerDetail> AnswerDetails { get; set; } = new List<StudentAnswerDetail>();
    }
}