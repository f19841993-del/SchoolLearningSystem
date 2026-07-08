namespace SchoolLearningSystem.Applicationf.DTOs.StudentAnswer
{
    // نادراً ما سنحتاج لتعديل إجابة طالب، ولكن هذا متاح إذا لزم الأمر.
    // MemorizeSessionId غير قابل للتعديل عمداً - الإجابة تبقى مرتبطة بجلستها الأصلية دائماً.
    public class StudentAnswerDetailUpdateDto
    {
        public string? SelectedAnswer { get; set; }
        public bool? IsCorrect { get; set; }
        public int? Quality { get; set; }
    }
}