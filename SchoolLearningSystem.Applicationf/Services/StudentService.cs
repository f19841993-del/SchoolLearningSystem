using AutoMapper;
using SchoolLearningSystem.Applicationf.DTOs.CourseDto;
using SchoolLearningSystem.Applicationf.DTOs.MemorizeSession;
using SchoolLearningSystem.Applicationf.DTOs.Result;
using SchoolLearningSystem.Applicationf.DTOs.Student;
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
    public class StudentService : BaseService<Student, StudentReadDto, StudentCreateDto, StudentUpdateDto>, IStudentService
    {
        private readonly IStudentRepository _studentRepository;

        public StudentService(IStudentRepository studentRepository, IMapper mapper)
            : base(studentRepository, mapper) // الأب يدير الـ CRUD
        {
            _studentRepository = studentRepository;
        }

        // 🔹 CRUD الأساسي: موروث من BaseService (لا حاجة لكتابته هنا)

        // 🔹 علاقات إضافية (Business Logic)
        public async Task<IEnumerable<CourseReadDto>> GetCoursesByStudentIdAsync(int studentId)
        {
            var student = await _studentRepository.GetByIdAsync(studentId)
                ?? throw new Exception("Student not found");

            var courses = student.CourseStudents.Select(cs => cs.Course).ToList();
            return _mapper.Map<IEnumerable<CourseReadDto>>(courses);
        }

        public async Task<IEnumerable<ResultReadDto>> GetResultsByStudentIdAsync(int studentId)
        {
            var student = await _studentRepository.GetByIdAsync(studentId)
                ?? throw new Exception("Student not found");

            return _mapper.Map<IEnumerable<ResultReadDto>>(student.Results);
        }

        public async Task<IEnumerable<MemorizeSessionReadDto>> GetMemorizeSessionsByStudentIdAsync(int studentId)
        {
            var student = await _studentRepository.GetByIdAsync(studentId)
                ?? throw new Exception("Student not found");

            return _mapper.Map<IEnumerable<MemorizeSessionReadDto>>(student.MemorizeSessions);
        }

        // 🔹 إحصائيات
        public async Task<double> GetAverageScoreByStudentIdAsync(int studentId)
        {
            var student = await _studentRepository.GetByIdAsync(studentId)
                ?? throw new Exception("Student not found");

            return student.Results.Any() ? student.Results.Average(r => r.Score) : 0;
        }

        public async Task<int> GetTotalCoursesByStudentIdAsync(int studentId)
        {
            var student = await _studentRepository.GetByIdAsync(studentId)
                ?? throw new Exception("Student not found");

            return student.CourseStudents?.Count ?? 0;
        }
    }
}