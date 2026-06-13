using SchoolLearningSystem.Domain.Enums;

namespace SchoolLearningSystem.Domain.Entities
{
    // أضفنا BaseEntity للتوحيد
    public class Exercise : BaseEntity
    {
        public string Question { get; set; } = string.Empty;
        public string Answer { get; set; } = string.Empty;

        // لتصنيف مستوى صعوبة التمرين، وهذا حيوي لعمل الذكاء الاصطناعي
        public DifficultyLevel Difficulty { get; set; }

        // علاقة مع Lesson
        public int LessonId { get; set; }
        public Lesson Lesson { get; set; } = null!;

        // تتبع جلسات التكرار المتباعد المرتبطة بهذا التمرين
        public ICollection<MemorizeSession> MemorizeSessions { get; set; } = new List<MemorizeSession>();
    }
}