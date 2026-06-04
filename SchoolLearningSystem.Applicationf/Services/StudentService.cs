using AutoMapper;
using SchoolLearningSystem.Applicationf.DTOs;
using SchoolLearningSystem.Applicationf.Interfaces;
using SchoolLearningSystem.Domain.Entities;
using WebApiTemplate.Domain.Interfaces;

namespace SchoolLearningSystem.Applicationf.Services
{
    public class StudentService : IStudentService
    {
        private readonly IStudentRepository _studentRepository;
        private readonly IMapper _mapper;

        public StudentService(IStudentRepository studentRepository, IMapper mapper)
        {
            _studentRepository = studentRepository;
            _mapper = mapper;
        }

        // العمليات الأساسية
        public async Task<IEnumerable<StudentDto>> GetAllStudentsAsync()
        {
            var students = await _studentRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<StudentDto>>(students);
        }

        public async Task<StudentDto?> GetStudentByIdAsync(int id)
        {
            var student = await _studentRepository.GetByIdAsync(id);
            return _mapper.Map<StudentDto?>(student);
        }

        public async Task AddStudentAsync(StudentDto dto)
        {
            var entity = _mapper.Map<Student>(dto);
            await _studentRepository.AddAsync(entity);
        }

        public async Task UpdateStudentAsync(StudentDto dto)
        {
            var entity = _mapper.Map<Student>(dto);
            await _studentRepository.UpdateAsync(entity);
        }

        public async Task DeleteStudentAsync(int id)
        {
            await _studentRepository.DeleteAsync(id);
        }

        // علاقات إضافية
        public async Task<IEnumerable<CourseDto>> GetCoursesByStudentIdAsync(int studentId)
        {
            var student = await _studentRepository.GetByIdAsync(studentId);
            if (student == null) return Enumerable.Empty<CourseDto>();

            // تحويل CourseStudents إلى CourseDto
            var courses = student.CourseStudents.Select(cs => cs.Course).ToList();
            return _mapper.Map<IEnumerable<CourseDto>>(courses);
        }

        public async Task<IEnumerable<ResultDto>> GetResultsByStudentIdAsync(int studentId)
        {
            var student = await _studentRepository.GetByIdAsync(studentId);
            if (student == null) return Enumerable.Empty<ResultDto>();

            return _mapper.Map<IEnumerable<ResultDto>>(student.Results);
        }

        public async Task<IEnumerable<MemorizeSessionDto>> GetMemorizeSessionsByStudentIdAsync(int studentId)
        {
            var student = await _studentRepository.GetByIdAsync(studentId);
            if (student == null) return Enumerable.Empty<MemorizeSessionDto>();

            return _mapper.Map<IEnumerable<MemorizeSessionDto>>(student.MemorizeSessions);
        }

        // إحصائيات
        public async Task<double> GetAverageScoreByStudentIdAsync(int studentId)
        {
            var student = await _studentRepository.GetByIdAsync(studentId);
            if (student == null || !student.Results.Any()) return 0;

            return student.Results.Average(r => r.Score);
        }

        public async Task<int> GetTotalCoursesByStudentIdAsync(int studentId)
        {
            var student = await _studentRepository.GetByIdAsync(studentId);
            if (student == null) return 0;

            return student.CourseStudents.Count;
        }
    }
}
