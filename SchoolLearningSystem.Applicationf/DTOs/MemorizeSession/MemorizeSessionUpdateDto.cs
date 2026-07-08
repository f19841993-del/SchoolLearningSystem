namespace SchoolLearningSystem.Applicationf.DTOs.MemorizeSession
{
    // بصراحة، في أنظمة الـ SRS، السجل هو غالباً Immutable (غير قابل للتعديل)
    // لأنه يمثل حدثاً تاريخياً. هذا الـ DTO يُستخدم أساساً لإنهاء الجلسة
    // (IsCompleted / CompletedAt) أو لتصحيح خطأ إدخال نادر.
    public class MemorizeSessionUpdateDto
    {
        public double? SuccessRate { get; set; }
        public int? DurationInSeconds { get; set; }
        public bool? IsCompleted { get; set; }
    }
}