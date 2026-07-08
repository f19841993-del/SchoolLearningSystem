using AutoMapper;
using SchoolLearningSystem.Applicationf.DTOs.StudentQuestionProgress;
using SchoolLearningSystem.Domain.Entities;

namespace SchoolLearningSystem.Applicationf.Mappings
{
    public class StudentQuestionProgressProfile : Profile
    {
        public StudentQuestionProgressProfile()
        {
            // 1. من الكيان → للعرض (Read)
            CreateMap<StudentQuestionProgress, StudentQuestionProgressReadDto>()
                .ForMember(dest => dest.QuestionText,
                    opt => opt.MapFrom(src => src.Question != null ? src.Question.Text : string.Empty));

            // 2. من الإنشاء → للكيان (Create)
            // NextReviewDate/EaseFactor/Interval/RepetitionLevel تبقى بقيمها
            // الافتراضية المعرّفة بالـ Entity نفسه (لا تُرسل من الـ Client)
            CreateMap<StudentQuestionProgressCreateDto, StudentQuestionProgress>()
                .ForMember(dest => dest.Student, opt => opt.Ignore())
                .ForMember(dest => dest.Question, opt => opt.Ignore());

            // 3. من التعديل → للكيان (Update)
            // ⚠️ Admin-only override - راجع التحذير المعماري بـ
            // StudentQuestionProgressUpdateDto. التعديل الطبيعي يمر حصراً
            // عبر SrsService.ProcessAnswerAsync، ليس عبر هذا الـ Mapping.
            //
            // 💡 كل حقل مُصرَّح صراحة (بدل ForAllMembers) لتفادي أي التباس
            // بترتيب استدعاء ForAllMembers/ForMember(Ignore) - وهو تفاعل
            // موثق ومعروف بـ AutoMapper (راجع GitHub Discussion #4370).
            // Student/Question لا يحتاجان Ignore أصلاً: لا يوجد لهما حقل
            // مطابق بالـ Update DTO فيحاول AutoMapper ربطه أساساً.
            CreateMap<StudentQuestionProgressUpdateDto, StudentQuestionProgress>()
                .ForMember(dest => dest.NextReviewDate, opt => {
                    opt.Condition(src => src.NextReviewDate.HasValue);
                    opt.MapFrom(src => src.NextReviewDate);
                })
                .ForMember(dest => dest.RepetitionLevel, opt => {
                    opt.Condition(src => src.RepetitionLevel.HasValue);
                    opt.MapFrom(src => src.RepetitionLevel);
                })
                .ForMember(dest => dest.EaseFactor, opt => {
                    opt.Condition(src => src.EaseFactor.HasValue);
                    opt.MapFrom(src => src.EaseFactor);
                })
                .ForMember(dest => dest.Interval, opt => {
                    opt.Condition(src => src.Interval.HasValue);
                    opt.MapFrom(src => src.Interval);
                })
                .ForMember(dest => dest.TotalAttempts, opt => {
                    opt.Condition(src => src.TotalAttempts.HasValue);
                    opt.MapFrom(src => src.TotalAttempts);
                })
                .ForMember(dest => dest.CorrectAttempts, opt => {
                    opt.Condition(src => src.CorrectAttempts.HasValue);
                    opt.MapFrom(src => src.CorrectAttempts);
                })
                .ForMember(dest => dest.LastReviewedAt, opt => {
                    opt.Condition(src => src.LastReviewedAt.HasValue);
                    opt.MapFrom(src => src.LastReviewedAt);
                });
        }
    }
}