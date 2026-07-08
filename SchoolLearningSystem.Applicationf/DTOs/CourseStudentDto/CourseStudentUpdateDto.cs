namespace SchoolLearningSystem.Applicationf.DTOs.CourseStudent
{
    // ⚠️ نفس التحذير المعماري المطبّق على StudentQuestionProgressUpdateDto:
    // ProgressPercentage و LastAccessedAt هي حقول تغذي قرارات AI
    // (تحديد الطالب "المتعثر"/"المتفوق"، والتنبؤ بفقدان الاهتمام Churn Prediction).
    // يجب أن تُحسب تلقائياً بالـ Service من نشاط الطالب الفعلي (إكمال دروس،
    // دخول للكورس...)، وليس تُرسل جاهزة من الـ Client - لتفادي تلاعب يفسد
    // دقة أي تحليل مستقبلي مبني عليها.
    //
    // CourseId/StudentId غير موجودين هنا - يُفترض تمريرهما كـ route parameters
    // (المفتاح المركّب).
    public class CourseStudentUpdateDto
    {
        // الاستخدام الوحيد المشروع من الـ Client: إلغاء/إعادة تفعيل التسجيل
        public bool? IsActive { get; set; }
    }
}