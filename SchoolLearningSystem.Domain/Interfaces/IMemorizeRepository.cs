using SchoolLearningSystem.Domain.Entities;
using SchoolLearningSystem.Domain.Interfaces.Base;

namespace SchoolLearningSystem.Domain.Interfaces
{
    public interface IMemorizeRepository : IGenericRepository<MemorizeSession>
    {
        // جلب الجلسة الحالية المفتوحة للطالب (لم يكملها اليوم)
        Task<MemorizeSession?> GetActiveSessionByStudentIdAsync(int studentId);

        // جلب سجل جلسات الطالب (تاريخ المراجعات) - مفيد للوحة التحكم (Dashboard)
        Task<IEnumerable<MemorizeSession>> GetSessionHistoryByStudentIdAsync(int studentId);

        // 🆕 جلب جلسة معيّنة مع كل تفاصيل إجاباتها (للمراجعة الكاملة بعد انتهاء الجلسة)
        Task<MemorizeSession?> GetSessionWithAnswersAsync(int sessionId);

        // ملاحظة: تم حذف GetByLessonIdAsync و GetByExerciseIdAsync عن قصد
        // لأن الجلسة تعتمد على "الطالب" و "اليوم"، وليس على درس معين.
        // الأسئلة داخل الجلسة قد تنتمي لدروس مختلفة بناءً على موعد استحقاقها (SRS).
    }
}