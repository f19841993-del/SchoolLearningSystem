using AutoMapper;
using SchoolLearningSystem.Application.Common.Models;
using SchoolLearningSystem.Application.Common.Parameters;
using SchoolLearningSystem.Applicationf.DTOs.CourseDto;
using SchoolLearningSystem.Applicationf.DTOs.CourseStudent;
using SchoolLearningSystem.Applicationf.DTOs.Student;
using SchoolLearningSystem.Applicationf.Interfaces;
using SchoolLearningSystem.Domain.Entities;
using SchoolLearningSystem.Domain.Interfaces;
using SchoolLearningSystem.Domain.Interfaces.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SchoolLearningSystem.Applicationf.Services
{
    public class CourseStudentService : ICourseStudentService
    {
        private readonly ICourseStudentRepository _courseStudentRepository;
        private readonly ICourseRepository _courseRepository;
        private readonly IStudentRepository _studentRepository;


        private readonly IMapper _mapper;

        public CourseStudentService(
            ICourseStudentRepository courseStudentRepository,
            ICourseRepository courseRepository,
            IStudentRepository studentRepository,
            IMapper mapper)
        {
            _courseStudentRepository = courseStudentRepository;
            _courseRepository = courseRepository;
            _studentRepository = studentRepository;
            _mapper = mapper;
        }

        #region 1. العمليات الأساسية (CRUD)

        public async Task<IEnumerable<CourseStudentReadDto>> GetAllCourseStudentsAsync()
        {
            var entities = await _courseStudentRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<CourseStudentReadDto>>(entities);
        }


        public async Task<CourseStudentReadDto?> GetCourseStudentByIdAsync(int courseId, int studentId)
        {
            var entity = await _courseStudentRepository.GetByIdAsync(courseId, studentId);
            return _mapper.Map<CourseStudentReadDto?>(entity);
        }

        public async Task AddCourseStudentAsync(CourseStudentCreateDto dto)
        {
            var course = await _courseRepository.GetByIdAsync(dto.CourseId) ?? throw new Exception("Course not found");
            var student = await _studentRepository.GetByIdAsync(dto.StudentId) ?? throw new Exception("Student not found");

            var entity = _mapper.Map<CourseStudent>(dto);
            entity.Course = course;
            entity.Student = student;

            await _courseStudentRepository.AddAsync(entity);
            //await _courseStudentRepository.SaveChangesAsync();
        }

        public async Task UpdateCourseStudentAsync(int courseId, int studentId, CourseStudentUpdateDto dto)
        {
            var entity = await _courseStudentRepository.GetByIdAsync(courseId, studentId) ?? throw new Exception("Relation not found");

            _mapper.Map(dto, entity);
            await _courseStudentRepository.UpdateAsync(entity);
            //await _courseStudentRepository.SaveChangesAsync();
        }

        public async Task DeleteCourseStudentAsync(int courseId, int studentId)
        {
            await _courseStudentRepository.DeleteAsync(courseId, studentId);
            //await _courseStudentRepository.SaveChangesAsync();
        }

        #endregion

        #region 2. علاقات إضافية (Query Logic)

        public async Task<IEnumerable<StudentReadDto>> GetStudentsByCourseIdAsync(int courseId)
        {
            var relations = await _courseStudentRepository.GetByCourseIdAsync(courseId);
            var students = relations.Select(cs => cs.Student);
            return _mapper.Map<IEnumerable<StudentReadDto>>(students);
        }

        public async Task<IEnumerable<CourseReadDto>> GetCoursesByStudentIdAsync(int studentId)
        {
            var relations = await _courseStudentRepository.GetByStudentIdAsync(studentId);
            var courses = relations.Select(cs => cs.Course);
            return _mapper.Map<IEnumerable<CourseReadDto>>(courses);
        }

        // 🔹 الدالة الأولى: جلب طلاب كورس معين مع الترقيم
        public async Task<PagedList<StudentReadDto>> GetPagedStudentsByCourseIdAsync(int courseId, QueryParameters parameters)
        {
            // 1. استدعاء الدالة السريعة من المستودع (ترسل الـ Id وأرقام الصفحات فقط)
            var result = await _courseStudentRepository.GetPagedByCourseIdAsync(
                courseId,
                parameters.PageNumber,
                parameters.PageSize);

            // 2. النتيجة (result.Items) هي علاقات (CourseStudent). نحن نحتاج استخراج (Student) منها
            var students = result.Items.Select(cs => cs.Student);

            // 3. التحويل من Student إلى StudentReadDto
            var itemsDto = _mapper.Map<IEnumerable<StudentReadDto>>(students);

            // 4. التغليف النهائي في PagedList وإرجاعها
            return new PagedList<StudentReadDto>(
                itemsDto,
                result.TotalCount,
                parameters.PageNumber,
                parameters.PageSize);
        }

        // 🔹 الدالة الثانية: جلب كورسات طالب معين مع الترقيم
        public async Task<PagedList<CourseReadDto>> GetPagedCoursesByStudentIdAsync(int studentId, QueryParameters parameters)
        {
            // 1. استدعاء الدالة السريعة من المستودع
            var result = await _courseStudentRepository.GetPagedByStudentIdAsync(
                studentId,
                parameters.PageNumber,
                parameters.PageSize);

            // 2. استخراج (Course) من جدول الربط (CourseStudent)
            var courses = result.Items.Select(cs => cs.Course);

            // 3. التحويل من Course إلى CourseReadDto
            var itemsDto = _mapper.Map<IEnumerable<CourseReadDto>>(courses);

            // 4. التغليف النهائي
            return new PagedList<CourseReadDto>(
                itemsDto,
                result.TotalCount,
                parameters.PageNumber,
                parameters.PageSize);
        }

        #endregion

        #region 3. عمليات التسجيل والإزالة (Business Rules)

        public async Task EnrollStudentAsync(int courseId, int studentId)
        {
            var exists = await _courseStudentRepository.GetByIdAsync(courseId, studentId);
            if (exists != null) throw new Exception("Student is already enrolled in this course");

            var course = await _courseRepository.GetByIdAsync(courseId) ?? throw new Exception("Course not found");
            var student = await _studentRepository.GetByIdAsync(studentId) ?? throw new Exception("Student not found");

            var relation = new CourseStudent
            {
                CourseId = courseId,
                StudentId = studentId,
                EnrolledAt = DateTime.UtcNow,
                IsActive = true
            };

            await _courseStudentRepository.AddAsync(relation);
            //await _courseStudentRepository.SaveChangesAsync();
        }

        public async Task RemoveStudentAsync(int courseId, int studentId)
        {
            var relation = await _courseStudentRepository.GetByIdAsync(courseId, studentId) ?? throw new Exception("Relation not found");
            await _courseStudentRepository.DeleteAsync(courseId, studentId);
            //await _courseStudentRepository.SaveChangesAsync();
        }

        #endregion

        #region 4. إحصائيات (Statistics)

        public async Task<int> GetTotalStudentsByCourseIdAsync(int courseId)
        {
            return await _courseStudentRepository.CountByCourseIdAsync(courseId);
        }

        public async Task<int> GetTotalCoursesByStudentIdAsync(int studentId)
        {
            return await _courseStudentRepository.CountByStudentIdAsync(studentId);
        }

        #endregion
    }
}