namespace SchoolLearningSystem.Applicationf.DTOs.CourseStudent
{
    public class CourseStudentUpdateDto
    {
        // نستخدم Nullable لأن المستخدم قد لا يريد تحديث كل شيء
        public DateTime? EnrollmentDate { get; set; }

        // حقل اختياري لتجميد حساب الطالب في الكورس أو تفعيله
        public bool? IsActive { get; set; }

        // يمكنك إضافة أي حقل إضافي تحتاجه مستقبلاً (مثل ProgressPercentage)
    }
}