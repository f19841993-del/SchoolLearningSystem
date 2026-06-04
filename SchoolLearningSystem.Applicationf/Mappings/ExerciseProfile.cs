using AutoMapper;
using SchoolLearningSystem.Applicationf.DTOs;
using SchoolLearningSystem.Domain.Entities;

public class ExerciseProfile : Profile
{
    public ExerciseProfile()
    {
        // من Exercise → ExerciseDto
        CreateMap<Exercise, ExerciseDto>()
            .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Question)) // نخلي الـ Title يساوي السؤال
            .ForMember(dest => dest.MemorizeSessions, opt => opt.MapFrom(src => src.MemorizeSessions));

        // من ExerciseDto → Exercise


       
        CreateMap<ExerciseDto, Exercise>()
            .ForMember(dest => dest.Question, opt => opt.MapFrom(src => string.IsNullOrEmpty(src.Question) ? src.Title : src.Question))
            //.ForMember(dest => dest.Question, opt => opt.MapFrom(src => src.Question))
            .ForMember(dest => dest.Answer, opt => opt.MapFrom(src => src.Answer))
            .ForMember(dest => dest.MemorizeSessions, opt => opt.Ignore()); // تنربط لاحقاً بالـ DbContext
    }
}
