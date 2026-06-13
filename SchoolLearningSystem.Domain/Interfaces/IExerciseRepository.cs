using SchoolLearningSystem.Domain.Entities;
using SchoolLearningSystem.Domain.Interfaces.Base;
using SchoolLearningSystem.Domain.Enums; // لضمان استخدام الـ Enum إذا احتجناه

namespace SchoolLearningSystem.Domain.Interfaces
{
    public interface IExerciseRepository : IGenericRepository<Exercise>
    {
        // CRUD الأساسي (GetById, Add, Update, Delete) تأتينا تلقائياً من IGenericRepository

        // استعلام مخصص لخدمة الـ AI (جلب التمارين الخاصة بدرس معين)
        Task<IEnumerable<Exercise>> GetByLessonIdAsync(int lessonId);

        // 💡 إضافة ذكية للذكاء الاصطناعي:
        // جلب تمارين حسب مستوى الصعوبة (ضروري لتوليد مسارات تعليمية مخصصة)
        Task<IEnumerable<Exercise>> GetByDifficultyAsync(DifficultyLevel difficulty);
    }
}