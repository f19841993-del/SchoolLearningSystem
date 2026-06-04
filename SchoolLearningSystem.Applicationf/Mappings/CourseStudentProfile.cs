using AutoMapper;
using SchoolLearningSystem.Applicationf.DTOs;
using SchoolLearningSystem.Domain.Entities;

public class CourseStudentProfile : Profile
{
    public CourseStudentProfile()
    {
        // من CourseStudent → CourseStudentDto
        CreateMap<CourseStudent, CourseStudentDto>()
            .ForMember(dest => dest.CourseTitle, opt => opt.MapFrom(src => src.Course != null ? src.Course.Title : string.Empty))
            .ForMember(dest => dest.StudentName, opt => opt.MapFrom(src => src.Student != null ? src.Student.Name : string.Empty))
            .ForMember(dest => dest.EnrollmentDate, opt => opt.MapFrom(src => src.EnrolledAt));

        // من CourseStudentDto → CourseStudent
        CreateMap<CourseStudentDto, CourseStudent>()
            .ForMember(dest => dest.Course, opt => opt.Ignore())   // CourseTitle ما يتحول مباشرة
            .ForMember(dest => dest.Student, opt => opt.Ignore()) // StudentName ما يتحول مباشرة
            .ForMember(dest => dest.EnrolledAt, opt => opt.MapFrom(src => src.EnrollmentDate))
            .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => true)); // افتراضيًا نخلي الطالب فعال
    }
}
