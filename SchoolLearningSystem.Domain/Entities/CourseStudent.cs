namespace SchoolLearningSystem.Domain.Entities
{
    public class CourseStudent
    {
        public int CourseId { get; set; }
        public Course? Course { get; set; }

        public int StudentId { get; set; }
        public Student Student { get; set; }

        // ممكن تضيف خصائص إضافية مثل تاريخ الاشتراك
        public DateTime EnrolledAt { get; set; } = DateTime.UtcNow;
        public bool IsActive { get; set; } = true;
    }
}
