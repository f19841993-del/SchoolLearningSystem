using AutoMapper;
using SchoolLearningSystem.Applicationf.DTOs;
using SchoolLearningSystem.Applicationf.Interfaces;
using SchoolLearningSystem.Domain.Entities;
using SchoolLearningSystem.Domain.Interfaces;

namespace SchoolLearningSystem.Applicationf.Services
{
    public class TeacherService : ITeacherService
    {
        private readonly ITeacherRepository _teacherRepository;
        private readonly IMapper _mapper;

        public TeacherService(ITeacherRepository teacherRepository, IMapper mapper)
        {
            _teacherRepository = teacherRepository;
            _mapper = mapper;
        }

        // العمليات الأساسية
        public async Task<IEnumerable<TeacherDto>> GetAllTeachersAsync()
        {
            var teachers = await _teacherRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<TeacherDto>>(teachers);
        }

        public async Task<TeacherDto?> GetTeacherByIdAsync(int id)
        {
            var teacher = await _teacherRepository.GetByIdAsync(id);
            return _mapper.Map<TeacherDto?>(teacher);
        }

        public async Task AddTeacherAsync(TeacherDto dto)
        {
            var entity = _mapper.Map<Teacher>(dto);
            await _teacherRepository.AddAsync(entity);
        }

        public async Task UpdateTeacherAsync(TeacherDto dto)
        {
            var entity = _mapper.Map<Teacher>(dto);
            await _teacherRepository.UpdateAsync(entity);
        }

        public async Task DeleteTeacherAsync(int id)
        {
            await _teacherRepository.DeleteAsync(id);
        }

        // علاقات إضافية
        public async Task<IEnumerable<CourseDto>> GetCoursesByTeacherIdAsync(int teacherId)
        {
            var teacher = await _teacherRepository.GetByIdAsync(teacherId);
            if (teacher == null) return Enumerable.Empty<CourseDto>();

            return _mapper.Map<IEnumerable<CourseDto>>(teacher.Courses);
        }

        public async Task<IEnumerable<LessonDto>> GetLessonsByTeacherIdAsync(int teacherId)
        {
            var teacher = await _teacherRepository.GetByIdAsync(teacherId);
            if (teacher == null) return Enumerable.Empty<LessonDto>();

            // جلب الدروس من الكورسات المرتبطة بالمدرس
            var lessons = teacher.Courses.SelectMany(c => c.Lessons).ToList();
            return _mapper.Map<IEnumerable<LessonDto>>(lessons);
        }

        // إحصائيات
        public async Task<int> GetTotalCoursesByTeacherIdAsync(int teacherId)
        {
            var teacher = await _teacherRepository.GetByIdAsync(teacherId);
            if (teacher == null) return 0;

            return teacher.Courses.Count;
        }

        public async Task<int> GetTotalLessonsByTeacherIdAsync(int teacherId)
        {
            var teacher = await _teacherRepository.GetByIdAsync(teacherId);
            if (teacher == null) return 0;

            return teacher.Courses.SelectMany(c => c.Lessons).Count();
        }
    }
}
