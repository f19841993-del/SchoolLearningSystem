using AutoMapper;
using SchoolLearningSystem.Applicationf.DTOs.CourseDto;
using SchoolLearningSystem.Applicationf.DTOs.Lesson;
using SchoolLearningSystem.Applicationf.DTOs.Teacher;
using SchoolLearningSystem.Applicationf.Interfaces;
using SchoolLearningSystem.Applicationf.Services.Base;
using SchoolLearningSystem.Domain.Entities;
using SchoolLearningSystem.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SchoolLearningSystem.Applicationf.Services
{
    public class TeacherService : BaseService<Teacher, TeacherReadDto, TeacherCreateDto, TeacherUpdateDto>, ITeacherService
    {
        private readonly ITeacherRepository _teacherRepository;

        public TeacherService(ITeacherRepository teacherRepository, IMapper mapper)
            : base(teacherRepository, mapper) // الأب يدير الـ CRUD
        {
            _teacherRepository = teacherRepository;
        }

      

    

        // 🔹 CRUD الأساسي: موروث من BaseService (لا حاجة لكتابته)

        // 🔹 علاقات إضافية (Business Logic)
        public async Task<IEnumerable<CourseReadDto>> GetCoursesByTeacherIdAsync(int teacherId)
        {
            // نعتمد هنا على أن الـ Repository يجلب الكيان مع علاقاته (Courses)
            var teacher = await _teacherRepository.GetByIdAsync(teacherId)
                ?? throw new Exception("Teacher not found");

            return _mapper.Map<IEnumerable<CourseReadDto>>(teacher.Courses);
        }

        public async Task<IEnumerable<LessonReadDto>> GetLessonsByTeacherIdAsync(int teacherId)
        {
            var teacher = await _teacherRepository.GetByIdAsync(teacherId)
                ?? throw new Exception("Teacher not found");

            // تسطيح البيانات: المعلم يملك كورسات، والكورسات تملك دروساً
            var lessons = teacher.Courses.SelectMany(c => c.Lessons);
            return _mapper.Map<IEnumerable<LessonReadDto>>(lessons);
        }

      

        // 🔹 إحصائيات
        public async Task<int> GetTotalCoursesByTeacherIdAsync(int teacherId)
        {
            var teacher = await _teacherRepository.GetByIdAsync(teacherId)
                ?? throw new Exception("Teacher not found");

            return teacher.Courses?.Count ?? 0;
        }

        public async Task<int> GetTotalLessonsByTeacherIdAsync(int teacherId)
        {
            var teacher = await _teacherRepository.GetByIdAsync(teacherId)
                ?? throw new Exception("Teacher not found");

            // حساب عدد الدروس الكلي بناءً على قائمة الكورسات
            return teacher.Courses?.SelectMany(c => c.Lessons).Count() ?? 0;
        }

      
    }
}