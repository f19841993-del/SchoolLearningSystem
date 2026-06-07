using SchoolLearningSystem.Applicationf.DTOs.MemorizeSession;

namespace SchoolLearningSystem.Applicationf.DTOs
{
    public class StudentDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;

        // المرحلة الدراسية
        public string GradeLevel { get; set; } = string.Empty;

        // الكورسات المشترك بها الطالب
        public List<int> CourseIds { get; set; } = new List<int>();

        // نتائج الطالب
        public List<ResultDto> Results { get; set; } = new List<ResultDto>();

        // جلسات المراجعة الخاصة بالطالب
        public List<MemorizeSessionDto> MemorizeSessions { get; set; } = new List<MemorizeSessionDto>();
    }
}
