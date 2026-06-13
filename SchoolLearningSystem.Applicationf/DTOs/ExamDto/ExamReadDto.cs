namespace SchoolLearningSystem.Applicationf.DTOs.ExamDto
{
    public class ExamReadDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;

        // بيانات الكورس المرتبط بالامتحان
        public int CourseId { get; set; }
        public string CourseTitle { get; set; } = string.Empty;

        public int DurationInMinutes { get; set; }
        public int PassingScore { get; set; }
        public DateTime StartDate { get; set; }
        public bool IsActive { get; set; } // لمعرفة هل الامتحان متاح الآن أم لا
    }
}