using SchoolLearningSystem.Applicationf.DTOs.CourseDto;

namespace SchoolLearningSystem.Applicationf.DTOs.Teacher
{
    public class TeacherReadDto
    {
        public int Id { get; set; }

        // اسم المدرس
        public string Name { get; set; } = string.Empty;

        // المادة الأساسية اللي يدرّسها
        public string Subject { get; set; } = string.Empty;

        // الكورسات المرتبطة بالمدرس (عرض فقط)
        public List<CourseReadDto> Courses { get; set; } = new List<CourseReadDto>();
    }
}
