namespace SchoolLearningSystem.Applicationf.DTOs.MemorizeSession
{  //بصراحة، في أنظمة الـ SRS، السجل هو غالباً
   //Immutable (غير قابل للتعديل) لأنه يمثل حدثاً
   //تاريخياً. ولكن إذا
   //احتجت لتعديل خطأ في الإدخال، يمكنك استخدام هذا:
    public class MemorizeSessionUpdateDto
    {
        public int? Attempts { get; set; }
        public double? SuccessRate { get; set; }
    }
}