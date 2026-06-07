using AutoMapper;
using SchoolLearningSystem.Applicationf.DTOs.CourseDto;
using SchoolLearningSystem.Applicationf.DTOs.Lesson;
using SchoolLearningSystem.Applicationf.DTOs.Teacher;
using SchoolLearningSystem.Applicationf.Interfaces;
using SchoolLearningSystem.Domain.Entities;
using SchoolLearningSystem.Domain.Interfaces; // نفترض عندك ITeacherRepository
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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

        // 🔹 العمليات الأساسية
        public async Task<IEnumerable<TeacherReadDto>> GetAllTeachersAsync()
        {
            var teachers = await _teacherRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<TeacherReadDto>>(teachers);
        }

        public async Task<TeacherReadDto?> GetTeacherByIdAsync(int id)
        {
            var teacher = await _teacherRepository.GetByIdAsync(id);
            return _mapper.Map<TeacherReadDto?>(teacher);
        }

        public async Task AddTeacherAsync(TeacherCreateDto dto)
        {
            var entity = _mapper.Map<Teacher>(dto);

            // ربط الكورسات بالمدرس إذا CourseIds موجودة
            if (dto.CourseIds.Any())
            {
                entity.Courses = dto.CourseIds.Select(id => new Course { Id = id }).ToList();
            }

            await _teacherRepository.AddAsync(entity);
        }

        public async Task UpdateTeacherAsync(int id, TeacherUpdateDto dto)
        {
            var existing = await _teacherRepository.GetByIdAsync(id);
            if (existing != null)
            {
                _mapper.Map(dto, existing);

                // تحديث الكورسات المرتبطة
                if (dto.CourseIds.Any())
                {
                    existing.Courses = dto.CourseIds.Select(id => new Course { Id = id }).ToList();
                }

                await _teacherRepository.UpdateAsync(existing);
            }
            else
            {
                throw new KeyNotFoundException("Teacher not found");
            }
        }

        public async Task DeleteTeacherAsync(int id)
        {
            await _teacherRepository.DeleteAsync(id);
        }

        // 🔹 علاقات إضافية
        public async Task<IEnumerable<CourseReadDto>> GetCoursesByTeacherIdAsync(int teacherId)
        {
            var teacher = await _teacherRepository.GetByIdAsync(teacherId);
            if (teacher == null) return Enumerable.Empty<CourseReadDto>();

            return _mapper.Map<IEnumerable<CourseReadDto>>(teacher.Courses);
        }

        public async Task<IEnumerable<LessonReadDto>> GetLessonsByTeacherIdAsync(int teacherId)
        {
            var teacher = await _teacherRepository.GetByIdAsync(teacherId);
            if (teacher == null) return Enumerable.Empty<LessonReadDto>();

            // نفترض أن كل كورس يحتوي دروس
            var lessons = teacher.Courses.SelectMany(c => c.Lessons).ToList();
            return _mapper.Map<IEnumerable<LessonReadDto>>(lessons);
        }

        // 🔹 إحصائيات
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
