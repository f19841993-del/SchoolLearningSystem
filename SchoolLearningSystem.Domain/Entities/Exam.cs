using SchoolLearningSystem.Domain.Enums;

namespace SchoolLearningSystem.Domain.Entities
{
    // يرث من BaseEntity للحصول على Id و CreatedAt تلقائياً
    public class Exam : BaseEntity
    {
        public string Title { get; set; } = string.Empty;
        public ExamType ExamType { get; set; }

        public int CourseId { get; set; }
        public Course Course { get; set; }

        // 💡 تعديل جوهري: هل الامتحان لدرس معين أم شامل للكورس؟
        // إذا كان الامتحان لدرس واحد، نترك هذه العلاقة، 
        // لكن الأفضل أن تكون الأسئلة هي التي ترتبط بالدرس وليس الامتحان.
        public int? LessonId { get; set; }
        public Lesson? Lesson { get; set; }
        public DifficultyLevel Difficulty { get; set; } // تأكد من إضافة هذا السطر
        public ICollection<Result> Results { get; set; } = new List<Result>();

        // الامتحان لا يحتوي على الأسئلة مباشرة، بل هو "حاوية" لمجموعة أسئلة
        public ICollection<Question> Questions { get; set; } = new List<Question>();
    }
}









//using SchoolLearningSystem.Domain.Enums;

//namespace SchoolLearningSystem.Domain.Entities
//{
//    public class Exam
//    {
//        public int Id { get; set; }
//        public string Title { get; set; } = string.Empty;
//        public ExamType ExamType { get; set; }

//        public int CourseId { get; set; }
//        public Course Course { get; set; }

//        // داخل Exam.cs
//        public int LessonId { get; set; }
//        public Lesson Lesson { get; set; }
//        public ICollection<Result> Results { get; set; } = new List<Result>();

//        public ICollection<Question> Questions { get; set; } = new List<Question>();
//    }
//}
