using SchoolLearningSystem.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolLearningSystem.Domain.Entities
{
    //يمثل المرحلة الدراسية، وتربط الكورس به
    // ✅ إضافة كيان المنهج الدراسي
    // تمثل المنهج الدراسي الذي يضم مجموعة من الدورات(كورسات) المرتبطة بمرحلة دراسية معينة
    public class Curriculum
    {
        public int Id { get; set; }
        public GradeLevel GradeLevel { get; set; }
        public string Name { get; set; } = string.Empty; // مثلاً: "رياضيات الصف الرابع"
        public string Description { get; set; } = string.Empty; // وصف المنهج
        public ICollection<Course> Courses { get; set; } = new List<Course>();
    }
}
