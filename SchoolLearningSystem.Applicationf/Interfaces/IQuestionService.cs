using SchoolLearningSystem.Applicationf.DTOs.Question;
using SchoolLearningSystem.Applicationf.Interfaces.Base;
using SchoolLearningSystem.Domain.Enums;

namespace SchoolLearningSystem.Applicationf.Interfaces
{
    public interface IQuestionService : IBaseService<QuestionReadDto, QuestionCreateDto, QuestionUpdateDto>
    {
        // 🔹 العمليات الأساسية (GetAll, GetById, Create, Update, Delete, GetPaged)
        // موجودة مسبقاً بفضل الوراثة من IBaseService

        // ==========================================
        // 🔹 علاقات إضافية
        // ==========================================

        // جلب أسئلة امتحان معيّن (تُستخدم فعلياً من داخل ExamService.GetQuestionsByExamIdAsync)
        Task<IEnumerable<QuestionReadDto>> GetQuestionsByExamIdAsync(int examId);

        // جلب كل الأسئلة العامة لدرس معيّن (تدريب حر، غير مرتبطة بامتحان محدد)
        Task<IEnumerable<QuestionReadDto>> GetQuestionsByLessonIdAsync(int lessonId);

        // ==========================================
        // 🔹 دعم الذكاء الاصطناعي (AI Support)
        // ==========================================

        // جلب أسئلة حسب مستوى صعوبة معيّن - يستخدمها محرك الـ AI لبناء اختبار تكيفي
        Task<IEnumerable<QuestionReadDto>> GetQuestionsByDifficultyAsync(DifficultyLevel difficulty);

        // ==========================================
        // 🔹 إحصائيات
        // ==========================================

        Task<int> CountByExamIdAsync(int examId);
        Task<int> CountByDifficultyAsync(DifficultyLevel difficulty);
        Task<int> GetTotalQuestionsByLessonIdAsync(int lessonId);
    }
}