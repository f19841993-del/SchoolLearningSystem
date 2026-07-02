namespace SchoolLearningSystem.Domain.Entities
{
    public class StudentAnswerDetail : BaseEntity
    {
        // 1. علاقة الطالب
        public int StudentId { get; set; }
        public Student Student { get; set; } = null!;

        // 2. علاقة السؤال
        public int QuestionId { get; set; }
        public Question Question { get; set; } = null!;

        // 💡 التعديل المعماري: ربط الإجابة بالجلسة التي تمت فيها
        public int MemorizeSessionId { get; set; }
        public MemorizeSession MemorizeSession { get; set; } = null!;

        // تفاصيل الإجابة
        public string SelectedAnswer { get; set; } = string.Empty;
        public bool IsCorrect { get; set; }

        // 🌟 الخصائص الذهبية للذكاء الاصطناعي (SM-2)

        // جودة الإجابة من 0 إلى 5
        public int Quality { get; set; }

        // الوقت المستغرق في التفكير والحل
        public int TimeTakenInSeconds { get; set; }

        // ❌ تم حذف Timestamp لأن BaseEntity يوفر خاصية CreatedAt تلقائياً
    }
}