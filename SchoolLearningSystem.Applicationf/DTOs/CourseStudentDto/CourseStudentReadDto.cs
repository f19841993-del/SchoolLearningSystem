namespace SchoolLearningSystem.Applicationf.DTOs.CourseStudent
{
    public class CourseStudentReadDto
    {
        public int CourseId { get; set; }
        public string CourseTitle { get; set; } = string.Empty;

        public int StudentId { get; set; }
        public string StudentName { get; set; } = string.Empty;

        public DateTime EnrollmentDate { get; set; }
    }
}
