using AutoMapper;
using SchoolLearningSystem.Applicationf.DTOs.CourseDto;
using SchoolLearningSystem.Applicationf.DTOs.MemorizeSession;
using SchoolLearningSystem.Applicationf.DTOs.Result;
using SchoolLearningSystem.Applicationf.DTOs.Student;
using SchoolLearningSystem.Applicationf.Interfaces;
using SchoolLearningSystem.Domain.Entities;
using SchoolLearningSystem.Domain.Interfaces; // نفترض عندك IStudentRepository
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

        // 🔹 العمليات الأساسية
        public async Task<IEnumerable<StudentReadDto>> GetAllStudentsAsync()
        {
            var students = await _studentRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<StudentReadDto>>(students);
        }

        public async Task<StudentReadDto?> GetStudentByIdAsync(int id)
        {
            var student = await _studentRepository.GetByIdAsync(id);
            return _mapper.Map<StudentReadDto?>(student);
        }

        public async Task AddStudentAsync(StudentCreateDto dto)
        {
            var entity = _mapper.Map<Student>(dto);
            await _studentRepository.AddAsync(entity);
        }

        public async Task UpdateStudentAsync(int id, StudentUpdateDto dto)
        {
            var existing = await _studentRepository.GetByIdAsync(id);
            if (existing != null)
            {
                _mapper.Map(dto, existing);
                await _studentRepository.UpdateAsync(existing);
            }
            else
            {
                throw new KeyNotFoundException("Student not found");
            }
        }

        public async Task DeleteStudentAsync(int id)
        {
            await _studentRepository.DeleteAsync(id);
        }

        // 🔹 علاقات إضافية
        public async Task<IEnumerable<CourseDto>> GetCoursesByStudentIdAsync(int studentId)
        {
            var student = await _studentRepository.GetByIdAsync(studentId);
            if (student == null) return Enumerable.Empty<CourseDto>();

            var courses = student.CourseStudents.Select(cs => cs.Course).ToList();
            return _mapper.Map<IEnumerable<CourseDto>>(courses);
        }

        public async Task<IEnumerable<ResultReadDto>> GetResultsByStudentIdAsync(int studentId)
        {
            var student = await _studentRepository.GetByIdAsync(studentId);
            if (student == null) return Enumerable.Empty<ResultReadDto>();

            return _mapper.Map<IEnumerable<ResultReadDto>>(student.Results);
        }

        public async Task<IEnumerable<MemorizeSessionReadDto>> GetMemorizeSessionsByStudentIdAsync(int studentId)
        {
            var student = await _studentRepository.GetByIdAsync(studentId);
            if (student == null) return Enumerable.Empty<MemorizeSessionReadDto>();

            return _mapper.Map<IEnumerable<MemorizeSessionReadDto>>(student.MemorizeSessions);
        }

        // 🔹 إحصائيات
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
