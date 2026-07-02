using SchoolLearningSystem.Domain.Enums;

namespace SchoolLearningSystem.Domain.Entities
{
    public class Exam : BaseEntity
    {
        public string Title { get; set; } = string.Empty;
        public ExamType ExamType { get; set; }
        public DifficultyLevel Difficulty { get; set; }

        // 1. علاقة إجبارية: كل امتحان يجب أن ينتمي لفصل/كورس
        public int CourseId { get; set; }
        public Course Course { get; set; }

        // 2. علاقة اختيارية: قد يكون الامتحان لدرس محدد (كويز) أو شاملاً للفصل
        public int? LessonId { get; set; }
        public Lesson? Lesson { get; set; }

        // 3. علاقة (أب - ابن): الامتحان يحتوي على أسئلة 
        // (تذكر: حذف الامتحان لن يحذف الأسئلة بل سيجعل ExamId فيها = Null)
        public ICollection<Question> Questions { get; set; } = new List<Question>();

        // 4. بيانات حساسة: نتائج الطلاب في هذا الامتحان
        // (يجب حماية الامتحان من الحذف إذا كان يحتوي على نتائج)
        public ICollection<Result> Results { get; set; } = new List<Result>();
    }
}