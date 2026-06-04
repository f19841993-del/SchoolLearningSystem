using AutoMapper;
using SchoolLearningSystem.Applicationf.DTOs;
using SchoolLearningSystem.Domain.Entities;
using SchoolLearningSystem.Domain.Enums;

public class QuestionProfile : Profile
{
    // دالة خاصة لتحويل النص إلى Enum
    private DifficultyLevel ParseDifficulty(string difficultyString)
    {
        return Enum.TryParse<DifficultyLevel>(difficultyString, true, out var difficulty)
            ? difficulty
            : DifficultyLevel.Easy; // نخلي Easy كقيمة افتراضية إذا فشل التحويل
    }

    public QuestionProfile()
    {
        // من Question → QuestionDto
        CreateMap<Question, QuestionDto>()
            .ForMember(dest => dest.DifficultyLevel, opt => opt.MapFrom(src => src.DifficultyLevel.ToString()))
            .ForMember(dest => dest.ExamTitle, opt => opt.MapFrom(src => src.Exam != null ? src.Exam.Title : string.Empty))
            .ForMember(dest => dest.QuestionNumber, opt => opt.Ignore()); // يتحدد لاحقاً بالمنطق

        // من QuestionDto → Question
        CreateMap<QuestionDto, Question>()
            .ForMember(dest => dest.DifficultyLevel, opt => opt.MapFrom(src => ParseDifficulty(src.DifficultyLevel)))
            .ForMember(dest => dest.Exam, opt => opt.Ignore()); // ExamTitle ما يتحول مباشرة
    }
}
