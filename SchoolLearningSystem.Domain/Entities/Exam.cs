using SchoolLearningSystem.Domain.Enums;

namespace SchoolLearningSystem.Domain.Entities
{
    public class Exam
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public ExamType ExamType { get; set; }

        public int CourseId { get; set; }
        public Course Course { get; set; }

        // داخل Exam.cs
        public int LessonId { get; set; }
        public Lesson Lesson { get; set; }
        public ICollection<Result> Results { get; set; } = new List<Result>();

        public ICollection<Question> Questions { get; set; } = new List<Question>();
    }
}
