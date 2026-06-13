using AutoMapper;
using SchoolLearningSystem.Applicationf.DTOs.ExerciseDto;
using SchoolLearningSystem.Domain.Entities;
using SchoolLearningSystem.Domain.Enums;
using System.Linq;

namespace SchoolLearningSystem.Applicationf.Mappings
{
    public class ExerciseProfile : Profile
    {
        public ExerciseProfile()
        {
            // 1. من الكيان → للعرض (Read)
            CreateMap<Exercise, ExerciseReadDto>()
                .ForMember(dest => dest.QuestionText, opt => opt.MapFrom(src => src.Question))
                .ForMember(dest => dest.CorrectAnswer, opt => opt.MapFrom(src => src.Answer))
                .ForMember(dest => dest.LessonTitle, opt => opt.MapFrom(src => src.Lesson != null ? src.Lesson.Title : string.Empty))
                .ForMember(dest => dest.DifficultyLevel, opt => opt.MapFrom(src => (int)src.Difficulty));

            // 2. من الإنشاء → للكيان (Create)
            CreateMap<ExerciseCreateDto, Exercise>()
                .ForMember(dest => dest.Question, opt => opt.MapFrom(src => src.QuestionText))
                .ForMember(dest => dest.Answer, opt => opt.MapFrom(src => src.CorrectAnswer))
                .ForMember(dest => dest.Difficulty, opt => opt.MapFrom(src => (DifficultyLevel)src.DifficultyLevel))
                // الحماية الموحدة لحقول الـ BaseEntity
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.LastModifiedAt, opt => opt.Ignore())
                .ForMember(dest => dest.IsDeleted, opt => opt.Ignore())
                // تجاهل العلاقات
                .ForMember(dest => dest.Lesson, opt => opt.Ignore());

            // 3. من التعديل → للكيان (Update) - مع شرط التحديث الجزئي
            CreateMap<ExerciseUpdateDto, Exercise>()
                .ForMember(dest => dest.Question, opt => {
                    opt.Condition(src => src.Title != null);
                    // إذا كان الـ Exercise يحتوي على حقل Title، فقم بالربط، وإلا Ignore
                })
                .ForMember(dest => dest.Question, opt => {
                    opt.Condition(src => src.Question != null);
                    opt.MapFrom(src => src.Question);
                })
                .ForMember(dest => dest.Answer, opt => {
                    opt.Condition(src => src.Answer != null);
                    opt.MapFrom(src => src.Answer);
                })
                .ForMember(dest => dest.LessonId, opt => {
                    opt.Condition(src => src.LessonId.HasValue);
                    opt.MapFrom(src => src.LessonId);
                })
                // الحماية الموحدة لحقول الـ BaseEntity
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.LastModifiedAt, opt => opt.Ignore())
                .ForMember(dest => dest.IsDeleted, opt => opt.Ignore())
                .ForMember(dest => dest.Lesson, opt => opt.Ignore());
        }
    }
}