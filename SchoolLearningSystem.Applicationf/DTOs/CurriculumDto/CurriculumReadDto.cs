using SchoolLearningSystem.Applicationf.DTOs.CourseDto;

namespace SchoolLearningSystem.Applicationf.DTOs.Curriculum
{
    public class CurriculumReadDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string GradeLevel { get; set; } = string.Empty;

        // الكورسات المرتبطة بهذا المنهج
        public List<CourseReadDto> Courses { get; set; } = new List<CourseReadDto>();
    }
}
