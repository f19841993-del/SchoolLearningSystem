using System.ComponentModel.DataAnnotations;

namespace SchoolLearningSystem.Applicationf.DTOs.Srs
{
    // يمثّل بيانات إجابة واحدة يرسلها الطالب أثناء جلسة المراجعة
    public class AnswerSubmissionDto
    {
        public int StudentId { get; set; }
        public int QuestionId { get; set; }

        // ✅ FK إجباري: StudentAnswerDetail.MemorizeSessionId - لازم يُمرَّر هنا
        // حتى يقدر SrsService.ProcessAnswerAsync يُنشئ سجل StudentAnswerDetail الكامل.
        public int MemorizeSessionId { get; set; }

        // 🆕 إضافة إجبارية: StudentAnswerDetail.SelectedAnswer إجباري بالـ Entity
        // ولم يكن موجوداً هنا سابقاً - بدونه ProcessAnswerAsync ما يقدر ينشئ
        // سجل StudentAnswerDetail الكامل رغم وجود MemorizeSessionId.
        public string SelectedAnswer { get; set; } = string.Empty;

        // جودة الإجابة من 0 إلى 5 حسب خوارزمية SM-2
        public int Quality { get; set; }

        // اختياري: الوقت المستغرق بالثواني (يفيد بتحليلات AI مستقبلية)
        public int? TimeTakenInSeconds { get; set; }

        // ❌ لا حاجة لحقل IsCorrect هنا (خلافاً لـ StudentAnswerDetailCreateDto) -
        // يُحسب تلقائياً داخل SrsService.ProcessAnswerAsync كـ (Quality >= 3)
        // بدل استقباله جاهزاً من الفرونت، لمنع أي تلاعب أو تناقض مع Quality.
    }
}