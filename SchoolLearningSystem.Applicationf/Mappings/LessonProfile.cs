using AutoMapper;
using SchoolLearningSystem.Applicationf.DTOs.Lesson;
using SchoolLearningSystem.Domain.Entities;
using System.Linq;

namespace SchoolLearningSystem.Applicationf.Mappings
{
    public class LessonProfile : Profile
    {
        public LessonProfile()
        {
            // 1. من الكيان → للعرض (Read)
            CreateMap<Lesson, LessonReadDto>()
                .ForMember(dest => dest.CourseTitle, opt => opt.MapFrom(src => src.Course != null ? src.Course.Title : string.Empty));
            // باقي الحقول (Id, Title, Content, CourseId) تطابق الأسماء لذا AutoMapper سيربطها تلقائياً

            // 2. من الإنشاء → للكيان (Create)
            CreateMap<LessonCreateDto, Lesson>()
                // الحماية الموحدة لحقول الـ BaseEntity
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.LastModifiedAt, opt => opt.Ignore())
                .ForMember(dest => dest.IsDeleted, opt => opt.Ignore())
                // تجاهل العلاقات
                .ForMember(dest => dest.Course, opt => opt.Ignore())
                .ForMember(dest => dest.Exams, opt => opt.Ignore())
                .ForMember(dest => dest.Exercises, opt => opt.Ignore())
                .ForMember(dest => dest.Results, opt => opt.Ignore())
                .ForMember(dest => dest.MemorizeSessions, opt => opt.Ignore())
                .ForMember(dest => dest.Questions, opt => opt.Ignore());

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
                .ForMember(dest => dest.CourseId, opt => {
                    opt.Condition(src => src.CourseId.HasValue);
                    opt.MapFrom(src => src.CourseId);
                })
                // الحماية الموحدة لحقول الـ BaseEntity والعلاقات
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.LastModifiedAt, opt => opt.Ignore())
                .ForMember(dest => dest.IsDeleted, opt => opt.Ignore())
                .ForMember(dest => dest.Course, opt => opt.Ignore())
                .ForMember(dest => dest.Exams, opt => opt.Ignore())
                .ForMember(dest => dest.Exercises, opt => opt.Ignore())
                .ForMember(dest => dest.Results, opt => opt.Ignore())
                .ForMember(dest => dest.MemorizeSessions, opt => opt.Ignore())
                .ForMember(dest => dest.Questions, opt => opt.Ignore());
        }
    }
}