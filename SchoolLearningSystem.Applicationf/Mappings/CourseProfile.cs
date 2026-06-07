using AutoMapper;
using SchoolLearningSystem.Applicationf.DTOs.CourseDto;
using SchoolLearningSystem.Domain.Entities;

public class CourseProfile : Profile
{
    public CourseProfile()
    {
        // من الكيان إلى العرض
        CreateMap<Course, CourseReadDto>()
            .ForMember(dest => dest.TeacherName, opt => opt.MapFrom(src => src.Teacher != null ? src.Teacher.Name : string.Empty))
            .ForMember(dest => dest.CurriculumTitle, opt => opt.MapFrom(src => src.Curriculum != null ? src.Curriculum.Name : string.Empty))
            .ForMember(dest => dest.StudentIds, opt => opt.MapFrom(src => src.CourseStudents.Select(cs => cs.StudentId)))
            .ForMember(dest => dest.Lessons, opt => opt.MapFrom(src => src.Lessons))
            .ForMember(dest => dest.Exams, opt => opt.MapFrom(src => src.Exams));

        // من الإنشاء إلى الكيان
        CreateMap<CourseCreateDto, Course>()
            .ForMember(dest => dest.Teacher, opt => opt.Ignore()) // TeacherName ما يتحول مباشرة
            .ForMember(dest => dest.CourseStudents, opt => opt.Ignore()) // StudentIds تربط بالـ DbContext
            .ForMember(dest => dest.Lessons, opt => opt.Ignore())
            .ForMember(dest => dest.Exams, opt => opt.Ignore());

        // من التعديل إلى الكيان
        CreateMap<CourseUpdateDto, Course>()
            .ForMember(dest => dest.Teacher, opt => opt.Ignore())
            .ForMember(dest => dest.CourseStudents, opt => opt.Ignore())
            .ForMember(dest => dest.Lessons, opt => opt.Ignore())
            .ForMember(dest => dest.Exams, opt => opt.Ignore());
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
