using SchoolLearningSystem.Applicationf.DTOs.Lesson;

namespace SchoolLearningSystem.Applicationf.DTOs.CourseDto
{
    public class CourseReadDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string TeacherName { get; set; } = string.Empty;
        public int CurriculumId { get; set; }
        public string CurriculumTitle { get; set; } = string.Empty;
        public List<int> StudentIds { get; set; } = new();
        public List<LessonDto> Lessons { get; set; } = new();
        public List<ExamDto> Exams { get; set; } = new();
    }
}
