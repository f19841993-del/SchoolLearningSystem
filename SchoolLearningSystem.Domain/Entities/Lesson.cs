
using SchoolLearningSystem.Domain.Enums;
using SchoolLearningSystem.Domain.Entities; // تأكد من استدعاء المجلد الصحيح

namespace SchoolLearningSystem.Domain.Entities
{
    // الدرس يرث الآن من BaseEntity ليحصل على خصائص التتبع والحذف المنطقي
    public class Lesson : BaseEntity
    {
        public string Title { get; set; } = string.Empty;

        // المحتوى التعليمي (قد يكون نصاً أو رابطاً لمحتوى تفاعلي)
        public string Content { get; set; } = string.Empty;

        // علاقة الدرس بالكورس (الدرس دائماً ينتمي لكورس واحد)
        public int CourseId { get; set; }
        public Course Course { get; set; }

        // 🔹 المكونات التعليمية للدرس (العلاقات)
        public ICollection<Exam> Exams { get; set; } = new List<Exam>();
        public ICollection<MemorizeSession> MemorizeSessions { get; set; } = new List<MemorizeSession>();
        public ICollection<Result> Results { get; set; } = new List<Result>();
        public ICollection<Exercise> Exercises { get; set; } = new List<Exercise>();

        // 🔹 قلب الذكاء الاصطناعي: الأسئلة المرتبطة بهذا الدرس
        // هذا الجدول هو المصدر الذي سيستعلم منه الـ AI لتقييم مستوى الطالب
        public ICollection<Question> Questions { get; set; } = new List<Question>();
    }
}




//using System.Diagnostics;
//using SchoolLearningSystem.Domain.Enums;

//namespace SchoolLearningSystem.Domain.Entities
//{
//    public class Lesson
//    {
//        public int Id { get; set; }
//        public string Title { get; set; } = string.Empty;
//        //public GradeLevel GradeLevel { get; set; }
//        public string Content { get; set; } = string.Empty;
//        public int CourseId { get; set; }
//        public Course Course { get; set; }

//        // داخل Lesson.cs
//        public ICollection<Exam> Exams { get; set; } = new List<Exam>();
//        public ICollection<MemorizeSession> MemorizeSessions { get; set; } = new List<MemorizeSession>();
//        public ICollection<Result> Results { get; set; } = new List<Result>();
//        public ICollection<Exercise> Exercises { get; set; } = new List<Exercise>();

//        // 🔹 إضافة علاقة الأسئلة
//        public ICollection<Question> Questions { get; set; } = new List<Question>();
//    }
//}
