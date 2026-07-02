using SchoolLearningSystem.Domain.Enums;

namespace SchoolLearningSystem.Domain.Entities
{
    // 💡 التعديل الأول: الوراثة من BaseEntity
    public class Question : BaseEntity
    {
        // تم حذف Id لأنه موجود في الأب
        public string Text { get; set; } = string.Empty;
        public string Answer { get; set; } = string.Empty;
        public DifficultyLevel DifficultyLevel { get; set; }

        // 💡 التعديل الثاني: علاقة الامتحان أصبحت "اختيارية" (Nullable)
        public int? ExamId { get; set; }
        public Exam? Exam { get; set; }

        // علاقة الدرس "إجبارية" (كل سؤال يتبع لدرس)
        public int LessonId { get; set; }
        public Lesson Lesson { get; set; }

        // 🔹 قلب الذكاء الاصطناعي: سجل أداء الطلاب في هذا السؤال
        public ICollection<StudentQuestionProgress> QuestionStats { get; set; } = new List<StudentQuestionProgress>();
    }
}