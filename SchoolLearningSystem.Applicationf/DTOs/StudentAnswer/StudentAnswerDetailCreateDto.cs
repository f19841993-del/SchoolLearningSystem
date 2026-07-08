using System.ComponentModel.DataAnnotations;

namespace SchoolLearningSystem.Applicationf.DTOs.StudentAnswer
{
    public class StudentAnswerDetailCreateDto
    {
        [Required]
        public int StudentId { get; set; }

        [Required]
        public int QuestionId { get; set; }

        // ✅ إضافة إجبارية: FK إجباري بالـ Entity (MemorizeSession.AnswerDetails)
        [Required]
        public int MemorizeSessionId { get; set; }

        [Required]
        public string SelectedAnswer { get; set; } = string.Empty;

        // ⚠️ فكر لو الأفضل حذف هذا الحقل وجعله Computed داخل الـ Service
        // (IsCorrect = Quality >= 3) بدل استقباله جاهزاً من الفرونت.
        [Required]
        public bool IsCorrect { get; set; }

        [Required]
        [Range(0, 5, ErrorMessage = "Quality must be between 0 and 5.")]
        public int Quality { get; set; }

        [Required]
        [Range(1, 3600, ErrorMessage = "Time must be between 1 second and 1 hour")]
        public int TimeTakenInSeconds { get; set; }
    }
}