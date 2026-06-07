using SchoolLearningSystem.Applicationf.DTOs.Question;
using SchoolLearningSystem.Applicationf.DTOs.Result;

namespace SchoolLearningSystem.Applicationf.DTOs.ExamDto
{
    public class ExamReadDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;

        // نوع الامتحان (Final, Quiz...)
        public string ExamType { get; set; } = string.Empty;

        public int CourseId { get; set; }
        public int LessonId { get; set; }

        public List<QuestionReadDto> Questions { get; set; } = new List<QuestionReadDto>();
        public List<ResultReadDto> Results { get; set; } = new List<ResultReadDto>();
    }
}
