using SchoolLearningSystem.Applicationf.DTOs.Srs;
using SchoolLearningSystem.Applicationf.DTOs.StudentQuestionProgress;

namespace SchoolLearningSystem.Applicationf.Interfaces
{
    // 💡 هذا هو المصدر الوحيد بكل المشروع لمنطق خوارزمية SM-2 (SRS Engine)
    // تم دمج كل دوال IStudentQuestionProgressService هنا وحذفها (كانت مكررة).
    // لا يرث من IBaseService لأن StudentQuestionProgress بمفتاح مركّب،
    // وكل التفاعل معه منطق أعمال متخصص، مو CRUD تقليدي.
    public interface ISrsService
    {
        // ==========================================
        // 🔹 قلب محرك التكرار المتباعد (SRS Engine)
        // ==========================================

        // يعالج إجابة الطالب، يطبق خوارزمية SM-2 لحساب المستوى وموعد المراجعة القادم
        Task ProcessAnswerAsync(AnswerSubmissionDto dto);

        // يجلب الأسئلة المستحقة المراجعة الآن لطالب معيّن - تُستخدم عند بدء جلسة جديدة
        Task<IEnumerable<StudentQuestionProgressReadDto>> GetDueQuestionsForSessionAsync(int studentId, DateTime? currentDate);

        // ==========================================
        // 🔹 تحليلات ولوحات تحكم (منقولة من IStudentQuestionProgressService)
        // ==========================================

        // جلب سجل تقدم طالب معيّن بكل الأسئلة (لتحليل شامل لمستواه)
        Task<IEnumerable<StudentQuestionProgressReadDto>> GetProgressByStudentIdAsync(int studentId);

        // جلب تقدم طالب بسؤال معيّن بالتحديد
        Task<StudentQuestionProgressReadDto?> GetProgressByStudentAndQuestionAsync(int studentId, int questionId);
       
        // يبني قائمة "نقاط الضعف" لدرس معيّن: الأسئلة اللي أخطأ فيها الطالب سابقاً، مع حالتها الحالية بـ SM-2
        Task<IEnumerable<StudentQuestionProgressReadDto>> GetWeakQuestionsForLessonAsync(int studentId, int lessonId);

    }
}