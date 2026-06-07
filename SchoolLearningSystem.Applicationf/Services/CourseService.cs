using AutoMapper;
using SchoolLearningSystem.Applicationf.DTOs;
using SchoolLearningSystem.Applicationf.DTOs.CourseDto;
using SchoolLearningSystem.Applicationf.DTOs.ExamDto;
using SchoolLearningSystem.Applicationf.DTOs.Lesson;
using SchoolLearningSystem.Applicationf.DTOs.Student;
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
        public async Task<IEnumerable<CourseReadDto>> GetAllCoursesAsync()
        {
            var courses = await _courseRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<CourseReadDto>>(courses);
        }

        public async Task<CourseReadDto?> GetCourseByIdAsync(int id)
        {
            var course = await _courseRepository.GetByIdAsync(id);
            return _mapper.Map<CourseReadDto?>(course);
        }

        public async Task AddCourseAsync(CourseCreateDto dto)
        {
            var entity = _mapper.Map<Course>(dto);
            await _courseRepository.AddAsync(entity);
        }

        public async Task UpdateCourseAsync(int id, CourseUpdateDto dto)
        {
            var course = await _courseRepository.GetByIdAsync(id);
            if (course == null) throw new Exception("Course not found");

            _mapper.Map(dto, course); // يطبق التعديلات على الكورس الموجود
            await _courseRepository.UpdateAsync(course);
        }

        public async Task DeleteCourseAsync(int id)
        {
            await _courseRepository.DeleteAsync(id);
        }

        // علاقات إضافية
        public async Task<IEnumerable<StudentReadDto>> GetStudentsByCourseIdAsync(int courseId)
        {
            var course = await _courseRepository.GetByIdAsync(courseId);
            if (course == null) return Enumerable.Empty<StudentReadDto>();

            var students = course.CourseStudents.Select(cs => cs.Student);
            return _mapper.Map<IEnumerable<StudentReadDto>>(students);
        }

        public async Task<IEnumerable<LessonReadDto>> GetLessonsByCourseIdAsync(int courseId)
        {
            var lessons = await _lessonRepository.GetByCourseIdAsync(courseId);
            return _mapper.Map<IEnumerable<LessonReadDto>>(lessons);
        }

        public async Task<IEnumerable<ExamReadDto>> GetExamsByCourseIdAsync(int courseId)
        {
            var exams = await _examRepository.GetByCourseIdAsync(courseId);
            return _mapper.Map<IEnumerable<ExamReadDto>>(exams);
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
