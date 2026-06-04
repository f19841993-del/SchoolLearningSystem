using AutoMapper;
using SchoolLearningSystem.Applicationf.DTOs;
using SchoolLearningSystem.Applicationf.Interfaces;
using SchoolLearningSystem.Domain.Entities;
using SchoolLearningSystem.Domain.Interfaces;

namespace SchoolLearningSystem.Applicationf.Services
{
    public class CurriculumService : ICurriculumService
    {
        private readonly ICurriculumRepository _curriculumRepository;
        private readonly ICourseRepository _courseRepository;
        private readonly IMapper _mapper;

        public CurriculumService(
            ICurriculumRepository curriculumRepository,
            ICourseRepository courseRepository,
            IMapper mapper)
        {
            _curriculumRepository = curriculumRepository;
            _courseRepository = courseRepository;
            _mapper = mapper;
        }

        // العمليات الأساسية
        public async Task<IEnumerable<CurriculumDto>> GetAllCurriculumsAsync()
        {
            var curriculums = await _curriculumRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<CurriculumDto>>(curriculums);
        }

        public async Task<CurriculumDto?> GetCurriculumByIdAsync(int id)
        {
            var curriculum = await _curriculumRepository.GetByIdAsync(id);
            return _mapper.Map<CurriculumDto?>(curriculum);
        }

        public async Task AddCurriculumAsync(CurriculumDto dto)
        {
            var entity = _mapper.Map<Curriculum>(dto);
            await _curriculumRepository.AddAsync(entity);

        }

        public async Task UpdateCurriculumAsync(CurriculumDto dto)
        {
            var entity = _mapper.Map<Curriculum>(dto);
            await _curriculumRepository.UpdateAsync(entity);
        }

        public async Task DeleteCurriculumAsync(int id)
        {
            await _curriculumRepository.DeleteAsync(id);
        }

        // علاقات إضافية
        public async Task<IEnumerable<CourseDto>> GetCoursesByCurriculumIdAsync(int curriculumId)
        {
            var curriculum = await _curriculumRepository.GetByIdAsync(curriculumId);
            if (curriculum == null) return Enumerable.Empty<CourseDto>();

            var courses = curriculum.Courses;
            return _mapper.Map<IEnumerable<CourseDto>>(courses);
        }

        // البحث حسب المرحلة الدراسية
        public async Task<CurriculumDto?> GetCurriculumByGradeLevelAsync(string gradeLevel)
        {
            var curriculums = await _curriculumRepository.GetAllAsync();
            var curriculum = curriculums.FirstOrDefault(c => c.GradeLevel.ToString() == gradeLevel);
            return _mapper.Map<CurriculumDto?>(curriculum);
        }

        // إحصائيات
        public async Task<int> GetTotalCoursesByCurriculumIdAsync(int curriculumId)
        {
            var curriculum = await _curriculumRepository.GetByIdAsync(curriculumId);
            return curriculum?.Courses.Count ?? 0;
        }
    }
}
