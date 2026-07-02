using SchoolLearningSystem.Domain.Entities;
using SchoolLearningSystem.Domain.Interfaces.Base;
using SchoolLearningSystem.Domain.Enums; // لضمان استخدام الـ Enum بدلاً من الـ string

namespace SchoolLearningSystem.Domain.Interfaces
{
    public interface IQuestionRepository : IGenericRepository<Question>
    {
        // CRUD الأساسي يأتينا تلقائياً من IGenericRepository<Question>
        // لاحظ أننا وحدنا دالة الحذف لتكون DeleteAsync(int id) لتتوافق مع باقي المستودعات

        // استعلامات مخصصة لخدمة الـ AI والـ Exam System
        Task<IEnumerable<Question>> GetByExamIdAsync(int examId);

        // 🔹 إحصائيات مع تطوير الـ Type Safety
        Task<int> CountByExamIdAsync(int examId);

        // استخدام الـ Enum هنا يمنع الأخطاء البرمجية عند الاستعلام
        Task<int> CountByDifficultyAsync(DifficultyLevel difficultyLevel);

        // إحصائيات (مفيدة للـ Dashboard الخاصة بالمعلم أو الطالب)
        Task<int> GetTotalQuestionsByLessonIdAsync(int lessonId);

        // جلب الأسئلة المرتبطة بدرس معين (ضروري للـ AI لبناء الامتحانات)
        Task<IEnumerable<Question>> GetQuestionsByLessonIdAsync(int lessonId);
    }
}