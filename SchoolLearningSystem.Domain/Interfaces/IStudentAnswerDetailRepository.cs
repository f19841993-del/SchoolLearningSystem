using SchoolLearningSystem.Domain.Entities;
using SchoolLearningSystem.Domain.Interfaces.Base;

namespace SchoolLearningSystem.Domain.Interfaces
{
    public interface IStudentAnswerDetailRepository : IGenericRepository<StudentAnswerDetail>
    {
        // 1. جلب تاريخ إجابات الطالب (أساسي لبناء الـ Learning Profile)
        Task<IEnumerable<StudentAnswerDetail>> GetByStudentIdAsync(int studentId);

        // 2. جلب إجابات كل الطلاب على سؤال معين (أساسي لتحليل صعوبة السؤال)
        Task<IEnumerable<StudentAnswerDetail>> GetByQuestionIdAsync(int questionId);

        // 3. جلب آخر N إجابة للطالب (مهم لتتبع التطور اللحظي أو الـ Dashboard)
        Task<IEnumerable<StudentAnswerDetail>> GetRecentAnswersAsync(int studentId, int count);

        // 4. استعلام ذكي: جلب إجابات الطالب الخاطئة في موضوع معين (لإعادة التدريب)
        Task<IEnumerable<StudentAnswerDetail>> GetIncorrectAnswersByStudentIdAsync(int studentId, int lessonId);
    }
}