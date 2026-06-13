using AutoMapper;
using SchoolLearningSystem.Applicationf.DTOs.CourseDto;
using SchoolLearningSystem.Applicationf.DTOs.CourseStudent;
using SchoolLearningSystem.Applicationf.DTOs.Student;
using SchoolLearningSystem.Applicationf.Interfaces;
using SchoolLearningSystem.Domain.Entities;
using SchoolLearningSystem.Domain.Interfaces;

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

        // 🔹 العمليات الأساسية (Manual CRUD)

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
            // التحقق من وجود الكورس والطالب قبل الربط
            var course = await _courseRepository.GetByIdAsync(dto.CourseId)
                ?? throw new Exception("Course not found");
            var student = await _studentRepository.GetByIdAsync(dto.StudentId)
                ?? throw new Exception("Student not found");

            var entity = _mapper.Map<CourseStudent>(dto);
            // ضبط العلاقات
            entity.Course = course;
            entity.Student = student;

            await _courseStudentRepository.AddAsync(entity);
        }

        public async Task UpdateCourseStudentAsync(int courseId, int studentId, CourseStudentUpdateDto dto)
        {
            var entity = await _courseStudentRepository.GetByIdAsync(courseId, studentId)
                ?? throw new Exception("Relation not found");

            _mapper.Map(dto, entity);
            await _courseStudentRepository.UpdateAsync(entity);
        }

        public async Task DeleteCourseStudentAsync(int courseId, int studentId)
        {
            await _courseStudentRepository.DeleteAsync(courseId, studentId);
        }

        // 🔹 علاقات إضافية (Query Logic)

        public async Task<IEnumerable<StudentReadDto>> GetStudentsByCourseIdAsync(int courseId)
        {
            var relations = await _courseStudentRepository.GetByCourseIdAsync(courseId);
            // نقوم بعمل Select لجلب الطلاب من العلاقات
            var students = relations.Select(cs => cs.Student);
            return _mapper.Map<IEnumerable<StudentReadDto>>(students);
        }

        public async Task<IEnumerable<CourseReadDto>> GetCoursesByStudentIdAsync(int studentId)
        {
            var relations = await _courseStudentRepository.GetByStudentIdAsync(studentId);
            var courses = relations.Select(cs => cs.Course);
            return _mapper.Map<IEnumerable<CourseReadDto>>(courses);
        }

        // 🔹 عمليات التسجيل والإزالة (Business Rules)

        public async Task EnrollStudentAsync(int courseId, int studentId)
        {
            // منطق إضافي للتحقق من عدم وجود تسجيل مكرر
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
        }

        public async Task RemoveStudentAsync(int courseId, int studentId)
        {
            var relation = await _courseStudentRepository.GetByIdAsync(courseId, studentId);
            if (relation == null) throw new Exception("Relation not found");

            await _courseStudentRepository.DeleteAsync(courseId, studentId);
        }

        // 🔹 إحصائيات

        public async Task<int> GetTotalStudentsByCourseIdAsync(int courseId)
        {
            var relations = await _courseStudentRepository.CountByCourseIdAsync(courseId);
            return relations;
        }

        public async Task<int> GetTotalCoursesByStudentIdAsync(int studentId)
        {
            var relations = await _courseStudentRepository.CountByStudentIdAsync(studentId);
            return relations;
        }
    }
}