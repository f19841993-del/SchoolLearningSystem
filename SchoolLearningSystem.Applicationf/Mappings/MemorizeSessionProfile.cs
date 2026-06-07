using AutoMapper;
using SchoolLearningSystem.Domain.Entities;
using SchoolLearningSystem.Applicationf.DTOs.MemorizeSession;

public class MemorizeSessionProfile : Profile
{
    public MemorizeSessionProfile()
    {
        // من الكيان → للعرض
        CreateMap<MemorizeSession, MemorizeSessionReadDto>()
            .ForMember(dest => dest.StudentName, opt => opt.MapFrom(src => src.Student.Name))
            .ForMember(dest => dest.LessonTitle, opt => opt.MapFrom(src => src.Lesson.Title))
            .ForMember(dest => dest.ExerciseTitle, opt => opt.MapFrom(src => src.Exercise.Title));

        // من الإنشاء → للكيان
        CreateMap<MemorizeSessionCreateDto, MemorizeSession>();

        // من التعديل → للكيان
        CreateMap<MemorizeSessionUpdateDto, MemorizeSession>();
    }
}
