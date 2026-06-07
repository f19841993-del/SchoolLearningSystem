using AutoMapper;
using SchoolLearningSystem.Applicationf.DTOs.ExamDto;
using SchoolLearningSystem.Domain.Entities;
using SchoolLearningSystem.Domain.Enums;

public class ExamProfile : Profile
{
    public ExamProfile()
    {
        // من الكيان → للعرض
        CreateMap<Exam, ExamReadDto>()
            .ForMember(dest => dest.ExamType, opt => opt.MapFrom(src => src.ExamType.ToString()))
            .ForMember(dest => dest.Questions, opt => opt.MapFrom(src => src.Questions))
            .ForMember(dest => dest.Results, opt => opt.MapFrom(src => src.Results));

        // من الإنشاء → للكيان
        CreateMap<ExamCreateDto, Exam>()
            .ForMember(dest => dest.ExamType, opt => opt.MapFrom(src => Enum.Parse<ExamType>(src.ExamType)))
            .ForMember(dest => dest.Questions, opt => opt.Ignore())
            .ForMember(dest => dest.Results, opt => opt.Ignore());

        // من التعديل → للكيان
        CreateMap<ExamUpdateDto, Exam>()
            .ForMember(dest => dest.ExamType, opt => opt.MapFrom(src => Enum.Parse<ExamType>(src.ExamType)))
            .ForMember(dest => dest.Questions, opt => opt.Ignore())
            .ForMember(dest => dest.Results, opt => opt.Ignore());
    }
}
