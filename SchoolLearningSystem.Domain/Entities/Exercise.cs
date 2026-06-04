namespace SchoolLearningSystem.Domain.Entities
{
    public class Exercise
    {
        public int Id { get; set; }
        public string Question { get; set; } = string.Empty;
        public string Answer { get; set; } = string.Empty;

        // علاقة مع Lesson
        public int LessonId { get; set; }
        public Lesson Lesson { get; set; } = null!;
        public ICollection<MemorizeSession> MemorizeSessions { get; set; } = new List<MemorizeSession>();

    }
}
