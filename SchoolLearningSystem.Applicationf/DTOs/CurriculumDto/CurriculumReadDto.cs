
using SchoolLearningSystem.Applicationf.DTOs.CourseDto;

namespace SchoolLearningSystem.Applicationf.DTOs.CurriculumDto
{
    public class CurriculumReadDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string Level { get; set; } = string.Empty;

        // قائمة الكورسات التابعة لهذا المنهج (علاقة 1 إلى متعدد)
        public List<CourseReadDto> Courses { get; set; } = new();
    }
}