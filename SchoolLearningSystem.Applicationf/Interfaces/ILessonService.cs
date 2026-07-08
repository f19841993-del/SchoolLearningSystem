using SchoolLearningSystem.Applicationf.Common.Models;
using SchoolLearningSystem.Applicationf.Common.Parameters;
using SchoolLearningSystem.Applicationf.DTOs.Lesson;
using SchoolLearningSystem.Applicationf.Interfaces.Base;

namespace SchoolLearningSystem.Applicationf.Interfaces
{
    public interface ILessonService : IBaseService<LessonReadDto, LessonCreateDto, LessonUpdateDto>
    {
        // 🔹 العمليات الأساسية (GetAll, GetById, Add, Update, Delete, GetPaged)
        // موجودة مسبقاً بفضل الوراثة من IBaseService

        // ==========================================
        // 🔹 عمليات مخصصة للدرس (Business Logic)
        // ==========================================

        // 🎯 Use Case: استرجاع درس تم حذفه بالخطأ (التراجع عن Soft Delete)
        Task RestoreLessonAsync(int lessonId);

        // 🎯 Use Case: تغيير حالة الدرس إلى (منشور) ليتمكن الطلاب من رؤيته
        Task PublishLessonAsync(int lessonId);

        // 🎯 Use Case: تغيير ترتيب الدرس داخل الكورس
        Task UpdateLessonOrderAsync(int lessonId, int newOrder);

        // 🎯 Use Case: جلب الدرس المرتبط بتمرين معيّن
        Task<LessonReadDto?> GetLessonByExerciseIdAsync(int exerciseId);

        // 🎯 Use Case: جلب دروس كورس معيّن (يفيد الـ Controller مباشرة بدون المرور بالريبو)
        Task<IEnumerable<LessonReadDto>> GetLessonsByCourseIdAsync(int courseId);

        // 🎯 Use Case: جلب الدرس التالي بالتسلسل (لدعم مسار التعلم بالـ AI)
        Task<LessonReadDto?> GetNextLessonAsync(int courseId, int currentLessonOrder);
    }
}