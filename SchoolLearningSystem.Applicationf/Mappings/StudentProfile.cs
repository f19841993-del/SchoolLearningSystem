using AutoMapper;
using SchoolLearningSystem.Applicationf.DTOs.Student;
using SchoolLearningSystem.Domain.Entities;
using SchoolLearningSystem.Domain.Enums;

public class StudentProfile : Profile
{
    private GradeLevel ParseGradeLevel(string gradeLevelString)
    {
        return Enum.TryParse<GradeLevel>(gradeLevelString, true, out var gradeLevel)
            ? gradeLevel
            : GradeLevel.Third; // قيمة افتراضية إذا فشل التحويل
    }

    public StudentProfile()
    {
        // من Student → StudentReadDto (للعرض)
        CreateMap<Student, StudentReadDto>()
            .ForMember(dest => dest.GradeLevel, opt => opt.MapFrom(src => src.GradeLevel.ToString()))
            .ForMember(dest => dest.CourseIds, opt => opt.MapFrom(src => src.CourseStudents.Select(cs => cs.CourseId)))
            .ForMember(dest => dest.Results, opt => opt.MapFrom(src => src.Results))
            .ForMember(dest => dest.MemorizeSessions, opt => opt.MapFrom(src => src.MemorizeSessions));

        // من StudentCreateDto → Student (للإضافة)
        CreateMap<StudentCreateDto, Student>()
            .ForMember(dest => dest.GradeLevel, opt => opt.MapFrom(src => ParseGradeLevel(src.GradeLevel)))
            .ForMember(dest => dest.CourseStudents, opt => opt.Ignore())   // CourseIds تتحول لاحقاً بالـ DbContext
            .ForMember(dest => dest.Results, opt => opt.Ignore())          // تنربط لاحقاً
            .ForMember(dest => dest.MemorizeSessions, opt => opt.Ignore());// نفس الشي

        // من StudentUpdateDto → Student (للتحديث)
        CreateMap<StudentUpdateDto, Student>()
            .ForMember(dest => dest.GradeLevel, opt => opt.MapFrom(src => ParseGradeLevel(src.GradeLevel)))
            .ForMember(dest => dest.CourseStudents, opt => opt.Ignore())   // CourseIds تتحول لاحقاً بالـ DbContext
            .ForMember(dest => dest.Results, opt => opt.Ignore())          // تنربط لاحقاً
            .ForMember(dest => dest.MemorizeSessions, opt => opt.Ignore());// نفس الشي
    }
}
