using AutoMapper;
using SchoolLearningSystem.Applicationf.DTOs.Result;
using SchoolLearningSystem.Domain.Entities;
using System;

namespace SchoolLearningSystem.Applicationf.Mappings
{
    public class ResultProfile : Profile
    {
        public ResultProfile()
        {
            // 1. من الكيان → للعرض (Read)
            CreateMap<Result, ResultReadDto>()
                .ForMember(dest => dest.StudentName, opt => opt.MapFrom(src => src.Student != null ? src.Student.Name : string.Empty))
                .ForMember(dest => dest.LessonTitle, opt => opt.MapFrom(src => src.Lesson != null ? src.Lesson.Title : string.Empty))
                .ForMember(dest => dest.ExamTitle, opt => opt.MapFrom(src => src.Exam != null ? src.Exam.Title : string.Empty))
                .ForMember(dest => dest.Date, opt => opt.MapFrom(src => src.CreatedAt))
                .ForMember(dest => dest.ResultType, opt => opt.MapFrom(src =>
                    src.Exam != null ? src.Exam.ExamType.ToString() : src.ResultType));

            // 2. من الإنشاء → للكيان (Create)
            CreateMap<ResultCreateDto, Result>()
                // الحماية الموحدة لحقول الـ BaseEntity
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.LastModifiedAt, opt => opt.Ignore())
                .ForMember(dest => dest.IsDeleted, opt => opt.Ignore())
                // تجاهل العلاقات (يتم ربطها يدوياً بالـ IDs في الـ Service)
                .ForMember(dest => dest.Student, opt => opt.Ignore())
                .ForMember(dest => dest.Lesson, opt => opt.Ignore())
                .ForMember(dest => dest.Exam, opt => opt.Ignore());

            // 3. من التعديل → للكيان (Update)
            CreateMap<ResultUpdateDto, Result>()
                .ForMember(dest => dest.ResultType, opt => {
                    opt.Condition(src => src.ResultType != null);
                    opt.MapFrom(src => src.ResultType);
                })
                .ForMember(dest => dest.Score, opt => {
                    opt.Condition(src => src.Score.HasValue);
                    opt.MapFrom(src => src.Score);
                })
                // تحديث وقت التعديل
                .ForMember(dest => dest.LastModifiedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
                // الحماية الموحدة لحقول الـ BaseEntity والعلاقات
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.IsDeleted, opt => opt.Ignore())
                .ForMember(dest => dest.Student, opt => opt.Ignore())
                .ForMember(dest => dest.Lesson, opt => opt.Ignore())
                .ForMember(dest => dest.Exam, opt => opt.Ignore());
        }
    }
}