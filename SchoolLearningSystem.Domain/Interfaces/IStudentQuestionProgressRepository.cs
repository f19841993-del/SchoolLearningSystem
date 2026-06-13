using SchoolLearningSystem.Domain.Entities;

namespace SchoolLearningSystem.Domain.Interfaces
{
    public interface IStudentQuestionProgressRepository
    {
        // 1. جلب سجل التقدم الخاص بسؤال معين لطالب معين
        Task<StudentQuestionProgress?> GetByStudentAndQuestionAsync(int studentId, int questionId);

        // 2. "قلب الـ SRS": يجلب الأسئلة التي حان موعد مراجعتها للطالب الآن
        Task<IEnumerable<StudentQuestionProgress>> GetDueQuestionsAsync(int studentId);

        // 3. جلب سجلات التقدم الخاصة بطالب معين (مفيد للتحليلات أو الـ Dashboard)
        Task<IEnumerable<StudentQuestionProgress>> GetByStudentIdAsync(int studentId);

        // 4. العمليات الأساسية (CRUD) معرفة يدوياً لتناسب المفاتيح المركبة
        Task AddAsync(StudentQuestionProgress progress);
        Task UpdateAsync(StudentQuestionProgress progress);
    }
}