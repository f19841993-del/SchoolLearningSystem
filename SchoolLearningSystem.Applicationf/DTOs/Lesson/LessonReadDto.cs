namespace SchoolLearningSystem.Applicationf.DTOs.Lesson
{
    public class LessonReadDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public int CourseId { get; set; }
        public string CourseTitle { get; set; } = string.Empty; // لجلب اسم الكورس مباشرة
    }
}