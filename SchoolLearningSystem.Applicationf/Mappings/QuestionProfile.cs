using AutoMapper;
using SchoolLearningSystem.Applicationf.DTOs.Question;
using SchoolLearningSystem.Domain.Entities;

namespace SchoolLearningSystem.Applicationf.Mappings
{
    public class QuestionProfile : Profile
    {
        public QuestionProfile()
        {
            // 1. من الكيان → للعرض (Read)
            // Text/Answer/DifficultyLevel/LessonId تتطابق أسماؤها تلقائياً
            CreateMap<Question, QuestionReadDto>()
                .ForMember(dest => dest.LessonTitle,
                    opt => opt.MapFrom(src => src.Lesson != null ? src.Lesson.Title : string.Empty));

            // 2. من الإنشاء → للكيان (Create)
            CreateMap<QuestionCreateDto, Question>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.LastModifiedAt, opt => opt.Ignore())
                .ForMember(dest => dest.IsDeleted, opt => opt.Ignore())
                .ForMember(dest => dest.Lesson, opt => opt.Ignore())
                .ForMember(dest => dest.Exam, opt => opt.Ignore())
                .ForMember(dest => dest.QuestionStats, opt => opt.Ignore());

            // 3. من التعديل → للكيان (Update)
            CreateMap<QuestionUpdateDto, Question>()
                .ForMember(dest => dest.Text, opt => {
                    opt.Condition(src => src.Text != null);
                    opt.MapFrom(src => src.Text);
                })
                .ForMember(dest => dest.Answer, opt => {
                    opt.Condition(src => src.Answer != null);
                    opt.MapFrom(src => src.Answer);
                })
                .ForMember(dest => dest.DifficultyLevel, opt => {
                    opt.Condition(src => src.DifficultyLevel.HasValue);
                    opt.MapFrom(src => src.DifficultyLevel);
                })
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.LastModifiedAt, opt => opt.Ignore())
                .ForMember(dest => dest.IsDeleted, opt => opt.Ignore())
                .ForMember(dest => dest.Lesson, opt => opt.Ignore())
                .ForMember(dest => dest.Exam, opt => opt.Ignore())
                .ForMember(dest => dest.QuestionStats, opt => opt.Ignore());
        }
    }
}