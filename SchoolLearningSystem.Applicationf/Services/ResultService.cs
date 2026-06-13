using AutoMapper;
using SchoolLearningSystem.Applicationf.DTOs.Result;
using SchoolLearningSystem.Applicationf.Interfaces;
using SchoolLearningSystem.Applicationf.Services.Base;
using SchoolLearningSystem.Domain.Entities;
using SchoolLearningSystem.Domain.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SchoolLearningSystem.Applicationf.Services
{
    public class ResultService : BaseService<Result, ResultReadDto, ResultCreateDto, ResultUpdateDto>, IResultService
    {
        private readonly IResultRepository _resultRepository;

        public ResultService(IResultRepository resultRepository, IMapper mapper)
            : base(resultRepository, mapper)
        {
            _resultRepository = resultRepository;
        }

        // 🔹 العلاقات (الآن أصبحت سريعة لأنها تستعلم من القاعدة مباشرة)
        public async Task<IEnumerable<ResultReadDto>> GetResultsByStudentIdAsync(int studentId)
        {
            // لاحظ: نستخدم الريبو مباشرة، ولن نستخدم GetAll() أبداً!
            var results = await _resultRepository.GetByStudentIdAsync(studentId);
            return _mapper.Map<IEnumerable<ResultReadDto>>(results);
        }

        public async Task<IEnumerable<ResultReadDto>> GetResultsByLessonIdAsync(int lessonId)
        {
            var results = await _resultRepository.GetByLessonIdAsync(lessonId);
            return _mapper.Map<IEnumerable<ResultReadDto>>(results);
        }

        public async Task<IEnumerable<ResultReadDto>> GetResultsByExamIdAsync(int examId)
        {
            var results = await _resultRepository.GetByExamIdAsync(examId);
            return _mapper.Map<IEnumerable<ResultReadDto>>(results);
        }

        // 🔹 إحصائيات (تطوير الأداء)
        public async Task<double> GetAverageScoreByStudentIdAsync(int studentId)
        {
            // الأفضل هنا عمل دالة AverageByStudentId في الريبو لتكون أسرع
            var results = await _resultRepository.GetByStudentIdAsync(studentId);
            return results.Any() ? results.Average(r => r.Score) : 0;
        }
       public async Task<double> GetAverageScoreByLessonIdAsync(int lessonId)
        {
            var results = await _resultRepository.GetByLessonIdAsync(lessonId);
       
            return results.Any() ? results.Average(r => r.Score) : 0;
        }

        public async Task<double> GetAverageScoreByExamIdAsync(int examId)
        {
            var results = await _resultRepository.GetByExamIdAsync(examId);

            return results.Any() ? results.Average(r => r.Score) : 0;
        }
    }
}