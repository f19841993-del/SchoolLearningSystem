using AutoMapper;
using SchoolLearningSystem.Applicationf.DTOs.CourseDto;
using SchoolLearningSystem.Applicationf.DTOs.Curriculum;
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

        // 🔹 العمليات الأساسية
        public async Task<IEnumerable<CurriculumReadDto>> GetAllCurriculumsAsync()
        {
            var entities = await _curriculumRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<CurriculumReadDto>>(entities);
        }

        public async Task<CurriculumReadDto?> GetCurriculumByIdAsync(int id)
        {
            var entity = await _curriculumRepository.GetByIdAsync(id);
            return _mapper.Map<CurriculumReadDto?>(entity);
        }

        public async Task AddCurriculumAsync(CurriculumCreateDto dto)
        {
            var entity = _mapper.Map<Curriculum>(dto);
            await _curriculumRepository.AddAsync(entity);
        }

        public async Task UpdateCurriculumAsync(int id, CurriculumUpdateDto dto)
        {
            var entity = await _curriculumRepository.GetByIdAsync(id)
                ?? throw new Exception("Curriculum not found");

            _mapper.Map(dto, entity);
            await _curriculumRepository.UpdateAsync(entity);
        }

        public async Task DeleteCurriculumAsync(int id)
        {
            await _curriculumRepository.DeleteAsync(id);
        }

        // 🔹 علاقات إضافية
        public async Task<IEnumerable<CourseReadDto>> GetCoursesByCurriculumIdAsync(int curriculumId)
        {
            var curriculum = await _curriculumRepository.GetByIdAsync(curriculumId)
                ?? throw new Exception("Curriculum not found");

            var courses = curriculum.Courses;
            return _mapper.Map<IEnumerable<CourseReadDto>>(courses);
        }

        // 🔹 البحث حسب المرحلة الدراسية
        public async Task<CurriculumReadDto?> GetCurriculumByGradeLevelAsync(string gradeLevel)
        {
            var entity = await _curriculumRepository.GetByGradeLevelAsync(gradeLevel);
            return _mapper.Map<CurriculumReadDto?>(entity);
        }

        // 🔹 إحصائيات
        public async Task<int> GetTotalCoursesByCurriculumIdAsync(int curriculumId)
        {
            var curriculum = await _curriculumRepository.GetByIdAsync(curriculumId)
                ?? throw new Exception("Curriculum not found");

            return curriculum.Courses.Count;
        }
    }
}
