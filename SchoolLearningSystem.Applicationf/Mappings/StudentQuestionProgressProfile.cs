using AutoMapper;
using SchoolLearningSystem.Applicationf.DTOs.CourseDto;
using SchoolLearningSystem.Applicationf.DTOs.Lesson;
using SchoolLearningSystem.Applicationf.DTOs.Question;
using SchoolLearningSystem.Applicationf.DTOs.Result;
using SchoolLearningSystem.Applicationf.DTOs.Student;
using SchoolLearningSystem.Applicationf.DTOs.StudentAnswer;
using SchoolLearningSystem.Applicationf.DTOs.StudentQuestionProgress;
using SchoolLearningSystem.Domain.Entities;

namespace SchoolLearningSystem.Applicationf.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // 1. المناهج والكورسات والدروس
            CreateMap<Course, CourseReadDto>()
                .ForMember(dest => dest.TeacherName, opt => opt.MapFrom(src => src.Teacher.Name));
            CreateMap<CourseCreateDto, Course>();
            CreateMap<CourseUpdateDto, Course>().ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            CreateMap<Lesson, LessonReadDto>()
                .ForMember(dest => dest.CourseTitle, opt => opt.MapFrom(src => src.Course.Title));
            CreateMap<LessonCreateDto, Lesson>();
            CreateMap<LessonUpdateDto, Lesson>().ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            // 2. الأسئلة والتمارين (البنك المعرفي)
            CreateMap<Question, QuestionReadDto>()
                .ForMember(dest => dest.LessonTitle, opt => opt.MapFrom(src => src.Lesson.Title));
            CreateMap<QuestionCreateDto, Question>();
            CreateMap<QuestionUpdateDto, Question>().ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            // 3. الطلاب والنتائج
            CreateMap<Student, StudentReadDto>();
            CreateMap<StudentCreateDto, Student>();
            CreateMap<StudentUpdateDto, Student>().ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            CreateMap<Result, ResultReadDto>()
                .ForMember(dest => dest.StudentName, opt => opt.MapFrom(src => src.Student.Name))
                .ForMember(dest => dest.LessonTitle, opt => opt.MapFrom(src => src.Lesson.Title));
            CreateMap<ResultCreateDto, Result>();

            // 4. نظام التكرار المتباعد (SRS - قلب النظام الذكي)
            CreateMap<StudentQuestionProgress, StudentQuestionProgressReadDto>()
                // التسطيح الذكي: جلب نص السؤال مباشرة دون الحاجة لـ Query إضافي
                .ForMember(dest => dest.QuestionText, opt => opt.MapFrom(src => src.Question));
            CreateMap<StudentQuestionProgressCreateDto, StudentQuestionProgress>();
            CreateMap<StudentQuestionProgressUpdateDto, StudentQuestionProgress>().ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            CreateMap<StudentAnswerDetail, StudentAnswerDetailReadDto>();
            CreateMap<StudentAnswerDetailCreateDto, StudentAnswerDetail>();
        }
    }
}