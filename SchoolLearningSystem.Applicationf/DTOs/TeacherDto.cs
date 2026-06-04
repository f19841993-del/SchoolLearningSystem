namespace SchoolLearningSystem.Applicationf.DTOs
{
    public class TeacherDto
    {
        public int Id { get; set; }

        // اسم المدرس
        public string Name { get; set; } = string.Empty;

        // المادة الأساسية اللي يدرّسها
        public string Subject { get; set; } = string.Empty;

        // الكورسات المرتبطة بالمدرس
        public List<CourseDto> Courses { get; set; } = new List<CourseDto>();
    }
}
