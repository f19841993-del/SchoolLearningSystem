using AutoMapper;
using SchoolLearningSystem.Domain.Entities;
using SchoolLearningSystem.Applicationf.DTOs.Lesson;

public class LessonProfile : Profile
{
    public LessonProfile()
    {
        // من الكيان → للعرض
        CreateMap<Lesson, LessonReadDto>()
            .ForMember(dest => dest.CourseTitle, opt => opt.MapFrom(src => src.Course.Title))
            .ForMember(dest => dest.Exams, opt => opt.MapFrom(src => src.Exams))
            .ForMember(dest => dest.Exercises, opt => opt.MapFrom(src => src.Exercises))
            .ForMember(dest => dest.Results, opt => opt.MapFrom(src => src.Results))
            .ForMember(dest => dest.MemorizeSessions, opt => opt.MapFrom(src => src.MemorizeSessions))
            .ForMember(dest => dest.Questions, opt => opt.MapFrom(src => src.Questions));

        // من الإنشاء → للكيان
        CreateMap<LessonCreateDto, Lesson>();

        // من التعديل → للكيان
        CreateMap<LessonUpdateDto, Lesson>();
    }
}
