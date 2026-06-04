using AutoMapper;
using SchoolLearningSystem.Applicationf.DTOs;
using SchoolLearningSystem.Domain.Entities;

public class LessonProfile : Profile
{
    public LessonProfile()
    {
        // من Lesson → LessonDto
        CreateMap<Lesson, LessonDto>()
            .ForMember(dest => dest.CourseTitle, opt => opt.MapFrom(src => src.Course != null ? src.Course.Title : string.Empty))
            .ForMember(dest => dest.GradeLevel, opt => opt.MapFrom(src => src.Course != null ? src.Course.Curriculum.GradeLevel.ToString() : string.Empty))
            .ForMember(dest => dest.Exams, opt => opt.MapFrom(src => src.Exams))
            .ForMember(dest => dest.Exercises, opt => opt.MapFrom(src => src.Exercises))
            .ForMember(dest => dest.Results, opt => opt.MapFrom(src => src.Results))
            .ForMember(dest => dest.MemorizeSessions, opt => opt.MapFrom(src => src.MemorizeSessions))
            .ForMember(dest => dest.Questions, opt => opt.MapFrom(src => src.Questions)); // 🔹 إضافة الأسئلة

        // من LessonDto → Lesson
        CreateMap<LessonDto, Lesson>()
            .ForMember(dest => dest.Course, opt => opt.Ignore()) // CourseTitle ما يتحول مباشرة
            .ForMember(dest => dest.Exams, opt => opt.Ignore())  // تنربط لاحقاً بالـ DbContext
            .ForMember(dest => dest.Exercises, opt => opt.Ignore())
            .ForMember(dest => dest.Results, opt => opt.Ignore())
            .ForMember(dest => dest.MemorizeSessions, opt => opt.Ignore())
            .ForMember(dest => dest.Questions, opt => opt.Ignore()); // 🔹 الأسئلة تنربط لاحقاً بالـ DbContext
    }
}
