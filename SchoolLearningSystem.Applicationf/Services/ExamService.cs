using AutoMapper;
using SchoolLearningSystem.Applicationf.DTOs.ExamDto;
using SchoolLearningSystem.Applicationf.DTOs.Lesson;
using SchoolLearningSystem.Applicationf.DTOs.Question;
using SchoolLearningSystem.Applicationf.DTOs.Result;
using SchoolLearningSystem.Applicationf.Interfaces;
using SchoolLearningSystem.Applicationf.Services.Base;
using SchoolLearningSystem.Domain.Entities;
using SchoolLearningSystem.Domain.Interfaces;

namespace SchoolLearningSystem.Applicationf.Services
{
    public class ExamService : BaseService<Exam, ExamReadDto, ExamCreateDto, ExamUpdateDto>, IExamService
    {
        private readonly IExamRepository _examRepository;
        private readonly ICourseRepository _courseRepository;
        private readonly ILessonRepository _lessonRepository;
     

        public ExamService(
            IExamRepository examRepository,
            ICourseRepository courseRepository,
            ILessonRepository lessonRepository,
            IMapper mapper)
            : base(examRepository, mapper) // الأب يقوم بكل عمليات الـ CRUD
        {
            _examRepository = examRepository;
            _courseRepository = courseRepository;
            _lessonRepository = lessonRepository;
             
        }

        // 🔹 ملاحظة: الـ CRUD الأساسية (AddAsync, UpdateAsync..) أصبحت جاهزة بفضل الوراثة!
        // إذا كنت تريد إضافة منطق خاص للـ Add، يمكنك عمل override لها:
        public async Task<ExamReadDto> CreateAsync(ExamCreateDto dto)
        {
            // 1. تحقق من وجود العلاقات أولاً
            var course = await _courseRepository.GetByIdAsync(dto.CourseId)
                         ?? throw new Exception("Course not found");
            var lesson = await _lessonRepository.GetByIdAsync(dto.LessonId)
                         ?? throw new Exception("Lesson not found");

            // 2. التحويل (Map)
            var entity = _mapper.Map<Exam>(dto);

            // 3. الربط اليدوي (هذا هو المكان الصحيح للقيام بذلك)
            entity.Course = course;
            entity.Lesson = lesson;

            // 4. الحفظ (بما أننا لا نستخدم base، يجب أن نحفظ هنا)
            await _examRepository.AddAsync(entity);

            // 5. إرجاع النتيجة
            return _mapper.Map<ExamReadDto>(entity);
        }

        // 🔹 تنفيذ العلاقات الإضافية
        public async Task<IEnumerable<QuestionReadDto>> GetQuestionsByExamIdAsync(int examId)
        {
            var exam = await _examRepository.GetByIdAsync(examId) ?? throw new Exception("Exam not found");
            return _mapper.Map<IEnumerable<QuestionReadDto>>(exam.Questions);
        }

        public async Task<IEnumerable<ResultReadDto>> GetResultsByExamIdAsync(int examId)
        {
            var exam = await _examRepository.GetByIdAsync(examId) ?? throw new Exception("Exam not found");
            return _mapper.Map<IEnumerable<ResultReadDto>>(exam.Results);
        }

        public async Task<IEnumerable<LessonReadDto>> GetLessonsByExamIdAsync(int examId)
        {
            var exam = await _examRepository.GetByIdAsync(examId) ?? throw new Exception("Exam not found");
            return _mapper.Map<IEnumerable<LessonReadDto>>(new List<Lesson> { exam.Lesson });
        }
    }
}