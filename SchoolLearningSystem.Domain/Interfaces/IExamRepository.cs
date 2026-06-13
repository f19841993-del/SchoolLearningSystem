using SchoolLearningSystem.Domain.Entities;
using SchoolLearningSystem.Domain.Interfaces.Base;
using SchoolLearningSystem.Domain.Enums; // نحتاجها لدعم الـ AI

namespace SchoolLearningSystem.Domain.Interfaces
{
    public interface IExamRepository : IGenericRepository<Exam>
    {
        // استعلام مخصص لجلب امتحانات الكورس
        Task<IEnumerable<Exam>> GetByCourseIdAsync(int courseId);

        // 💡 إضافة ذكية للذكاء الاصطناعي:
        // تمكن الـ Service من جلب امتحانات ذات مستوى صعوبة محدد ليقوم الـ AI بتخصيص التقييم للطالب
        Task<IEnumerable<Exam>> GetExamsByDifficultyAsync(DifficultyLevel difficulty);
    }
}