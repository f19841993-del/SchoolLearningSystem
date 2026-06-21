namespace SchoolLearningSystem.Applicationf.DTOs.CourseDto
{
    // كلاس نظيف تماماً، بدون أي [Required] لأن FluentValidation سيقوم بالمهمة
    public class CourseCreateDto
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string? Image { get; set; } // Nullable كما تفضلت
        public int TeacherId { get; set; }
        public int CurriculumId { get; set; }
    }
}