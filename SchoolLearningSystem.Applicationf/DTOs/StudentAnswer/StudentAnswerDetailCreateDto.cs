using System.ComponentModel.DataAnnotations;

namespace SchoolLearningSystem.Applicationf.DTOs.StudentAnswer
{
    public class StudentAnswerDetailCreateDto
    {
        public int StudentId { get; set; }
        public int QuestionId { get; set; }

        // ✅ إضافة إجبارية: FK إجباري بالـ Entity (MemorizeSession.AnswerDetails)
        public int MemorizeSessionId { get; set; }
        public string SelectedAnswer { get; set; } = string.Empty;

        // ⚠️ فكر لو الأفضل حذف هذا الحقل وجعله Computed داخل الـ Service
        // (IsCorrect = Quality >= 3) بدل استقباله جاهزاً من الفرونت.
        public bool IsCorrect { get; set; }
        public int Quality { get; set; }
        public int TimeTakenInSeconds { get; set; }
    }
}