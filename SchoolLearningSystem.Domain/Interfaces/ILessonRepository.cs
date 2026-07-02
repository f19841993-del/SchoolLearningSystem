using SchoolLearningSystem.Domain.Entities;
using SchoolLearningSystem.Domain.Interfaces.Base;

namespace SchoolLearningSystem.Domain.Interfaces
{
    // الآن أصبح الكود نظيفاً جداً، يركز فقط على "ما يميز الدرس"
    public interface ILessonRepository : IGenericRepository<Lesson>
    {
        // استعلامات مخصصة لخدمة منطق العمل والذكاء الاصطناعي
        Task<IEnumerable<Lesson>> GetByCourseIdAsync(int courseId);

      

      
        
    }
}