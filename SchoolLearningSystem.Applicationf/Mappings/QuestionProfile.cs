using AutoMapper;
using SchoolLearningSystem.Applicationf.DTOs.Question;
using SchoolLearningSystem.Domain.Entities;
using SchoolLearningSystem.Domain.Enums;

public class QuestionProfile : Profile
{
    private DifficultyLevel ParseDifficulty(string difficultyString)
    {
        return Enum.TryParse<DifficultyLevel>(difficultyString, true, out var difficulty)
            ? difficulty
            : DifficultyLevel.Easy;
    }

    public QuestionProfile()
    {
        // من Question → QuestionReadDto
        CreateMap<Question, QuestionReadDto>()
            .ForMember(dest => dest.DifficultyLevel, opt => opt.MapFrom(src => src.DifficultyLevel.ToString()))
            .ForMember(dest => dest.ExamTitle, opt => opt.MapFrom(src => src.Exam != null ? src.Exam.Title : string.Empty));

        // من QuestionCreateDto → Question
        CreateMap<QuestionCreateDto, Question>()
            .ForMember(dest => dest.DifficultyLevel, opt => opt.MapFrom(src => ParseDifficulty(src.DifficultyLevel)))
            .ForMember(dest => dest.Exam, opt => opt.Ignore()); // ExamTitle ما يتحول مباشرة

        // من QuestionUpdateDto → Question
        CreateMap<QuestionUpdateDto, Question>()
            .ForMember(dest => dest.DifficultyLevel, opt => opt.MapFrom(src => ParseDifficulty(src.DifficultyLevel)))
            .ForMember(dest => dest.Exam, opt => opt.Ignore()); // ExamTitle ما يتحول مباشرة
    }
}
