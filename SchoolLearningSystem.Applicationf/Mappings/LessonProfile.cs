using AutoMapper;
using SchoolLearningSystem.Applicationf.DTOs.Lesson;
using SchoolLearningSystem.Domain.Entities;

namespace SchoolLearningSystem.Applicationf.Mappings
{
    public class LessonProfile : Profile
    {
        public LessonProfile()
        {
            // 1. من الكيان → للعرض (Read)
            // Title, Content, VideoUrl, Order, IsPublished, CourseId تتطابق أسماؤها تلقائياً
            CreateMap<Lesson, LessonReadDto>()
                .ForMember(dest => dest.CourseTitle,
                    opt => opt.MapFrom(src => src.Course != null ? src.Course.Title : string.Empty));

            // 2. من الإنشاء → للكيان (Create)
            CreateMap<LessonCreateDto, Lesson>()
                // Order/IsPublished مستثنيان عمداً - يُديرهما الـ Service (راجع LessonCreateDto)
                .ForMember(dest => dest.Order, opt => opt.Ignore())
                .ForMember(dest => dest.IsPublished, opt => opt.Ignore())
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.LastModifiedAt, opt => opt.Ignore())
                .ForMember(dest => dest.IsDeleted, opt => opt.Ignore())
                .ForMember(dest => dest.Course, opt => opt.Ignore())
                .ForMember(dest => dest.Exams, opt => opt.Ignore())
                .ForMember(dest => dest.Exercises, opt => opt.Ignore())
                .ForMember(dest => dest.Results, opt => opt.Ignore())
                .ForMember(dest => dest.Questions, opt => opt.Ignore());
            // ⚠️ لا يوجد MemorizeSessions هنا - هذه Navigation Property محذوفة
            // عمداً من Lesson Entity (راجع تعليق Lesson.cs)

            // 3. من التعديل → للكيان (Update)
            CreateMap<LessonUpdateDto, Lesson>()
                .ForMember(dest => dest.Title, opt => {
                    opt.Condition(src => src.Title != null);
                    opt.MapFrom(src => src.Title);
                })
                .ForMember(dest => dest.Content, opt => {
                    opt.Condition(src => src.Content != null);
                    opt.MapFrom(src => src.Content);
                })
                .ForMember(dest => dest.VideoUrl, opt => {
                    opt.Condition(src => src.VideoUrl != null);
                    opt.MapFrom(src => src.VideoUrl);
                })
                .ForMember(dest => dest.CourseId, opt => {
                    opt.Condition(src => src.CourseId.HasValue);
                    opt.MapFrom(src => src.CourseId);
                })
                .ForMember(dest => dest.Order, opt => opt.Ignore())
                .ForMember(dest => dest.IsPublished, opt => opt.Ignore())
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.LastModifiedAt, opt => opt.Ignore())
                .ForMember(dest => dest.IsDeleted, opt => opt.Ignore())
                .ForMember(dest => dest.Course, opt => opt.Ignore())
                .ForMember(dest => dest.Exams, opt => opt.Ignore())
                .ForMember(dest => dest.Exercises, opt => opt.Ignore())
                .ForMember(dest => dest.Results, opt => opt.Ignore())
                .ForMember(dest => dest.Questions, opt => opt.Ignore());
        }
    }
}