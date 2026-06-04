using AutoMapper;
using SchoolLearningSystem.Applicationf.DTOs;
using SchoolLearningSystem.Applicationf.Interfaces;
using SchoolLearningSystem.Domain.Entities;
using SchoolLearningSystem.Domain.Interfaces;
using WebApiTemplate.Domain.Interfaces;

namespace SchoolLearningSystem.Applicationf.Services
{
    public class CourseService : ICourseService
    {
        private readonly ICourseRepository _courseRepository;
        private readonly IStudentRepository _studentRepository;
        private readonly ILessonRepository _lessonRepository;
        private readonly IExamRepository _examRepository;
        private readonly IMapper _mapper;

        public CourseService(
            ICourseRepository courseRepository,
            IStudentRepository studentRepository,
            ILessonRepository lessonRepository,
            IExamRepository examRepository,
            IMapper mapper)
        {
            _courseRepository = courseRepository;
            _studentRepository = studentRepository;
            _lessonRepository = lessonRepository;
            _examRepository = examRepository;
            _mapper = mapper;
        }

        // العمليات الأساسية
        public async Task<IEnumerable<CourseDto>> GetAllCoursesAsync()
        {
            var courses = await _courseRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<CourseDto>>(courses);
        }

        public async Task<CourseDto?> GetCourseByIdAsync(int id)
        {
            var course = await _courseRepository.GetByIdAsync(id);
            return _mapper.Map<CourseDto?>(course);
        }

        public async Task AddCourseAsync(CourseDto dto, int teacherId)
        {
            var entity = _mapper.Map<Course>(dto);
            entity.TeacherId = teacherId;
            await _courseRepository.AddAsync(entity);
        }

        public async Task UpdateCourseAsync(CourseDto dto)
        {
            var entity = _mapper.Map<Course>(dto);
            await _courseRepository.UpdateAsync(entity);
        }

        public async Task DeleteCourseAsync(int id)
        {
            await _courseRepository.DeleteAsync(id);
        }

        // علاقات إضافية
        public async Task<IEnumerable<StudentDto>> GetStudentsByCourseIdAsync(int courseId)
        {
            var course = await _courseRepository.GetByIdAsync(courseId);
            if (course == null) return Enumerable.Empty<StudentDto>();

            var students = course.CourseStudents.Select(cs => cs.Student);
            return _mapper.Map<IEnumerable<StudentDto>>(students);
        }

        public async Task<IEnumerable<LessonDto>> GetLessonsByCourseIdAsync(int courseId)
        {
            var lessons = await _lessonRepository.GetByCourseIdAsync(courseId);
            return _mapper.Map<IEnumerable<LessonDto>>(lessons);
        }

        public async Task<IEnumerable<ExamDto>> GetExamsByCourseIdAsync(int courseId)
        {
            var exams = await _examRepository.GetByCourseIdAsync(courseId);
            return _mapper.Map<IEnumerable<ExamDto>>(exams);
        }

        // ربط طالب بالكورس
        public async Task EnrollStudentAsync(int courseId, int studentId)
        {
            var course = await _courseRepository.GetByIdAsync(courseId);
            var student = await _studentRepository.GetByIdAsync(studentId);

            if (course == null || student == null)
                throw new Exception("Course or Student not found");

            course.CourseStudents.Add(new CourseStudent
            {
                CourseId = courseId,
                StudentId = studentId,
                Student = student
            });

            await _courseRepository.UpdateAsync(course);
        }

        public async Task RemoveStudentAsync(int courseId, int studentId)
        {
            var course = await _courseRepository.GetByIdAsync(courseId);
            if (course == null) throw new Exception("Course not found");

            var relation = course.CourseStudents.FirstOrDefault(cs => cs.StudentId == studentId);
            if (relation != null)
            {
                course.CourseStudents.Remove(relation);
                await _courseRepository.UpdateAsync(course);
            }
        }
    }
}
