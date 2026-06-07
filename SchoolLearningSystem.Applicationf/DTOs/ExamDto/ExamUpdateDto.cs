namespace SchoolLearningSystem.Applicationf.DTOs.ExamDto
{
    public class ExamUpdateDto
    {
        public string Title { get; set; } = string.Empty;
        public string ExamType { get; set; } = string.Empty;

        public int LessonId { get; set; }
    }
}
