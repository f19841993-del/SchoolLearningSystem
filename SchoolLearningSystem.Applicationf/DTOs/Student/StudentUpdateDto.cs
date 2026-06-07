namespace SchoolLearningSystem.Applicationf.DTOs.Student
{
    public class StudentUpdateDto
    {
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;

        // المرحلة الدراسية
        public string GradeLevel { get; set; } = string.Empty;

        // الكورسات المشترك بها الطالب
        public List<int> CourseIds { get; set; } = new List<int>();
    }
}
