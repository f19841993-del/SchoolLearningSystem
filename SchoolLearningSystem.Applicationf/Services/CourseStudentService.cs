using AutoMapper;
using SchoolLearningSystem.Applicationf.DTOs;
using SchoolLearningSystem.Applicationf.Interfaces;
using SchoolLearningSystem.Domain.Entities;
using SchoolLearningSystem.Domain.Interfaces;
using WebApiTemplate.Domain.Interfaces;

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

        // العمليات الأساسية
        public async Task<IEnumerable<CourseStudentDto>> GetAllCourseStudentsAsync()
        {
            var entities = await _courseStudentRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<CourseStudentDto>>(entities);
        }

        public async Task<CourseStudentDto?> GetCourseStudentByIdAsync(int courseId, int studentId)
        {
            var entity = await _courseStudentRepository.GetByIdAsync(courseId, studentId);
            return _mapper.Map<CourseStudentDto?>(entity);
        }

        public async Task AddCourseStudentAsync(CourseStudentDto dto)
        {
            var entity = _mapper.Map<CourseStudent>(dto);

            entity.Course = await _courseRepository.GetByIdAsync(dto.CourseId)
                ?? throw new Exception("Course not found");
            entity.Student = await _studentRepository.GetByIdAsync(dto.StudentId)
                ?? throw new Exception("Student not found");

            await _courseStudentRepository.AddAsync(entity);
        }

        public async Task UpdateCourseStudentAsync(CourseStudentDto dto)
        {
            var entity = _mapper.Map<CourseStudent>(dto);
            await _courseStudentRepository.UpdateAsync(entity);
        }
        public async Task DeleteCourseStudentAsync(int courseId, int studentId)
        {
            await _courseStudentRepository.DeleteAsync(courseId, studentId);
        }



        // علاقات إضافية
        public async Task<IEnumerable<StudentDto>> GetStudentsByCourseIdAsync(int courseId)
        {
            var relations = await _courseStudentRepository.GetByCourseIdAsync(courseId);
            var students = relations.Select(cs => cs.Student);
            return _mapper.Map<IEnumerable<StudentDto>>(students);
        }

        public async Task<IEnumerable<CourseDto>> GetCoursesByStudentIdAsync(int studentId)
        {
            var relations = await _courseStudentRepository.GetByStudentIdAsync(studentId);
            var courses = relations.Select(cs => cs.Course);
            return _mapper.Map<IEnumerable<CourseDto>>(courses);
        }

        // عمليات التسجيل والإزالة
        public async Task EnrollStudentAsync(int courseId, int studentId)
        {
            var course = await _courseRepository.GetByIdAsync(courseId)
                ?? throw new Exception("Course not found");
            var student = await _studentRepository.GetByIdAsync(studentId)
                ?? throw new Exception("Student not found");

            var relation = new CourseStudent
            {
                CourseId = courseId,
                StudentId = studentId,
                Course = course,
                Student = student,
                EnrolledAt = DateTime.UtcNow,
                IsActive = true
            };

            await _courseStudentRepository.AddAsync(relation);
        }

        public async Task RemoveStudentAsync(int courseId, int studentId)
        {
            var relation = await _courseStudentRepository.GetByIdAsync(courseId, studentId);
            if (relation != null)
            {
                await _courseStudentRepository.DeleteAsync(courseId, studentId);
            }
        }

        // إحصائيات
        public async Task<int> GetTotalStudentsByCourseIdAsync(int courseId)
        {
            var relations = await _courseStudentRepository.GetByCourseIdAsync(courseId);
            return relations.Count();
        }

        public async Task<int> GetTotalCoursesByStudentIdAsync(int studentId)
        {
            var relations = await _courseStudentRepository.GetByStudentIdAsync(studentId);
            return relations.Count();
        }
    }
}
