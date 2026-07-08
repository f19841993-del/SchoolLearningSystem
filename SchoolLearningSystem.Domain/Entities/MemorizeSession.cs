namespace SchoolLearningSystem.Domain.Entities
{
    public class MemorizeSession : BaseEntity
    {
        // 1. من هو الطالب الذي قام بهذه الجلسة؟
        public int StudentId { get; set; }
        public Student Student { get; set; } = null!;

        // 💡 تم حذف LessonId/Lesson عن قصد: الجلسة تجمع أسئلة مستحقة (Due) من
        // دروس متعددة حسب موعد استحقاقها بخوارزمية SM-2 - لا تنتمي لدرس واحد.
        // السياق الدرسي لكل سؤال متوفر عبر: AnswerDetails → Question → LessonId

        // يمكن أن يكون اختيارياً إذا كانت الجلسة للدرس ككل
        public int? ExerciseId { get; set; }
        public Exercise? Exercise { get; set; }

        // 🔹 حقول التحليل للجلسة
        public int TotalAttempts { get; set; }
        public double SuccessRate { get; set; }
        public int DurationInSeconds { get; set; }

        // 🔹 حقل جديد للتحكم بحالة الجلسة
        public bool IsCompleted { get; set; } = false;

        public DateTime? CompletedAt { get; set; }

        // 🚀 الحقل الجوهري: الـ 20 إجابة التي قام بها الطالب خلال هذه الجلسة
        // (كل إجابة ترتبط بسؤالها، والسؤال يحمل LessonId الخاص به)
        public ICollection<StudentAnswerDetail> AnswerDetails { get; set; } = new List<StudentAnswerDetail>();
    }
}