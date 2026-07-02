using SchoolLearningSystem.Domain.Enums;

namespace SchoolLearningSystem.Domain.Entities
{
    public class Exercise : BaseEntity
    {
        public string Question { get; set; } = string.Empty;
        public string Answer { get; set; } = string.Empty;

        // تصنيف التمرين (مفيد لعرض التمارين بالتدريج من السهل للصعب داخل الدرس)
        public DifficultyLevel Difficulty { get; set; }

        // علاقة إجبارية مع Lesson (إذا حُذف الدرس، يُحذف التمرين)
        public int LessonId { get; set; }
        public Lesson Lesson { get; set; } = null!;

        // 💡 تم إزالة MemorizeSessions لتركيز مهام الذكاء الاصطناعي على جدول Questions فقط، 
        // لكي لا يتشتت النظام ويصبح بطيئاً.
    }
}