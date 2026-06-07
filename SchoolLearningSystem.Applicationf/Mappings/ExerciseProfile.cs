using AutoMapper;
using SchoolLearningSystem.Domain.Entities;
using SchoolLearningSystem.Applicationf.DTOs.Exercise;

public class ExerciseProfile : Profile
{
    public ExerciseProfile()
    {
        // من الكيان → للعرض
        CreateMap<Exercise, ExerciseReadDto>()
            .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Question)) // مثال: ممكن نخلي الـ Title = Question
            .ForMember(dest => dest.MemorizeSessions, opt => opt.MapFrom(src => src.MemorizeSessions));

        // من الإنشاء → للكيان
        CreateMap<ExerciseCreateDto, Exercise>();

        // من التعديل → للكيان
        CreateMap<ExerciseUpdateDto, Exercise>();
    }
}
