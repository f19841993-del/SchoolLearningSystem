using SchoolLearningSystem.Applicationf.DTOs.MemorizeSession;
using SchoolLearningSystem.Applicationf.DTOs.Result;

namespace SchoolLearningSystem.Applicationf.DTOs.Student
{
    public class StudentReadDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;

        // المرحلة الدراسية
        public string GradeLevel { get; set; } = string.Empty;

        // الكورسات المشترك بها الطالب
        public List<int> CourseIds { get; set; } = new List<int>();

        // نتائج الطالب
        public List<ResultReadDto> Results { get; set; } = new List<ResultReadDto>();

        // جلسات المراجعة الخاصة بالطالب
        public List<MemorizeSessionReadDto> MemorizeSessions { get; set; } = new List<MemorizeSessionReadDto>();
    }
}
