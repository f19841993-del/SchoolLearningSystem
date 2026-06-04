using AutoMapper;
using SchoolLearningSystem.Applicationf.DTOs;
using SchoolLearningSystem.Domain.Entities;
using SchoolLearningSystem.Domain.Enums;

public class CurriculumProfile : Profile
{

    // دالة خاصة للتحويل
    private GradeLevel ParseGradeLevel(string gradeLevelString)
    {
        return Enum.TryParse<GradeLevel>(gradeLevelString, true, out var grade)
            ? grade
            : GradeLevel.Third; // نخلي Unknown كقيمة افتراضية إذا النص غلط
    }
    public CurriculumProfile()
    {
        // من Curriculum → CurriculumDto
        CreateMap<Curriculum, CurriculumDto>()
            .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.GradeLevel, opt => opt.MapFrom(src => src.GradeLevel.ToString()))
            .ForMember(dest => dest.Courses, opt => opt.MapFrom(src => src.Courses));

        // من CurriculumDto → Curriculum
        CreateMap<CurriculumDto, Curriculum>()
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Title))
            .ForMember(dest => dest.GradeLevel, opt => opt.MapFrom(src => ParseGradeLevel(src.GradeLevel)))
            .ForMember(dest => dest.Courses, opt => opt.Ignore()); // الكورسات تنربط لاحقاً بالـ DbContext
    }
}
