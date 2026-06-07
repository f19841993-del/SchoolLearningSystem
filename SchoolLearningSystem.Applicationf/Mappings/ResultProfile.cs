using AutoMapper;
using SchoolLearningSystem.Applicationf.DTOs.Result;
using SchoolLearningSystem.Domain.Entities;

public class ResultProfile : Profile
{
    public ResultProfile()
    {
        // من Result → ResultReadDto (للعرض)
        CreateMap<Result, ResultReadDto>()
            .ForMember(dest => dest.StudentName, opt => opt.MapFrom(src => src.Student != null ? src.Student.Name : string.Empty))
            .ForMember(dest => dest.LessonTitle, opt => opt.MapFrom(src => src.Lesson != null ? src.Lesson.Title : string.Empty))
            .ForMember(dest => dest.ExamTitle, opt => opt.MapFrom(src => src.Exam != null ? src.Exam.Title : string.Empty))
            .ForMember(dest => dest.ResultType, opt => opt.MapFrom(src =>
                src.Exam != null ? src.Exam.ExamType.ToString() : src.ResultType // إذا مرتبط بامتحان نجيب نوعه، إذا لا نستخدم النوع المخزن
            ));

        // من ResultCreateDto → Result (للإضافة)
        CreateMap<ResultCreateDto, Result>()
            .ForMember(dest => dest.Student, opt => opt.Ignore())
            .ForMember(dest => dest.Lesson, opt => opt.Ignore())
            .ForMember(dest => dest.Exam, opt => opt.Ignore());

        // من ResultUpdateDto → Result (للتحديث)
        CreateMap<ResultUpdateDto, Result>()
            .ForMember(dest => dest.Student, opt => opt.Ignore())
            .ForMember(dest => dest.Lesson, opt => opt.Ignore())
            .ForMember(dest => dest.Exam, opt => opt.Ignore());
    }
}
