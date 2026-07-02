using SchoolLearningSystem.Domain.Entities;
using SchoolLearningSystem.Domain.Interfaces.Base;

namespace SchoolLearningSystem.Domain.Interfaces
{
    public interface IMemorizeRepository : IGenericRepository<MemorizeSession>
    {
        // 1. جلب الجلسة الحالية المفتوحة للطالب (إذا كان لديه جلسة لم يكملها اليوم)
        Task<MemorizeSession?> GetActiveSessionByStudentIdAsync(int studentId);

        // 2. جلب سجل جلسات الطالب (تاريخ المراجعات) - مفيد للوحة التحكم (Dashboard)
        Task<IEnumerable<MemorizeSession>> GetSessionHistoryByStudentIdAsync(int studentId);

        // ملاحظة: تم حذف GetByLessonIdAsync و GetByExerciseIdAsync 
        // لأن الجلسة تعتمد على "الطالب" و "اليوم"، وليس على درس معين.
        // الأسئلة داخل الجلسة هي التي قد تنتمي لدروس مختلفة بناءً على موعد استحقاقها.
    }
}