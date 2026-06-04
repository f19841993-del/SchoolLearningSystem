using AutoMapper;
using SchoolLearningSystem.Domain.Entities;
using SchoolLearningSystem.Application.DTOs;

namespace SchoolLearningSystem.Application.Mappings
{
    public class TeacherProfile : Profile
    {
        public TeacherProfile()
        {
            CreateMap<Teacher, TeacherDto>().ReverseMap();
        }
    }
}
