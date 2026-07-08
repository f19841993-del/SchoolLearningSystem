using System.ComponentModel.DataAnnotations;

namespace SchoolLearningSystem.Applicationf.DTOs.Srs
{
    // يمثّل بيانات إجابة واحدة يرسلها الطالب أثناء جلسة المراجعة
    public class AnswerSubmissionDto
    {
        [Required]
        public int StudentId { get; set; }

        [Required]
        public int QuestionId { get; set; }

        // ✅ إضافة إجبارية: StudentAnswerDetail.MemorizeSessionId هو FK إجباري
        // بالـ Entity - لازم يُمرَّر هنا حتى يقدر SrsService.ProcessAnswerAsync
        // يُنشئ سجل StudentAnswerDetail الكامل بدون فشل.
        [Required]
        public int MemorizeSessionId { get; set; }

        // جودة الإجابة من 0 إلى 5 حسب خوارزمية SM-2
        [Range(0, 5)]
        public int Quality { get; set; }

        // اختياري: الوقت المستغرق بالثواني (يفيد بتحليلات AI مستقبلية)
        public int? TimeTakenInSeconds { get; set; }
    }
}