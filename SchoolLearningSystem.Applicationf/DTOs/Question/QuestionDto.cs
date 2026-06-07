namespace SchoolLearningSystem.Applicationf.DTOs.Question
{
    public class QuestionDto
    {
        public int Id { get; set; }

        // نص السؤال
        public string Text { get; set; } = string.Empty;

        // الإجابة الصحيحة
        public string Answer { get; set; } = string.Empty;

        // مستوى الصعوبة (مثلاً: Easy, Medium, Hard)
        public string DifficultyLevel { get; set; } = string.Empty;

        // الامتحان المرتبط
        public int ExamId { get; set; }
        public string ExamTitle { get; set; } = string.Empty;

        // ترتيب السؤال داخل الامتحان (اختياري)
        public int QuestionNumber { get; set; }
    }
}
