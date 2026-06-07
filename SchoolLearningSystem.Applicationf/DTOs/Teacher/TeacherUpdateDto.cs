namespace SchoolLearningSystem.Applicationf.DTOs.Teacher
{
    public class TeacherUpdateDto
    {
        // اسم المدرس
        public string Name { get; set; } = string.Empty;

        // المادة الأساسية اللي يدرّسها
        public string Subject { get; set; } = string.Empty;

        // IDs للكورسات المرتبطة بالمدرس
        public List<int> CourseIds { get; set; } = new List<int>();
    }
}
