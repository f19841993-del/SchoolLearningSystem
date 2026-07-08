using AutoMapper;
using SchoolLearningSystem.Applicationf.DTOs.Course;
using SchoolLearningSystem.Domain.Entities;
using System.Linq;

namespace SchoolLearningSystem.Applicationf.Mappings
{
    public class CourseProfile : Profile
    {
        public CourseProfile()
        {
            // 1. من الكيان → للعرض (Read)
            CreateMap<Course, CourseReadDto>()
                .ForMember(dest => dest.TeacherName,
                    opt => opt.MapFrom(src => src.Teacher != null ? src.Teacher.Name : string.Empty))
                .ForMember(dest => dest.CurriculumName,
                    opt => opt.MapFrom(src => src.Curriculum != null ? src.Curriculum.Name : string.Empty))
                .ForMember(dest => dest.LessonsCount,
                    opt => opt.MapFrom(src => src.Lessons != null ? src.Lessons.Count : 0))
                .ForMember(dest => dest.EnrolledStudentsCount,
                    opt => opt.MapFrom(src => src.CourseStudents != null ? src.CourseStudents.Count(cs => cs.IsActive) : 0));

            // 2. من الإنشاء → للكيان (Create)
            CreateMap<CourseCreateDto, Course>()
                // Order مستثنى عمداً - يُحسب بالـ Service (راجع تعليق CourseCreateDto)
                .ForMember(dest => dest.Order, opt => opt.Ignore())
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.LastModifiedAt, opt => opt.Ignore())
                .ForMember(dest => dest.IsDeleted, opt => opt.Ignore())
                .ForMember(dest => dest.Teacher, opt => opt.Ignore())
                .ForMember(dest => dest.Curriculum, opt => opt.Ignore())
                .ForMember(dest => dest.CourseStudents, opt => opt.Ignore())
                .ForMember(dest => dest.Lessons, opt => opt.Ignore())
                .ForMember(dest => dest.Exams, opt => opt.Ignore());

            // 3. من التعديل → للكيان (Update) - مع الحماية من Null Overwrite
            CreateMap<CourseUpdateDto, Course>()
                .ForMember(dest => dest.Title, opt => {
                    opt.Condition(src => src.Title != null);
                    opt.MapFrom(src => src.Title);
                })
                .ForMember(dest => dest.Description, opt => {
                    opt.Condition(src => src.Description != null);
                    opt.MapFrom(src => src.Description);
                })
                .ForMember(dest => dest.Image, opt => {
                    opt.Condition(src => src.Image != null);
                    opt.MapFrom(src => src.Image);
                })
                .ForMember(dest => dest.TeacherId, opt => {
                    opt.Condition(src => src.TeacherId.HasValue);
                    opt.MapFrom(src => src.TeacherId);
                })
                .ForMember(dest => dest.CurriculumId, opt => {
                    opt.Condition(src => src.CurriculumId.HasValue);
                    opt.MapFrom(src => src.CurriculumId);
                })
                // Order مستثنى عمداً - نفس منطق Create
                .ForMember(dest => dest.Order, opt => opt.Ignore())
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.LastModifiedAt, opt => opt.Ignore())
                .ForMember(dest => dest.IsDeleted, opt => opt.Ignore())
                .ForMember(dest => dest.Teacher, opt => opt.Ignore())
                .ForMember(dest => dest.Curriculum, opt => opt.Ignore())
                .ForMember(dest => dest.CourseStudents, opt => opt.Ignore())
                .ForMember(dest => dest.Lessons, opt => opt.Ignore())
                .ForMember(dest => dest.Exams, opt => opt.Ignore());
        }
    }
}