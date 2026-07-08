using AutoMapper;
using SchoolLearningSystem.Applicationf.DTOs.ExerciseDto;
using SchoolLearningSystem.Domain.Entities;

namespace SchoolLearningSystem.Applicationf.Mappings
{
    public class ExerciseProfile : Profile
    {
        public ExerciseProfile()
        {
            // 1. من الكيان → للعرض (Read)
            // Question/Answer/Difficulty تتطابق أسماؤها تلقائياً مع الـ Entity الآن
            // (الـ DTO أُعيدت تسميته ليطابق الـ Entity مباشرة - راجع dtos_review_report.md قسم 3.2)
            CreateMap<Exercise, ExerciseReadDto>()
                .ForMember(dest => dest.LessonTitle,
                    opt => opt.MapFrom(src => src.Lesson != null ? src.Lesson.Title : string.Empty));

            // 2. من الإنشاء → للكيان (Create)
            CreateMap<ExerciseCreateDto, Exercise>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.LastModifiedAt, opt => opt.Ignore())
                .ForMember(dest => dest.IsDeleted, opt => opt.Ignore())
                .ForMember(dest => dest.Lesson, opt => opt.Ignore());

            // 3. من التعديل → للكيان (Update) - مع شرط التحديث الجزئي
            // ✅ Difficulty مُضافة هنا - كانت مفقودة تماماً بالنسخة السابقة
            CreateMap<ExerciseUpdateDto, Exercise>()
                .ForMember(dest => dest.Question, opt => {
                    opt.Condition(src => src.Question != null);
                    opt.MapFrom(src => src.Question);
                })
                .ForMember(dest => dest.Answer, opt => {
                    opt.Condition(src => src.Answer != null);
                    opt.MapFrom(src => src.Answer);
                })
                .ForMember(dest => dest.Difficulty, opt => {
                    opt.Condition(src => src.Difficulty.HasValue);
                    opt.MapFrom(src => src.Difficulty);
                })
                .ForMember(dest => dest.LessonId, opt => {
                    opt.Condition(src => src.LessonId.HasValue);
                    opt.MapFrom(src => src.LessonId);
                })
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.LastModifiedAt, opt => opt.Ignore())
                .ForMember(dest => dest.IsDeleted, opt => opt.Ignore())
                .ForMember(dest => dest.Lesson, opt => opt.Ignore());
        }
    }
}