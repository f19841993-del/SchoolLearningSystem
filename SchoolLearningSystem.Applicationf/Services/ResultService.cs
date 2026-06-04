using AutoMapper;
using SchoolLearningSystem.Applicationf.DTOs;
using SchoolLearningSystem.Applicationf.Interfaces;
using SchoolLearningSystem.Domain.Entities;
using SchoolLearningSystem.Domain.Interfaces;

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
        public async Task<IEnumerable<ResultDto>> GetAllResultsAsync()
        {
            var results = await _resultRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<ResultDto>>(results);
        }

        public async Task<ResultDto?> GetResultByIdAsync(int id)
        {
            var result = await _resultRepository.GetByIdAsync(id);
            return _mapper.Map<ResultDto?>(result);
        }

        public async Task AddResultAsync(ResultDto dto)
        {
            var entity = _mapper.Map<Result>(dto);
            await _resultRepository.AddAsync(entity);
        }

        public async Task UpdateResultAsync(ResultDto dto)
        {
            var entity = _mapper.Map<Result>(dto);
            await _resultRepository.UpdateAsync(entity);
        }

        public async Task DeleteResultAsync(int id)
        {
            await _resultRepository.DeleteAsync(id);
        }

        // علاقات إضافية
        public async Task<IEnumerable<ResultDto>> GetResultsByStudentIdAsync(int studentId)
        {
            var results = await _resultRepository.GetAllAsync();
            var filtered = results.Where(r => r.StudentId == studentId);
            return _mapper.Map<IEnumerable<ResultDto>>(filtered);
        }

        public async Task<IEnumerable<ResultDto>> GetResultsByLessonIdAsync(int lessonId)
        {
            var results = await _resultRepository.GetAllAsync();
            var filtered = results.Where(r => r.LessonId == lessonId);
            return _mapper.Map<IEnumerable<ResultDto>>(filtered);
        }

        public async Task<IEnumerable<ResultDto>> GetResultsByExamIdAsync(int examId)
        {
            var results = await _resultRepository.GetAllAsync();
            var filtered = results.Where(r => r.ExamId == examId);
            return _mapper.Map<IEnumerable<ResultDto>>(filtered);
        }
    }
}
