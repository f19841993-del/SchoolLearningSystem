using AutoMapper;
using SchoolLearningSystem.Applicationf.DTOs.Curriculum;
using SchoolLearningSystem.Domain.Entities;
using SchoolLearningSystem.Domain.Enums;

public class CurriculumProfile : Profile
{
    public CurriculumProfile()
    {
        // من الكيان → للعرض
        CreateMap<Curriculum, CurriculumReadDto>()
            .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.GradeLevel, opt => opt.MapFrom(src => src.GradeLevel.ToString()))
            .ForMember(dest => dest.Courses, opt => opt.MapFrom(src => src.Courses));

        // من الإنشاء → للكيان
        CreateMap<CurriculumCreateDto, Curriculum>()
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Title))
            .ForMember(dest => dest.GradeLevel, opt => opt.MapFrom(src => Enum.Parse<GradeLevel>(src.GradeLevel)))
            .ForMember(dest => dest.Courses, opt => opt.Ignore());

        // من التعديل → للكيان
        CreateMap<CurriculumUpdateDto, Curriculum>()
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Title))
            .ForMember(dest => dest.GradeLevel, opt => opt.MapFrom(src => Enum.Parse<GradeLevel>(src.GradeLevel)))
            .ForMember(dest => dest.Courses, opt => opt.Ignore());
    }
}
