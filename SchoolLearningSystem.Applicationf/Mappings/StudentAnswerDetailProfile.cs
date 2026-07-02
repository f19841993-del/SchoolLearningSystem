using AutoMapper;
using SchoolLearningSystem.Applicationf.DTOs.StudentAnswer;
using SchoolLearningSystem.Domain.Entities;
using System;

namespace SchoolLearningSystem.Applicationf.Mappings
{
    public class StudentAnswerDetailProfile : Profile
    {
        public StudentAnswerDetailProfile()
        {
            // 1. من الكيان → للعرض (Read)
            CreateMap<StudentAnswerDetail, StudentAnswerDetailReadDto>()
                .ForMember(dest => dest.StudentName, opt => opt.MapFrom(src => src.Student != null ? src.Student.Name : string.Empty))
                //.ForMember(dest => dest.QuestionText, opt => opt.MapFrom(src => src.Question != null ? src.Question.Question : string.Empty)) 
                .ForMember(dest => dest.Timestamp, opt => opt.MapFrom(src => src.CreatedAt));
            // ملاحظة: حقل Quality سيتم تحويله تلقائياً لتطابق الأسماء

            // 2. من الإنشاء → للكيان (Create)
            CreateMap<StudentAnswerDetailCreateDto, StudentAnswerDetail>()
                // الحماية الموحدة لحقول الـ BaseEntity
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.LastModifiedAt, opt => opt.Ignore())
                .ForMember(dest => dest.IsDeleted, opt => opt.Ignore())
                // تجاهل العلاقات
                .ForMember(dest => dest.Student, opt => opt.Ignore())
                .ForMember(dest => dest.Question, opt => opt.Ignore());
            // ملاحظة: حقل Quality سيتم تحويله تلقائياً لتطابق الأسماء

            // 3. من التعديل → للكيان (Update) - مع شرط التحديث الجزئي
            CreateMap<StudentAnswerDetailUpdateDto, StudentAnswerDetail>()
                .ForMember(dest => dest.SelectedAnswer, opt => {
                    opt.Condition(src => src.SelectedAnswer != null);
                    opt.MapFrom(src => src.SelectedAnswer);
                })
                .ForMember(dest => dest.IsCorrect, opt => {
                    opt.Condition(src => src.IsCorrect.HasValue);
                    opt.MapFrom(src => src.IsCorrect);
                })
                // 🌟 التعديل الجديد: إضافة شرط الحماية لحقل التقييم (Quality)
                .ForMember(dest => dest.Quality, opt => {
                    opt.Condition(src => src.Quality.HasValue);
                    opt.MapFrom(src => src.Quality);
                })
                // الحماية الموحدة لحقول الـ BaseEntity والعلاقات
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.LastModifiedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.IsDeleted, opt => opt.Ignore())
                .ForMember(dest => dest.Student, opt => opt.Ignore())
                .ForMember(dest => dest.Question, opt => opt.Ignore());
        }
    }
}