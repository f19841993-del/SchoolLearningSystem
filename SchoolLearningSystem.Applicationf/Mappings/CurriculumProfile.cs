using AutoMapper;
using SchoolLearningSystem.Applicationf.DTOs.CurriculumDto;
using SchoolLearningSystem.Domain.Entities;

namespace SchoolLearningSystem.Applicationf.Mappings
{
    public class CurriculumProfile : Profile
    {
        public CurriculumProfile()
        {
            // 1. من الكيان → للعرض (Read)
            // Name/GradeLevel تتطابق أسماؤها تلقائياً - لا حاجة لـ ForMember
            CreateMap<Curriculum, CurriculumReadDto>()
                .ForMember(dest => dest.CoursesCount,
                    opt => opt.MapFrom(src => src.Courses != null ? src.Courses.Count : 0));

            // 2. من الإنشاء → للكيان (Create)
            // ⚠️ GradeLevel enum يُربط مباشرة بدون Enum.Parse - أبسط وأكثر أماناً
            // (يمنع أي احتمال خطأ تحويل نص غير صالح وقت التشغيل)
            CreateMap<CurriculumCreateDto, Curriculum>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.LastModifiedAt, opt => opt.Ignore())
                .ForMember(dest => dest.IsDeleted, opt => opt.Ignore())
                .ForMember(dest => dest.Courses, opt => opt.Ignore());

            // 3. من التعديل → للكيان (Update) - مع الحماية من Null Overwrite
            CreateMap<CurriculumUpdateDto, Curriculum>()
                .ForMember(dest => dest.Name, opt => {
                    opt.Condition(src => src.Name != null);
                    opt.MapFrom(src => src.Name);
                })
                .ForMember(dest => dest.GradeLevel, opt => {
                    opt.Condition(src => src.GradeLevel.HasValue);
                    opt.MapFrom(src => src.GradeLevel);
                })
                .ForMember(dest => dest.Description, opt => {
                    opt.Condition(src => src.Description != null);
                    opt.MapFrom(src => src.Description);
                })
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.LastModifiedAt, opt => opt.Ignore())
                .ForMember(dest => dest.IsDeleted, opt => opt.Ignore())
                .ForMember(dest => dest.Courses, opt => opt.Ignore());
        }
    }
}