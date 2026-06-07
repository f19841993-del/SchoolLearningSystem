using AutoMapper;
using SchoolLearningSystem.Applicationf.DTOs.Teacher;
using SchoolLearningSystem.Domain.Entities;

public class TeacherProfile : Profile
{
    public TeacherProfile()
    {
        // من Teacher → TeacherReadDto (للعرض)
        CreateMap<Teacher, TeacherReadDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.Subject, opt => opt.MapFrom(src => src.Subject))
            .ForMember(dest => dest.Courses, opt => opt.MapFrom(src => src.Courses));

        // من TeacherCreateDto → Teacher (للإضافة)
        CreateMap<TeacherCreateDto, Teacher>()
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.Subject, opt => opt.MapFrom(src => src.Subject))
            .ForMember(dest => dest.Courses, opt => opt.Ignore()); // CourseIds تتحول لاحقاً بالـ Service

        // من TeacherUpdateDto → Teacher (للتحديث)
        CreateMap<TeacherUpdateDto, Teacher>()
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.Subject, opt => opt.MapFrom(src => src.Subject))
            .ForMember(dest => dest.Courses, opt => opt.Ignore()); // نفس الشي

        // الاتجاه العكسي (Teacher → TeacherCreateDto / TeacherUpdateDto)
        CreateMap<Teacher, TeacherCreateDto>()
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.Subject, opt => opt.MapFrom(src => src.Subject))
            .ForMember(dest => dest.CourseIds, opt => opt.MapFrom(src => src.Courses.Select(c => c.Id)));

        CreateMap<Teacher, TeacherUpdateDto>()
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.Subject, opt => opt.MapFrom(src => src.Subject))
            .ForMember(dest => dest.CourseIds, opt => opt.MapFrom(src => src.Courses.Select(c => c.Id)));
    }
}
