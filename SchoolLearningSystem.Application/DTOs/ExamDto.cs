namespace SchoolLearningSystem.Application.DTOs
{
    public class ExamDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string ExamType { get; set; } = string.Empty;
        public int CourseId { get; set; }
    }
}
