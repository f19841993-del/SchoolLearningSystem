using SchoolLearningSystem.Domain.Enums;

namespace SchoolLearningSystem.Applicationf.DTOs.Curriculum
{
    public class CurriculumReadDto
    {
        public int Id { get; set; }
        public GradeLevel GradeLevel { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        // عدّاد كورسات المنهج - موثّق كـ Use Case بـ CurriculumService
        public int CoursesCount { get; set; }
    }
}