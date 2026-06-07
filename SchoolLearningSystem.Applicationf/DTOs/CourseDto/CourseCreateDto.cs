namespace SchoolLearningSystem.Applicationf.DTOs.CourseDto
{
    public class CourseCreateDto
    {
        public string Title { get; set; } = string.Empty;
        public int TeacherId { get; set; }
        public int CurriculumId { get; set; }
    }
}
