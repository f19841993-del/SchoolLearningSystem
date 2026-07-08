using AutoMapper;
using SchoolLearningSystem.Applicationf.DTOs.StudentAnswer;
using SchoolLearningSystem.Domain.Entities;

namespace SchoolLearningSystem.Applicationf.Mappings
{
    public class StudentAnswerDetailProfile : Profile
    {
        public StudentAnswerDetailProfile()
        {
            // 1. من الكيان → للعرض (Read)
            // MemorizeSessionId, SelectedAnswer, IsCorrect, Quality, TimeTakenInSeconds,
            // CreatedAt تتطابق أسماؤها تلقائياً
            CreateMap<StudentAnswerDetail, StudentAnswerDetailReadDto>()
                .ForMember(dest => dest.StudentName,
                    opt => opt.MapFrom(src => src.Student != null ? src.Student.Name : string.Empty))
                // ✅ تصحيح: خاصية السؤال بالـ Entity اسمها Text وليس Question
                .ForMember(dest => dest.QuestionText,
                    opt => opt.MapFrom(src => src.Question != null ? src.Question.Text : string.Empty));

            // 2. من الإنشاء → للكيان (Create)
            // ✅ MemorizeSessionId يتطابق تلقائياً (أُضيف كحقل إجباري بالـ DTO -
            // راجع dtos_review_report.md قسم 3.5)
            CreateMap<StudentAnswerDetailCreateDto, StudentAnswerDetail>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.LastModifiedAt, opt => opt.Ignore())
                .ForMember(dest => dest.IsDeleted, opt => opt.Ignore())
                .ForMember(dest => dest.Student, opt => opt.Ignore())
                .ForMember(dest => dest.Question, opt => opt.Ignore())
                .ForMember(dest => dest.MemorizeSession, opt => opt.Ignore());

            // 3. من التعديل → للكيان (Update)
            CreateMap<StudentAnswerDetailUpdateDto, StudentAnswerDetail>()
                .ForMember(dest => dest.SelectedAnswer, opt => {
                    opt.Condition(src => src.SelectedAnswer != null);
                    opt.MapFrom(src => src.SelectedAnswer);
                })
                .ForMember(dest => dest.IsCorrect, opt => {
                    opt.Condition(src => src.IsCorrect.HasValue);
                    opt.MapFrom(src => src.IsCorrect);
                })
                .ForMember(dest => dest.Quality, opt => {
                    opt.Condition(src => src.Quality.HasValue);
                    opt.MapFrom(src => src.Quality);
                })
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.LastModifiedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.IsDeleted, opt => opt.Ignore())
                .ForMember(dest => dest.Student, opt => opt.Ignore())
                .ForMember(dest => dest.Question, opt => opt.Ignore())
                // MemorizeSessionId غير قابل للتعديل عمداً - راجع StudentAnswerDetailUpdateDto
                .ForMember(dest => dest.MemorizeSession, opt => opt.Ignore());
        }
    }
}