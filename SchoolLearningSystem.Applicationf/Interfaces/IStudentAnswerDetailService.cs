using SchoolLearningSystem.Applicationf.DTOs.Analytics;
using SchoolLearningSystem.Applicationf.DTOs.StudentAnswer;

using SchoolLearningSystem.Applicationf.Interfaces.Base;

namespace SchoolLearningSystem.Applicationf.Interfaces
{
    public interface IStudentAnswerDetailService
        : IBaseService<StudentAnswerDetailReadDto, StudentAnswerDetailCreateDto, StudentAnswerDetailUpdateDto>
    {
        // 🔹 العمليات الأساسية (GetAll, GetById, Create, Update, Delete, GetPaged)
        // موجودة مسبقاً بفضل الوراثة من IBaseService
        // 💡 تسجيل إجابة جديدة يتم عبر CreateAsync الموروثة مباشرة - لا حاجة لدالة مخصصة

        // ==========================================
        // 🔹 بناء ملف تعلّم الطالب (Learning Profile) - قلب الذكاء الاصطناعي
        // ==========================================

        // تاريخ إجابات الطالب بالكامل
        Task<IEnumerable<StudentAnswerDetailReadDto>> GetAnswersByStudentIdAsync(int studentId);

        // كل إجابات الطلاب على سؤال معيّن - لتحليل صعوبته الفعلية
        Task<IEnumerable<StudentAnswerDetailReadDto>> GetAnswersByQuestionIdAsync(int questionId);

        // آخر N إجابة للطالب - لتتبع التطور اللحظي بلوحة التحكم
        Task<IEnumerable<StudentAnswerDetailReadDto>> GetRecentAnswersByStudentIdAsync(int studentId, int count);

        // إجابات الطالب الخاطئة ضمن درس معيّن - لإعادة التدريب المستهدف
        Task<IEnumerable<StudentAnswerDetailReadDto>> GetIncorrectAnswersByStudentIdAsync(int studentId, int lessonId);

        Task<IEnumerable<QuestionDifficultyStatsDto>> GetHardestQuestionsAsync(int? lessonId, int topN);

    }
}