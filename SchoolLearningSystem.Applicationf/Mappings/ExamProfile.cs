using AutoMapper;
using SchoolLearningSystem.Applicationf.DTOs.ExamDto;
using SchoolLearningSystem.Domain.Entities;

namespace SchoolLearningSystem.Applicationf.Mappings
{
    public class ExamProfile : Profile
    {
        public ExamProfile()
        {
            // 1. من الكيان → للعرض (Read)
            // Title/ExamType/Difficulty/CourseId/LessonId تتطابق أسماؤها تلقائياً
            CreateMap<Exam, ExamReadDto>()
                .ForMember(dest => dest.CourseTitle,
                    opt => opt.MapFrom(src => src.Course != null ? src.Course.Title : string.Empty))
                .ForMember(dest => dest.LessonTitle,
                    opt => opt.MapFrom(src => src.Lesson != null ? src.Lesson.Title : null))
                .ForMember(dest => dest.QuestionsCount,
                    opt => opt.MapFrom(src => src.Questions != null ? src.Questions.Count : 0));

            // 2. من الإنشاء → للكيان (Create)
            CreateMap<ExamCreateDto, Exam>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.LastModifiedAt, opt => opt.Ignore())
                .ForMember(dest => dest.IsDeleted, opt => opt.Ignore())
                .ForMember(dest => dest.Course, opt => opt.Ignore())
                .ForMember(dest => dest.Lesson, opt => opt.Ignore())
                .ForMember(dest => dest.Questions, opt => opt.Ignore())
                .ForMember(dest => dest.Results, opt => opt.Ignore());

            // 3. من التعديل → للكيان (Update)
            CreateMap<ExamUpdateDto, Exam>()
                .ForMember(dest => dest.Title, opt => {
                    opt.Condition(src => src.Title != null);
                    opt.MapFrom(src => src.Title);
                })
                .ForMember(dest => dest.ExamType, opt => {
                    opt.Condition(src => src.ExamType.HasValue);
                    opt.MapFrom(src => src.ExamType);
                })
                .ForMember(dest => dest.Difficulty, opt => {
                    opt.Condition(src => src.Difficulty.HasValue);
                    opt.MapFrom(src => src.Difficulty);
                })
                .ForMember(dest => dest.LessonId, opt => {
                    opt.Condition(src => src.LessonId.HasValue);
                    opt.MapFrom(src => src.LessonId);
                })
                // CourseId مستثنى عمداً - نقل امتحان بين كورسات Use Case منفصل (راجع ExamUpdateDto)
                .ForMember(dest => dest.CourseId, opt => opt.Ignore())
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.LastModifiedAt, opt => opt.Ignore())
                .ForMember(dest => dest.IsDeleted, opt => opt.Ignore())
                .ForMember(dest => dest.Course, opt => opt.Ignore())
                .ForMember(dest => dest.Lesson, opt => opt.Ignore())
                .ForMember(dest => dest.Questions, opt => opt.Ignore())
                .ForMember(dest => dest.Results, opt => opt.Ignore());
        }
    }
}