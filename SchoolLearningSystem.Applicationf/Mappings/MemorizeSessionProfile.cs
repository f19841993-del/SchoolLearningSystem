using AutoMapper;
using SchoolLearningSystem.Applicationf.DTOs.MemorizeSession;
using SchoolLearningSystem.Domain.Entities;

namespace SchoolLearningSystem.Applicationf.Mappings
{
    public class MemorizeSessionProfile : Profile
    {
        public MemorizeSessionProfile()
        {
            // 1. من الكيان → للعرض (Read)
            // TotalAttempts, SuccessRate, DurationInSeconds, IsCompleted, CompletedAt, CreatedAt
            // تتطابق أسماؤها تلقائياً
            CreateMap<MemorizeSession, MemorizeSessionReadDto>()
                .ForMember(dest => dest.StudentName,
                    opt => opt.MapFrom(src => src.Student != null ? src.Student.Name : string.Empty))
                .ForMember(dest => dest.ExerciseTitle,
                    opt => opt.MapFrom(src => src.Exercise != null ? src.Exercise.Question : null));
            // ⚠️ لا يوجد Lesson/LessonTitle هنا - MemorizeSession لا يرتبط بدرس واحد
            // (راجع تعليق MemorizeSession.cs: الجلسة تجمع أسئلة مستحقة من دروس متعددة)

            // 2. من الإنشاء → للكيان (Create)
            CreateMap<MemorizeSessionCreateDto, MemorizeSession>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.LastModifiedAt, opt => opt.Ignore())
                .ForMember(dest => dest.IsDeleted, opt => opt.Ignore())
                // القيم الابتدائية لحقول التحليل المحسوبة
                .ForMember(dest => dest.TotalAttempts, opt => opt.MapFrom(src => 0))
                .ForMember(dest => dest.SuccessRate, opt => opt.MapFrom(src => 0.0))
                .ForMember(dest => dest.IsCompleted, opt => opt.MapFrom(src => false))
                .ForMember(dest => dest.CompletedAt, opt => opt.Ignore())
                .ForMember(dest => dest.Student, opt => opt.Ignore())
                .ForMember(dest => dest.Exercise, opt => opt.Ignore())
                .ForMember(dest => dest.AnswerDetails, opt => opt.Ignore());

            // 3. من التعديل → للكيان (Update)
            // ✅ IsCompleted/CompletedAt/DurationInSeconds مُضافة - كانت مفقودة تماماً
            // بالنسخة السابقة، ما كان يخلي "إنهاء الجلسة" يشتغل فعلياً
            CreateMap<MemorizeSessionUpdateDto, MemorizeSession>()
                .ForMember(dest => dest.SuccessRate, opt => {
                    opt.Condition(src => src.SuccessRate.HasValue);
                    opt.MapFrom(src => src.SuccessRate);
                })
                .ForMember(dest => dest.DurationInSeconds, opt => {
                    opt.Condition(src => src.DurationInSeconds.HasValue);
                    opt.MapFrom(src => src.DurationInSeconds);
                })
                .ForMember(dest => dest.IsCompleted, opt => {
                    opt.Condition(src => src.IsCompleted.HasValue);
                    opt.MapFrom(src => src.IsCompleted);
                })
                // لما تكتمل الجلسة، نثبّت وقت الإكمال تلقائياً
                .ForMember(dest => dest.CompletedAt, opt => {
                    opt.Condition(src => src.IsCompleted == true);
                    opt.MapFrom(src => DateTime.UtcNow);
                })
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.LastModifiedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.IsDeleted, opt => opt.Ignore())
                .ForMember(dest => dest.TotalAttempts, opt => opt.Ignore())
                .ForMember(dest => dest.Student, opt => opt.Ignore())
                .ForMember(dest => dest.Exercise, opt => opt.Ignore())
                .ForMember(dest => dest.AnswerDetails, opt => opt.Ignore());
        }
    }
}