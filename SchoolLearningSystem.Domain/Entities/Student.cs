using System.Diagnostics;
using SchoolLearningSystem.Domain.Entities;
using SchoolLearningSystem.Domain.Enums;

namespace SchoolLearningSystem.Domain.Entities
{
    public class Student
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string Bio { get; set; } = string.Empty;
        public string Education { get; set; } = string.Empty;
        public string ProfileImage { get; set; } = string.Empty;

        public GradeLevel GradeLevel { get; set; }
        public ICollection<MemorizeSession> MemorizeSessions { get; set; } = new List<MemorizeSession>();

        public ICollection<CourseStudent> CourseStudents { get; set; } = new List<CourseStudent>();

        public ICollection<StudentQuestionProgress> Progresses { get; set; }
        public ICollection<Result> Results { get; set; } = new List<Result>();
    }
}
