using SchoolLearningSystem.Domain.Enums;

namespace SchoolLearningSystem.Domain.Entities
{
    public class Curriculum : BaseEntity // وراثة BaseEntity لتوحيد Id و CreatedAt
    {
        public GradeLevel GradeLevel { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        // Navigation Properties
        public ICollection<Course> Courses { get; set; } = new List<Course>();
    }
}