using SchoolLearningSystem.Domain.Entities;
using SchoolLearningSystem.Domain.Interfaces.Base;

namespace SchoolLearningSystem.Domain.Interfaces
{
    public interface IResultRepository : IGenericRepository<Result>
    {
        // CRUD الأساسي يأتينا تلقائياً من IGenericRepository<Result>

        // استعلامات مخصصة لخدمة التحليلات (Analytics) والذكاء الاصطناعي

        // لجلب تاريخ الطالب بالكامل (مهم للـ AI لتحليل تطور الأداء)
        Task<IEnumerable<Result>> GetByStudentIdAsync(int studentId);

        // لجلب نتائج امتحان معين (مهم لتحليل صعوبة الامتحان)
        Task<IEnumerable<Result>> GetByExamIdAsync(int examId);
        Task<IEnumerable<Result>> GetByLessonIdAsync(int lessonId); // أضفنا هذه
        // لجلب متوسط درجات الطالب في الكورس (مؤشر أداء)
        Task<double> GetAverageScoreByStudentIdAsync(int studentId, int courseId);
    }
}