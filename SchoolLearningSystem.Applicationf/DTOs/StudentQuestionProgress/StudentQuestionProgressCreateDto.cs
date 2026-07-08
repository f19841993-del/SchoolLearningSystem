namespace SchoolLearningSystem.Applicationf.DTOs.StudentQuestionProgress
{
    public class StudentQuestionProgressCreateDto
    {
        public int StudentId { get; set; }
        public int QuestionId { get; set; }

        // لا نحتاج لـ NextReviewDate/EaseFactor/Interval هنا -
        // السيرفر يحسبها تلقائياً كقيم بداية (SM-2 defaults).
    }
}