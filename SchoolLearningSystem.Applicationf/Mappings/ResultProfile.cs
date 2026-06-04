using AutoMapper;
using SchoolLearningSystem.Applicationf.DTOs;
using SchoolLearningSystem.Domain.Entities;

public class ResultProfile : Profile
{
    public ResultProfile()
    {
        // من Result → ResultDto
        CreateMap<Result, ResultDto>()
            .ForMember(dest => dest.StudentName, opt => opt.MapFrom(src => src.Student != null ? src.Student.Name : string.Empty))
            .ForMember(dest => dest.LessonTitle, opt => opt.MapFrom(src => src.Lesson != null ? src.Lesson.Title : string.Empty))
            .ForMember(dest => dest.ExamTitle, opt => opt.MapFrom(src => src.Exam != null ? src.Exam.Title : string.Empty))
            .ForMember(dest => dest.ResultType, opt => opt.MapFrom(src =>
                src.Exam != null ? src.Exam.ExamType.ToString() : "Homework" // إذا مرتبط بامتحان نجيب نوعه، إذا لا نخلي Homework
            ));

        // من ResultDto → Result
        CreateMap<ResultDto, Result>()
            .ForMember(dest => dest.Student, opt => opt.Ignore())   // StudentName ما يتحول مباشرة
            .ForMember(dest => dest.Lesson, opt => opt.Ignore())    // LessonTitle ما يتحول مباشرة
            .ForMember(dest => dest.Exam, opt => opt.Ignore());     // ExamTitle ما يتحول مباشرة
    }
}
