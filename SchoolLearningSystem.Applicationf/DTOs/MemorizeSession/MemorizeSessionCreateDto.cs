using System.ComponentModel.DataAnnotations;

namespace SchoolLearningSystem.Applicationf.DTOs.MemorizeSession
{
    public class MemorizeSessionCreateDto
    {
        [Required]
        public int StudentId { get; set; }

        // اختياري: الجلسة قد تكون مرتبطة بتمرين تدريبي محدد، أو جلسة SRS عامة
        public int? ExerciseId { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Duration cannot be negative")]
        public int DurationInSeconds { get; set; }

        // 💡 ملاحظة معمارية: لا يوجد هنا Attempts/SuccessRate/TotalAttempts —
        // هذه حقول محسوبة (Aggregated) من AnswerDetails.Count() ومعدل النجاح،
        // يجب أن يحسبها SrsService/MemorizeService تلقائياً بعد إضافة الإجابات،
        // وليس أن يرسلها الـ Client جاهزة.
    }
}