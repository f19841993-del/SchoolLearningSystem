namespace SchoolLearningSystem.Applicationf.DTOs.ExamDto
{
    public class ExamCreateDto
    {
        public string Title { get; set; } = string.Empty;
        public string ExamType { get; set; } = string.Empty;

        public int CourseId { get; set; }
        public int LessonId { get; set; }
    }
}
