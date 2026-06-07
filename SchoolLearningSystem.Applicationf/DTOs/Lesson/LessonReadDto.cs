using SchoolLearningSystem.Applicationf.DTOs.ExamDto;
using SchoolLearningSystem.Applicationf.DTOs.Exercise;

namespace SchoolLearningSystem.Applicationf.DTOs.Lesson
{
    public class LessonReadDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;

        public string GradeLevel { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;

        public int CourseId { get; set; }
        public string CourseTitle { get; set; } = string.Empty;

        public List<ExamReadDto> Exams { get; set; } = new List<ExamReadDto>();
        public List<ExerciseReadDto> Exercises { get; set; } = new List<ExerciseReadDto>();
        public List<ResultReadDto> Results { get; set; } = new List<ResultReadDto>();
        public List<MemorizeSessionReadDto> MemorizeSessions { get; set; } = new List<MemorizeSessionReadDto>();
        public List<QuestionReadDto> Questions { get; set; } = new List<QuestionReadDto>();
    }
}
