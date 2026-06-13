namespace SchoolLearningSystem.Applicationf.DTOs.Teacher
{
    public class TeacherReadDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Subject { get; set; } = string.Empty;
        public string? Bio { get; set; }
        public string? ProfileImage { get; set; }
        // لجلب عناوين الكورسات مباشرة لتسهيل العرض في الـ Dashboard
        public List<string> CourseTitles { get; set; } = new List<string>();
    }
}