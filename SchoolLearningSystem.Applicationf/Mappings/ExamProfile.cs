using AutoMapper;
using SchoolLearningSystem.Applicationf.DTOs;
using SchoolLearningSystem.Domain.Entities;
using SchoolLearningSystem.Domain.Enums;

public class ExamProfile : Profile
{
    private ExamType ParseExamType(string examTypeString)
    {
        return Enum.TryParse<ExamType>(examTypeString, true, out var examType)
            ? examType
            : ExamType.Test;
    }
    public ExamProfile()
    {

        // من Exam → ExamDto
        CreateMap<Exam, ExamDto>()
            .ForMember(dest => dest.ExamType, opt => opt.MapFrom(src => src.ExamType.ToString()))
            .ForMember(dest => dest.Questions, opt => opt.MapFrom(src => src.Questions))
            .ForMember(dest => dest.Results, opt => opt.MapFrom(src => src.Results));

        // من ExamDto → Exam
        CreateMap<ExamDto, Exam>()
    .ForMember(dest => dest.ExamType, opt => opt.MapFrom(src => ParseExamType(src.ExamType))) 
    .ForMember(dest => dest.Questions, opt => opt.Ignore()) // تنربط لاحقاً بالـ DbContext
    .ForMember(dest => dest.Results, opt => opt.Ignore());  // نفس الشي

    }
}
