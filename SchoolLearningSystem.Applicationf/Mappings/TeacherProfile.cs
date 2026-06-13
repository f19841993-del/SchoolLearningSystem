using AutoMapper;
using SchoolLearningSystem.Applicationf.DTOs.Teacher;
using SchoolLearningSystem.Domain.Entities;
using System.Linq;
using System;

namespace SchoolLearningSystem.Applicationf.Mappings
{
    public class TeacherProfile : Profile
    {
        public TeacherProfile()
        {
            // 1. من الكيان → للعرض
            CreateMap<Teacher, TeacherReadDto>()
                .ForMember(dest => dest.CourseTitles, opt => opt.MapFrom(src => src.Courses.Select(c => c.Title).ToList()));

            // 2. من الإنشاء → للكيان
            CreateMap<TeacherCreateDto, Teacher>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.LastModifiedAt, opt => opt.Ignore())
                .ForMember(dest => dest.IsDeleted, opt => opt.Ignore())
                .ForMember(dest => dest.Courses, opt => opt.Ignore());

            // 3. من التعديل → للكيان (مع الحماية من Null Overwrite)
            CreateMap<TeacherUpdateDto, Teacher>()
                .ForMember(dest => dest.Name, opt => {
                    opt.Condition(src => src.Name != null);
                    opt.MapFrom(src => src.Name);
                })
                .ForMember(dest => dest.Subject, opt => {
                    opt.Condition(src => src.Subject != null);
                    opt.MapFrom(src => src.Subject);
                })
                .ForMember(dest => dest.Bio, opt => {
                    opt.Condition(src => src.Bio != null);
                    opt.MapFrom(src => src.Bio);
                })
                .ForMember(dest => dest.ProfileImage, opt => {
                    opt.Condition(src => src.ProfileImage != null);
                    opt.MapFrom(src => src.ProfileImage);
                })
                // الحماية الموحدة لحقول الـ BaseEntity
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.LastModifiedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.IsDeleted, opt => opt.Ignore())
                .ForMember(dest => dest.Courses, opt => opt.Ignore());
        }
    }
}