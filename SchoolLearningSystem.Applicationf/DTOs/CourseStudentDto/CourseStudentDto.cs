namespace SchoolLearningSystem.Applicationf.DTOs
{
    public class CourseStudentDto
    {
        public int CourseId { get; set; }
        public string CourseTitle { get; set; } = string.Empty;

        public int StudentId { get; set; }
        public string StudentName { get; set; } = string.Empty;

        // ممكن تضيف معلومات إضافية مثل تاريخ الانضمام
        public DateTime EnrollmentDate { get; set; }
    }
}
