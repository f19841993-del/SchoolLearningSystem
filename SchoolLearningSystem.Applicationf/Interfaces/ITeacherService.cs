using SchoolLearningSystem.Applicationf.DTOs.CourseDto;
using SchoolLearningSystem.Applicationf.DTOs.Teacher;
using SchoolLearningSystem.Applicationf.Interfaces.Base;

namespace SchoolLearningSystem.Applicationf.Interfaces
{
    public interface ITeacherService : IBaseService<TeacherReadDto, TeacherCreateDto, TeacherUpdateDto>
    {
        // 🔹 العمليات الأساسية (GetAll, GetById, Create, Update, Delete, GetPaged)
        // موجودة مسبقاً بفضل الوراثة من IBaseService

        // ==========================================
        // 🔹 علاقات إضافية
        // ==========================================

        // جلب كل الكورسات التي يدرّسها معلم معيّن
        // 💡 تعتمد داخلياً على ICourseRepository.GetByTeacherIdAsync (مصدر واحد للحقيقة)
        Task<IEnumerable<CourseReadDto>> GetCoursesByTeacherIdAsync(int teacherId);
    }
}