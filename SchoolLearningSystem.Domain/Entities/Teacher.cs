using System.Collections.Generic;
using SchoolLearningSystem.Domain.Entities;

namespace SchoolLearningSystem.Domain.Entities
{
    public class Teacher : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public string Subject { get; set; } = "Math";

        // الخصائص التي كانت مفقودة لتتطابق مع الـ DTO
        //Bio هي اختصار لكلمة Biography، وبالعربية تعني "السيرة الذاتية المختصرة"
        //أو "نبذة تعريفية".
        public string? Bio { get; set; }
        public string? ProfileImage { get; set; }

        public ICollection<Course> Courses { get; set; } = new List<Course>();
    }
}