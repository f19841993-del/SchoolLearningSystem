namespace SchoolLearningSystem.Domain.Entities
{
    public class Lesson : BaseEntity
    {
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public int Order { get; set; }
        public string VideoUrl { get; set; } = string.Empty;

        // إضافة جديدة مطلوبة لدعم PublishLessonAsync
        public bool IsPublished { get; set; } = false;

        public int CourseId { get; set; }
        public Course Course { get; set; } = null!;

        public ICollection<Exam> Exams { get; set; } = new List<Exam>();

        // 💡 تم حذف MemorizeSessions من هنا: الجلسة لا ترتبط بدرس واحد، بل تجمع
        // أسئلة مستحقة من دروس متعددة حسب خوارزمية SM-2 (راجع MemorizeSession.cs)
        public ICollection<Result> Results { get; set; } = new List<Result>();
        public ICollection<Exercise> Exercises { get; set; } = new List<Exercise>();
        public ICollection<Question> Questions { get; set; } = new List<Question>();
    }
}