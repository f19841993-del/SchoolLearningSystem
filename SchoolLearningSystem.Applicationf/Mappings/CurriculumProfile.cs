using AutoMapper;
using SchoolLearningSystem.Applicationf.DTOs.CurriculumDto;
using SchoolLearningSystem.Domain.Entities;
using SchoolLearningSystem.Domain.Enums;

public class CurriculumProfile : Profile
{
    public CurriculumProfile()
    {
        // 1. من الكيان → للعرض
        CreateMap<Curriculum, CurriculumReadDto>()
            .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.Level, opt => opt.MapFrom(src => src.GradeLevel.ToString())) // تصحيح الاسم هنا
            .ForMember(dest => dest.Courses, opt => opt.MapFrom(src => src.Courses));

        // 2. من الإنشاء → للكيان
        CreateMap<CurriculumCreateDto, Curriculum>()
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Title))
            .ForMember(dest => dest.GradeLevel, opt => opt.MapFrom(src => Enum.Parse<GradeLevel>(src.Level)))
            .ForMember(dest => dest.Courses, opt => opt.Ignore());

        // 3. من التعديل → للكيان (مع الحماية من Null Overwrite)
        CreateMap<CurriculumUpdateDto, Curriculum>()
            .ForMember(dest => dest.Name, opt => {
                opt.Condition(src => src.Title != null);
                opt.MapFrom(src => src.Title);
            })
            .ForMember(dest => dest.GradeLevel, opt => {
                opt.Condition(src => src.Level != null);
                opt.MapFrom(src => Enum.Parse<GradeLevel>(src.Level));
            })
            .ForMember(dest => dest.Courses, opt => opt.Ignore());
    }
}
