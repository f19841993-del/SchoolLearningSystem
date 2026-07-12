using SchoolLearningSystem.Domain.Entities;
using SchoolLearningSystem.Domain.Interfaces.Base;

namespace SchoolLearningSystem.Domain.Interfaces
{
    public interface IResultRepository : IGenericRepository<Result>
    {
        // استعلامات مخصصة لخدمة التحليلات (Analytics) والذكاء الاصطناعي

        // لجلب تاريخ الطالب بالكامل (مهم للـ AI لتحليل تطور الأداء)
        Task<IEnumerable<Result>> GetByStudentIdAsync(int studentId);

        // لجلب نتائج امتحان معين (مهم لتحليل صعوبة الامتحان)
        Task<IEnumerable<Result>> GetByExamIdAsync(int examId);

        // لجلب نتائج درس معين
        Task<IEnumerable<Result>> GetByLessonIdAsync(int lessonId);

        // 🔹 متوسطات الدرجات (يطابق توقيع IResultService بالضبط)
        Task<double> GetAverageScoreByStudentIdAsync(int studentId);
        Task<double> GetAverageScoreByLessonIdAsync(int lessonId);
        Task<double> GetAverageScoreByExamIdAsync(int examId);
        // عدد الدروس المميزة (Distinct) اللي عند الطالب نتيجة عليها ضمن كورس معيّن — يُستخدم لحساب نسبة التقدم
        Task<int> CountDistinctCompletedLessonsAsync(int studentId, int courseId);

    }
}