

namespace SchoolLearningSystem.Domain.Entities
{
    public class Course : BaseEntity // 💡 إضافة الوراثة هنا
    {
        // Id حذفناه لأنه موجود في BaseEntity
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Image { get; set; } = string.Empty;

        public int TeacherId { get; set; }
        public Teacher Teacher { get; set; }

        public int CurriculumId { get; set; }
        public Curriculum Curriculum { get; set; }

        public ICollection<Lesson> Lessons { get; set; } = new List<Lesson>();
        public ICollection<CourseStudent> CourseStudents { get; set; } = new List<CourseStudent>();
        public ICollection<Exam> Exams { get; set; } = new List<Exam>();
    }
}





//namespace SchoolLearningSystem.Domain.Entities
//{
//    public class Course
//    {
//        public int Id { get; set; }
//        public string Title { get; set; } = string.Empty;
//        public string Description { get; set; } = string.Empty;
//        public string Image { get; set; } = string.Empty;

//        public int TeacherId { get; set; }
//        public Teacher Teacher { get; set; }

//        public int CurriculumId { get; set; } // ✅ ربط بالمرحلة
//        public Curriculum Curriculum { get; set; }
//        public ICollection<Lesson> Lessons { get; set; } = new List<Lesson>();
//        public ICollection<CourseStudent> CourseStudents { get; set; } = new List<CourseStudent>();

//        public ICollection<Exam> Exams { get; set; } = new List<Exam>();
//    }
//}
