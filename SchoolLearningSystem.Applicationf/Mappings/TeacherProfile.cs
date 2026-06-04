using AutoMapper;
using SchoolLearningSystem.Applicationf.DTOs;
using SchoolLearningSystem.Domain.Entities;

public class TeacherProfile : Profile
{
    public TeacherProfile()
    {
        // من Teacher → TeacherDto
        CreateMap<Teacher, TeacherDto>()
            .ForMember(dest => dest.Courses, opt => opt.MapFrom(src => src.Courses));

        // من TeacherDto → Teacher
        CreateMap<TeacherDto, Teacher>()
            .ForMember(dest => dest.Courses, opt => opt.Ignore()); // الكورسات تنربط لاحقاً بالـ DbContext
    }
}
