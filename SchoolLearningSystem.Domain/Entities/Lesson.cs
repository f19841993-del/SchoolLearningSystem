using System.Diagnostics;
using SchoolLearningSystem.Domain.Enums;

namespace SchoolLearningSystem.Domain.Entities
{
    public class Lesson
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        //public GradeLevel GradeLevel { get; set; }
        public string Content { get; set; } = string.Empty;
        public int CourseId { get; set; }
        public Course Course { get; set; }

        // داخل Lesson.cs
        public ICollection<Exam> Exams { get; set; } = new List<Exam>();
        public ICollection<MemorizeSession> MemorizeSessions { get; set; } = new List<MemorizeSession>();
        public ICollection<Result> Results { get; set; } = new List<Result>();
        public ICollection<Exercise> Exercises { get; set; } = new List<Exercise>();

        // 🔹 إضافة علاقة الأسئلة
        public ICollection<Question> Questions { get; set; } = new List<Question>();
    }
}
