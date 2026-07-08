using SchoolLearningSystem.Domain.Entities;
using SchoolLearningSystem.Domain.Enums;
using SchoolLearningSystem.Domain.Interfaces.Base;

namespace SchoolLearningSystem.Domain.Interfaces
{
    public interface IExamRepository : IGenericRepository<Exam>
    {
        // استعلام لجلب كل امتحانات كورس معين
        Task<IEnumerable<Exam>> GetByCourseIdAsync(int courseId);

        // جلب الامتحانات حسب مستوى الصعوبة لدعم الـ AI Engine
        Task<IEnumerable<Exam>> GetExamsByDifficultyAsync(DifficultyLevel difficulty);

        // 🆕 جلب كل امتحانات درس معيّن (كانت ناقصة - الانترفيس بالـ Service يحتاجها)
        Task<IEnumerable<Exam>> GetByLessonIdAsync(int lessonId);

        // جلب عدد الامتحانات لكل درس (مفيدة في لوحات تحكم المعلمين)
        Task<int> GetTotalExamsByLessonIdAsync(int lessonId);
    }
}