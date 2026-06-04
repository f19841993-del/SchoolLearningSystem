

namespace SchoolLearningSystem.Domain.Entities
{
    public class Teacher
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Subject { get; set; } = "Math";

        public ICollection<Course> Courses { get; set; } = new List<Course>();
    }
}
