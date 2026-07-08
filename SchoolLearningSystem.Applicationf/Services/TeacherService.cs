using AutoMapper;
using SchoolLearningSystem.Applicationf.DTOs.CourseDto;
using SchoolLearningSystem.Applicationf.DTOs.Teacher;
using SchoolLearningSystem.Applicationf.Exceptions;
using SchoolLearningSystem.Applicationf.Interfaces;
using SchoolLearningSystem.Applicationf.Services.Base;
using SchoolLearningSystem.Domain.Entities;
using SchoolLearningSystem.Domain.Interfaces;

namespace SchoolLearningSystem.Applicationf.Services
{
    // ==================================================================================
    // 📌 دور هذا الـ Service:
    // يدير الملف الشخصي للمعلم (الاسم، النبذة، الصورة). أي بيانات تخص علاقة المعلم
    // بالكورسات تُجلب من ICourseRepository مباشرة (مصدر واحد للحقيقة، بدون تكرار
    // منطق "كورسات المعلم" بمكانين مختلفين - نفس المبدأ المطبّق بكل الخدمات السابقة).
    // ==================================================================================
    public class TeacherService
        : BaseService<Teacher, TeacherReadDto, TeacherCreateDto, TeacherUpdateDto>, ITeacherService
    {
        private readonly ITeacherRepository _teacherRepository;
        private readonly ICourseRepository _courseRepository;

        public TeacherService(
            ITeacherRepository teacherRepository,
            ICourseRepository courseRepository,
            IMapper mapper)
            : base(teacherRepository, mapper)
        {
            _teacherRepository = teacherRepository;
            _courseRepository = courseRepository;
        }

        // 🔹 CRUD الأساسي موروث من BaseService

        // ============================================================================
        // 🎯 Use Case: "المعلم يفتح لوحة تحكمه الشخصية ليشوف كل الكورسات التي
        //              يدرّسها حالياً، بغض النظر عن المنهج أو الصف الذي تتبع له"
        //
        // مين يستدعيها: صفحة "لوحة تحكم المعلم" الرئيسية.
        //
        // 💡 لاحظ: لا نستخدم teacher.Courses (Navigation Property) لأن GetByIdAsync
        //    لا يستخدم Include، فترجع فاضية دائماً بصمت. نستخدم ICourseRepository
        //    مباشرة، وهي مصممة أصلاً لهذا الغرض بالضبط (GetByTeacherIdAsync).
        // ============================================================================
        public async Task<IEnumerable<CourseReadDto>> GetCoursesByTeacherIdAsync(int teacherId)
        {
            var teacherExists = await _teacherRepository.GetByIdAsync(teacherId)
                ?? throw new NotFoundException($"المعلم برقم {teacherId} غير موجود.");

            var courses = await _courseRepository.GetByTeacherIdAsync(teacherId);
            return _mapper.Map<IEnumerable<CourseReadDto>>(courses);
        }
    }
}