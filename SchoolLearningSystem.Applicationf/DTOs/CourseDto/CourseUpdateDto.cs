namespace SchoolLearningSystem.Applicationf.DTOs.CourseDto
{
    public class CourseUpdateDto
    {
        // نظيف تماماً! الـ Validator هو من سيفحص الطول
        public string? Title { get; set; }

        public string? Description { get; set; }

        public string? Image { get; set; }

        public int? TeacherId { get; set; }

        public int? CurriculumId { get; set; }
    }
}