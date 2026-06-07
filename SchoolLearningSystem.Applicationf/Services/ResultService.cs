using AutoMapper;
using SchoolLearningSystem.Applicationf.DTOs.Result;
using SchoolLearningSystem.Applicationf.Interfaces;
using SchoolLearningSystem.Domain.Entities;
using SchoolLearningSystem.Domain.Interfaces; // نفترض عندك IResultRepository
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SchoolLearningSystem.Applicationf.Services
{
    public class ResultService : IResultService
    {
        private readonly IResultRepository _resultRepository;
        private readonly IMapper _mapper;

        public ResultService(IResultRepository resultRepository, IMapper mapper)
        {
            _resultRepository = resultRepository;
            _mapper = mapper;
        }

        // العمليات الأساسية
        public async Task<IEnumerable<ResultReadDto>> GetAllResultsAsync()
        {
            var results = await _resultRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<ResultReadDto>>(results);
        }

        public async Task<ResultReadDto?> GetResultByIdAsync(int id)
        {
            var result = await _resultRepository.GetByIdAsync(id);
            return _mapper.Map<ResultReadDto?>(result);
        }

        public async Task AddResultAsync(ResultCreateDto dto)
        {
            var entity = _mapper.Map<Result>(dto);
            await _resultRepository.AddAsync(entity);
        }

        public async Task UpdateResultAsync(int id, ResultUpdateDto dto)
        {
            var existing = await _resultRepository.GetByIdAsync(id);
            if (existing != null)
            {
                _mapper.Map(dto, existing);
                await _resultRepository.UpdateAsync(existing);
            }
        }

        public async Task DeleteResultAsync(int id)
        {
            await _resultRepository.DeleteAsync(id);
        }

        // علاقات إضافية
        public async Task<IEnumerable<ResultReadDto>> GetResultsByStudentIdAsync(int studentId)
        {
            var results = await _resultRepository.GetAllAsync();
            var filtered = results.Where(r => r.StudentId == studentId);
            return _mapper.Map<IEnumerable<ResultReadDto>>(filtered);
        }

        public async Task<IEnumerable<ResultReadDto>> GetResultsByLessonIdAsync(int lessonId)
        {
            var results = await _resultRepository.GetAllAsync();
            var filtered = results.Where(r => r.LessonId == lessonId);
            return _mapper.Map<IEnumerable<ResultReadDto>>(filtered);
        }

        public async Task<IEnumerable<ResultReadDto>> GetResultsByExamIdAsync(int examId)
        {
            var results = await _resultRepository.GetAllAsync();
            var filtered = results.Where(r => r.ExamId == examId);
            return _mapper.Map<IEnumerable<ResultReadDto>>(filtered);
        }

        // إحصائيات إضافية
        public async Task<double> GetAverageScoreByStudentIdAsync(int studentId)
        {
            var results = await _resultRepository.GetAllAsync();
            var filtered = results.Where(r => r.StudentId == studentId);
            return filtered.Any() ? filtered.Average(r => r.Score) : 0;
        }

        public async Task<double> GetAverageScoreByLessonIdAsync(int lessonId)
        {
            var results = await _resultRepository.GetAllAsync();
            var filtered = results.Where(r => r.LessonId == lessonId);
            return filtered.Any() ? filtered.Average(r => r.Score) : 0;
        }

        public async Task<double> GetAverageScoreByExamIdAsync(int examId)
        {
            var results = await _resultRepository.GetAllAsync();
            var filtered = results.Where(r => r.ExamId == examId);
            return filtered.Any() ? filtered.Average(r => r.Score) : 0;
        }
    }
}
