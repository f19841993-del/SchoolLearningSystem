using SchoolLearningSystem.Domain.Entities;

namespace SchoolLearningSystem.Domain.Interfaces
{
    public interface ILessonRepository
    {
        // CRUD الأساسي
        Task<Lesson> GetByIdAsync(int id);
        Task<IEnumerable<Lesson>> GetAllAsync();
        Task AddAsync(Lesson lesson);
        Task UpdateAsync(Lesson lesson);
        Task DeleteAsync(int id);

        // استعلام إضافي: دروس حسب الكورس
        Task<IEnumerable<Lesson>> GetByCourseIdAsync(int courseId);

        // علاقات جديدة
        Task<IEnumerable<Question>> GetQuestionsByLessonIdAsync(int lessonId);

        // إحصائيات
        Task<int> GetTotalQuestionsByLessonIdAsync(int lessonId);
        Task<int> GetTotalExamsByLessonIdAsync(int lessonId);
    }
}
