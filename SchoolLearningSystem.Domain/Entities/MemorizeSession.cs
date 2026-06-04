using SchoolLearningSystem.Domain.Entities;

namespace SchoolLearningSystem.Domain.Entities
{
    public class MemorizeSession
    {
        public int Id { get; set; }
        public int StudentId { get; set; }
        public Student Student { get; set; }

        public int LessonId { get; set; }
        public Lesson Lesson { get; set; }

        public int? ExerciseId { get; set; }
        public Exercise? Exercise { get; set; }

        public int Attempts { get; set; }
        public double SuccessRate { get; set; }
        public DateTime Date { get; set; }
    }
}
