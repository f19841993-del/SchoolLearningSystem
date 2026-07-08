namespace SchoolLearningSystem.Applicationf.DTOs.StudentQuestionProgress
{
    // ⚠️ تحذير معماري: هذا الكيان هو "قلب الذكاء الاصطناعي" بالمشروع.
    // التعديل الطبيعي على EaseFactor/Interval/RepetitionLevel/NextReviewDate
    // يجب أن يمر حصراً عبر SrsService.ProcessAnswerAsync(AnswerSubmissionDto)
    // بناءً على جودة إجابة الطالب (Quality)، وليس عبر تعديل مباشر من الـ Client.
    //
    // هذا الـ DTO مخصص فقط لحالة استثنائية: تصحيح إداري نادر لخطأ بالبيانات.
    // - StudentId/QuestionId غير موجودين هنا عمداً؛ يُفترض تمريرهما كـ route
    //   parameters (المفتاح المركّب) وليس بالـ body.
    // - كل الحقول Nullable لدعم Partial Update بدل إجبار إرسال كل الحقول.
    // - الـ Endpoint المقابل لهذا الـ DTO يجب أن يكون محمياً بصلاحية إدارية
    //   حصراً، مثال: [Authorize(Roles = "Admin")]
    public class StudentQuestionProgressUpdateDto
    {
        public DateTime? NextReviewDate { get; set; }
        public int? RepetitionLevel { get; set; }
        public double? EaseFactor { get; set; }
        public int? Interval { get; set; }
        public int? TotalAttempts { get; set; }
        public int? CorrectAttempts { get; set; }
        public DateTime? LastReviewedAt { get; set; }
    }
}