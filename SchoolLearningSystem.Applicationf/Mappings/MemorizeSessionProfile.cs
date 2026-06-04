using AutoMapper;
using SchoolLearningSystem.Applicationf.DTOs;
using SchoolLearningSystem.Domain.Entities;

public class MemorizeSessionProfile : Profile
{
    public MemorizeSessionProfile()
    {
        // من MemorizeSession → MemorizeSessionDto
        CreateMap<MemorizeSession, MemorizeSessionDto>()
            .ForMember(dest => dest.StudentName, opt => opt.MapFrom(src => src.Student != null ? src.Student.Name : string.Empty))
            .ForMember(dest => dest.LessonTitle, opt => opt.MapFrom(src => src.Lesson != null ? src.Lesson.Title : string.Empty))
            .ForMember(dest => dest.ExerciseTitle, opt => opt.MapFrom(src => src.Exercise != null ? src.Exercise.Question : string.Empty))
            .ForMember(dest => dest.ExerciseId, opt => opt.MapFrom(src => src.ExerciseId.HasValue ? src.ExerciseId.Value : 0));

        // من MemorizeSessionDto → MemorizeSession
        CreateMap<MemorizeSessionDto, MemorizeSession>()
            .ForMember(dest => dest.Student, opt => opt.Ignore())   // StudentName ما يتحول مباشرة
            .ForMember(dest => dest.Lesson, opt => opt.Ignore())    // LessonTitle ما يتحول مباشرة
            .ForMember(dest => dest.Exercise, opt => opt.Ignore()); // ExerciseTitle ما يتحول مباشرة
    }
}
