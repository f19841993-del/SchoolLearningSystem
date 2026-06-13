using AutoMapper;
using SchoolLearningSystem.Applicationf.DTOs.Question;
using SchoolLearningSystem.Domain.Entities;
using SchoolLearningSystem.Domain.Enums;
using System.Linq;

namespace SchoolLearningSystem.Applicationf.Mappings
{
    public class QuestionProfile : Profile
    {
        public QuestionProfile()
        {
            // 1. من الكيان → للعرض (Read)
            CreateMap<Question, QuestionReadDto>()
                .ForMember(dest => dest.LessonTitle, opt => opt.MapFrom(src => src.Lesson != null ? src.Lesson.Title : string.Empty))
                // بقية الحقول (Id, Text, Answer, DifficultyLevel, LessonId) تتطابق أسماؤها تلقائياً
                ;

            // 2. من الإنشاء → للكيان (Create)
            CreateMap<QuestionCreateDto, Question>()
                // الحماية الموحدة لحقول الـ BaseEntity
                .ForMember(dest => dest.Id, opt => opt.Ignore())
               
                // تجاهل العلاقات
                .ForMember(dest => dest.Lesson, opt => opt.Ignore())
                .ForMember(dest => dest.Exam, opt => opt.Ignore());

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
                // الحماية الموحدة لحقول الـ BaseEntity والعلاقات
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Lesson, opt => opt.Ignore())
                .ForMember(dest => dest.Exam, opt => opt.Ignore());
        }
    }
}