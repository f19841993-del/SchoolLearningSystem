using AutoMapper;
using SchoolLearningSystem.Applicationf.DTOs.CourseDto;
using SchoolLearningSystem.Domain.Entities;
using System.Linq;

namespace SchoolLearningSystem.Applicationf.Mappings
{
    public class CourseProfile : Profile
    {
        public CourseProfile()
        {
            // 1. تحويل الكيان إلى DTO (للعرض)
            CreateMap<Course, CourseReadDto>()
                .ForMember(dest => dest.TeacherName,
                    opt => opt.MapFrom(src => src.Teacher != null ? src.Teacher.Name : string.Empty))
                .ForMember(dest => dest.CurriculumTitle,
                    opt => opt.MapFrom(src => src.Curriculum != null ? src.Curriculum.Name : string.Empty))
                .ForMember(dest => dest.StudentIds,
                    opt => opt.MapFrom(src => src.CourseStudents != null ? src.CourseStudents.Select(cs => cs.StudentId) : Enumerable.Empty<int>()));

            // 2. تحويل الـ CreateDto إلى الكيان
            CreateMap<CourseCreateDto, Course>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.LastModifiedAt, opt => opt.Ignore())
                .ForMember(dest => dest.IsDeleted, opt => opt.Ignore())
                .ForMember(dest => dest.Teacher, opt => opt.Ignore())
                .ForMember(dest => dest.Curriculum, opt => opt.Ignore())
                .ForMember(dest => dest.CourseStudents, opt => opt.Ignore())
                .ForMember(dest => dest.Lessons, opt => opt.Ignore())
                .ForMember(dest => dest.Exams, opt => opt.Ignore());
            CreateMap<CourseUpdateDto, Course>()
            // نطبق الشرط فقط على الخصائص النصية أو البسيطة
            .ForMember(dest => dest.Title,
            opt => opt.Condition(src => src.Title != null))
           .ForMember(dest => dest.Description,
           opt => opt.Condition(src => src.Description != null))
           // تجاهل الخصائص الحساسة أو المرتبطة بالنظام
          .ForMember(dest => dest.Id, opt => opt.Ignore())
    .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
    .ForMember(dest => dest.LastModifiedAt, opt => opt.Ignore())
    .ForMember(dest => dest.IsDeleted, opt => opt.Ignore())
    .ForMember(dest => dest.Teacher, opt => opt.Ignore())
    .ForMember(dest => dest.Curriculum, opt => opt.Ignore())
    .ForMember(dest => dest.CourseStudents, opt => opt.Ignore())
    .ForMember(dest => dest.Lessons, opt => opt.Ignore())
    .ForMember(dest => dest.Exams, opt => opt.Ignore());



        }
    }
}


//using AutoMapper;
//using SchoolLearningSystem.Applicationf.DTOs.CourseDto;
//using SchoolLearningSystem.Domain.Entities;

//public class CourseProfile : Profile
//{
//    public CourseProfile()
//    {
//        CreateMap<Course, CourseDto>()
//     .ForMember(dest => dest.TeacherName, opt => opt.MapFrom(src => src.Teacher != null ? src.Teacher.Name : string.Empty))
//     .ForMember(dest => dest.CurriculumTitle, opt => opt.MapFrom(src => src.Curriculum != null ? src.Curriculum.Name : string.Empty))
//     .ForMember(dest => dest.StudentIds, opt => opt.MapFrom(src => src.CourseStudents.Select(cs => cs.StudentId)))
//     .ForMember(dest => dest.Lessons, opt => opt.MapFrom(src => src.Lessons))
//     .ForMember(dest => dest.Exams, opt => opt.MapFrom(src => src.Exams));

//        CreateMap<CourseDto, Course>()
//            .ForMember(dest => dest.TeacherId, opt => opt.Ignore()) // TeacherName ما يتحول مباشرة
//            .ForMember(dest => dest.CurriculumId, opt => opt.MapFrom(src => src.CurriculumId))
//            .ForMember(dest => dest.CourseStudents, opt => opt.Ignore()) // StudentIds تنربط بالـ DbContext
//            .ForMember(dest => dest.Lessons, opt => opt.Ignore())
//            .ForMember(dest => dest.Exams, opt => opt.Ignore());

//    }
//}
