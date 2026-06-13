using AutoMapper;
using SchoolLearningSystem.Applicationf.DTOs.CourseDto;
using SchoolLearningSystem.Applicationf.DTOs.CurriculumDto;
using SchoolLearningSystem.Applicationf.Interfaces;
using SchoolLearningSystem.Applicationf.Services.Base;
using SchoolLearningSystem.Domain.Entities;
using SchoolLearningSystem.Domain.Enums;
using SchoolLearningSystem.Domain.Interfaces;

namespace SchoolLearningSystem.Applicationf.Services
{
    public class CurriculumService : BaseService<Curriculum, CurriculumReadDto, CurriculumCreateDto, CurriculumUpdateDto>, ICurriculumService
    {
        private readonly ICurriculumRepository _curriculumRepository;
        private readonly ICourseRepository _courseRepository;

        // لاحظ هنا: قمنا بتمرير _curriculumRepository إلى base (لأن الـ BaseService يحتاجه للـ CRUD)
        public CurriculumService(
            ICurriculumRepository curriculumRepository,
            ICourseRepository courseRepository,
            IMapper mapper)
            : base(curriculumRepository, mapper)
        {
            _curriculumRepository = curriculumRepository;
            _courseRepository = courseRepository;
        }

        // 🔹 الدوال الأساسية (GetAll, Add, Update, Delete)
        // لا نحتاج كتابتها هنا لأنها موجودة ومطبقة في BaseService!

        // 🔹 تنفيذ العمليات الخاصة (Specific Business Logic)
        public async Task<IEnumerable<CourseReadDto>> GetCoursesByCurriculumIdAsync(int curriculumId)
        {
            var curriculum = await _curriculumRepository.GetByIdAsync(curriculumId)
                ?? throw new Exception("Curriculum not found");

            // هنا نستخدم _mapper لتحويل الكورسات
            return _mapper.Map<IEnumerable<CourseReadDto>>(curriculum.Courses);
        }

        public async Task<CurriculumReadDto?> GetCurriculumByGradeLevelAsync(GradeLevel gradeLevel)
        {
            var entity = await _curriculumRepository.GetByGradeLevelAsync(gradeLevel);
            return _mapper.Map<CurriculumReadDto?>(entity);
        }

        public async Task<int> GetTotalCoursesByCurriculumIdAsync(int curriculumId)
        {
            var curriculum = await _curriculumRepository.GetByIdAsync(curriculumId)
                ?? throw new Exception("Curriculum not found");

            return curriculum.Courses.Count;
        }
    }
}