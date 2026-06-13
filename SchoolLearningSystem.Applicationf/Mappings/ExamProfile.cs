using AutoMapper;
using SchoolLearningSystem.Applicationf.DTOs.ExamDto;
using SchoolLearningSystem.Domain.Entities;


namespace SchoolLearningSystem.Applicationf.Mappings
{
    public class ExamProfile : Profile
    {
        public ExamProfile()
        {
            // 1. من الكيان → للعرض
            CreateMap<Exam, ExamReadDto>()
                .ForMember(dest => dest.CourseTitle, opt => opt.MapFrom(src => src.Course != null ? src.Course.Title : string.Empty))
                // بقية الحقول سيتم ربطها تلقائياً إذا كانت الأسماء متطابقة،
                // لكن سنضيفها يدوياً لضمان الدقة والتوحيد
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title))
                .ForMember(dest => dest.CourseId, opt => opt.MapFrom(src => src.CourseId))
             ;

            // 2. من الإنشاء → للكيان
            CreateMap<ExamCreateDto, Exam>()
                .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title))
                .ForMember(dest => dest.CourseId, opt => opt.MapFrom(src => src.CourseId))
              
                // حقول النظام والعلاقات يتم تجاهلها
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.LastModifiedAt, opt => opt.Ignore())
                .ForMember(dest => dest.IsDeleted, opt => opt.Ignore())
                .ForMember(dest => dest.Course, opt => opt.Ignore())
                .ForMember(dest => dest.Lesson, opt => opt.Ignore())
                .ForMember(dest => dest.Questions, opt => opt.Ignore())
                .ForMember(dest => dest.Results, opt => opt.Ignore());

            // 3. من التعديل → للكيان (مع الحماية من Null Overwrite)
            CreateMap<ExamUpdateDto, Exam>()
                .ForMember(dest => dest.Title, opt => {
                    opt.Condition(src => src.Title != null);
                    opt.MapFrom(src => src.Title);
                })
           
            
                // الحماية الموحدة لحقول الـ BaseEntity والعلاقات
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.LastModifiedAt, opt => opt.Ignore())
                .ForMember(dest => dest.IsDeleted, opt => opt.Ignore())
                .ForMember(dest => dest.Course, opt => opt.Ignore())
                .ForMember(dest => dest.Lesson, opt => opt.Ignore())
                .ForMember(dest => dest.Questions, opt => opt.Ignore())
                .ForMember(dest => dest.Results, opt => opt.Ignore());
        }
    }
}
