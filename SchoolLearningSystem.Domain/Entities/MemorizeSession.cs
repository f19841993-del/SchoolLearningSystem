namespace SchoolLearningSystem.Domain.Entities
{
    public class MemorizeSession : BaseEntity
    {
        public int StudentId { get; set; }
        public Student Student { get; set; }

        public int LessonId { get; set; }
        public Lesson Lesson { get; set; }

        // يمكن أن يكون اختيارياً إذا كانت الجلسة للدرس ككل
        public int? ExerciseId { get; set; }
        public Exercise? Exercise { get; set; }

        // 🔹 حقول الخوارزمية (التي تجعل النظام يعمل مثل Memrise)
        public DateTime NextReviewDate { get; set; } // الموعد القادم للمراجعة
        public int RepetitionLevel { get; set; }     // مستوى التكرار (عدد مرات الإتقان)
        public double EaseFactor { get; set; }       // معامل السهولة (أساس الحساب)

        // 🔹 حقول التحليل
        public int Attempts { get; set; }
        public double SuccessRate { get; set; }

        // DurationInSeconds: مفيد جداً لتحليل الذكاء الاصطناعي لسرعة استيعاب الطالب
        public int DurationInSeconds { get; set; }
    }
}