using AutoMapper;
using SchoolLearningSystem.Applicationf.DTOs.MemorizeSession;
using SchoolLearningSystem.Domain.Entities;
using System;
using System.Linq;

namespace SchoolLearningSystem.Applicationf.Mappings
{
    public class MemorizeSessionProfile : Profile
    {
        public MemorizeSessionProfile()
        {
            // 1. من الكيان → للعرض (Read)
            CreateMap<MemorizeSession, MemorizeSessionReadDto>()
                .ForMember(dest => dest.StudentName, opt => opt.MapFrom(src => src.Student != null ? src.Student.Name : string.Empty))
                .ForMember(dest => dest.LessonTitle, opt => opt.MapFrom(src => src.Lesson != null ? src.Lesson.Title : string.Empty))
                .ForMember(dest => dest.Date, opt => opt.MapFrom(src => src.CreatedAt));

            // 2. من الإنشاء → للكيان (Create)
            CreateMap<MemorizeSessionCreateDto, MemorizeSession>()
                // الحماية الموحدة لحقول الـ BaseEntity
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.UtcNow)) // تاريخ الجلسة لحظة الإنشاء
                .ForMember(dest => dest.LastModifiedAt, opt => opt.Ignore())
                .ForMember(dest => dest.IsDeleted, opt => opt.Ignore())
                // تجاهل العلاقات
                .ForMember(dest => dest.Student, opt => opt.Ignore())
                .ForMember(dest => dest.Lesson, opt => opt.Ignore())
                .ForMember(dest => dest.Exercise, opt => opt.Ignore());

            // 3. من التعديل → للكيان (Update)
            CreateMap<MemorizeSessionUpdateDto, MemorizeSession>()
                .ForMember(dest => dest.TotalAttempts, opt => {
                    opt.Condition(src => src.Attempts.HasValue);
                    opt.MapFrom(src => src.Attempts);
                })
                .ForMember(dest => dest.SuccessRate, opt => {
                    opt.Condition(src => src.SuccessRate.HasValue);
                    opt.MapFrom(src => src.SuccessRate);
                })
                // الحماية الموحدة لحقول الـ BaseEntity والعلاقات
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.LastModifiedAt, opt => opt.MapFrom(src => DateTime.UtcNow)) // تحديث وقت التعديل
                .ForMember(dest => dest.IsDeleted, opt => opt.Ignore())
                .ForMember(dest => dest.Student, opt => opt.Ignore())
                .ForMember(dest => dest.Lesson, opt => opt.Ignore())
                .ForMember(dest => dest.Exercise, opt => opt.Ignore());
        }
    }
}