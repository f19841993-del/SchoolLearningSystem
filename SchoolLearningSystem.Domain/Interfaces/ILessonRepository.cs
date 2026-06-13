using SchoolLearningSystem.Domain.Entities;
using SchoolLearningSystem.Domain.Interfaces.Base;

namespace SchoolLearningSystem.Domain.Interfaces
{
    // الآن أصبح الكود نظيفاً جداً، يركز فقط على "ما يميز الدرس"
    public interface ILessonRepository : IGenericRepository<Lesson>
    {
        // استعلامات مخصصة لخدمة منطق العمل والذكاء الاصطناعي
        Task<IEnumerable<Lesson>> GetByCourseIdAsync(int courseId);

        // جلب الأسئلة المرتبطة بدرس معين (ضروري للـ AI لبناء الامتحانات)
        Task<IEnumerable<Question>> GetQuestionsByLessonIdAsync(int lessonId);

        // إحصائيات (مفيدة للـ Dashboard الخاصة بالمعلم أو الطالب)
        Task<int> GetTotalQuestionsByLessonIdAsync(int lessonId);
        Task<int> GetTotalExamsByLessonIdAsync(int lessonId);
    }
}