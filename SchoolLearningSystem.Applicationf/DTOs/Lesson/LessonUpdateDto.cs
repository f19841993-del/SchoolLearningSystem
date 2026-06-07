namespace SchoolLearningSystem.Applicationf.DTOs.Lesson
{
    public class LessonUpdateDto
    {
        public string Title { get; set; } = string.Empty;
        public string GradeLevel { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;

        public int CourseId { get; set; }
    }
}
