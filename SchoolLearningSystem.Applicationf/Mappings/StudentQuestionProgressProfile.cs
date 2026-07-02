using AutoMapper;
using SchoolLearningSystem.Applicationf.DTOs.StudentQuestionProgress;
using SchoolLearningSystem.Domain.Entities;

namespace SchoolLearningSystem.Applicationf.Mappings
{
    // ✅ اسم الكلاس يطابق اسم الملف تماماً
    public class StudentQuestionProgressProfile : Profile
    {
        public StudentQuestionProgressProfile()
        {
            CreateMap<StudentQuestionProgress, StudentQuestionProgressReadDto>()
                .ForMember(dest => dest.QuestionText, opt => opt.MapFrom(src => src.Question.Text));

            CreateMap<StudentQuestionProgressCreateDto, StudentQuestionProgress>();

            CreateMap<StudentQuestionProgressUpdateDto, StudentQuestionProgress>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }
}