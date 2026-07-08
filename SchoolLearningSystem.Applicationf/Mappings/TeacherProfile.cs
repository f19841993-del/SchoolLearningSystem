using AutoMapper;
using SchoolLearningSystem.Applicationf.DTOs.Teacher;
using SchoolLearningSystem.Domain.Entities;
using System.Linq;

namespace SchoolLearningSystem.Applicationf.Mappings
{
    public class TeacherProfile : Profile
    {
        public TeacherProfile()
        {
            // 1. من الكيان → للعرض (Read)
            CreateMap<Teacher, TeacherReadDto>()
                .ForMember(dest => dest.CourseTitles,
                    opt => opt.MapFrom(src => src.Courses.Select(c => c.Title).ToList()));

            // 2. من الإنشاء → للكيان (Create)
            // ⚠️ Subject غير موجود بـ TeacherCreateDto عمداً - يبقى بقيمته
            // الافتراضية "Math" المعرّفة بالـ Entity نفسه (راجع TeacherCreateDto)
            CreateMap<TeacherCreateDto, Teacher>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.LastModifiedAt, opt => opt.Ignore())
                .ForMember(dest => dest.IsDeleted, opt => opt.Ignore())
                .ForMember(dest => dest.Courses, opt => opt.Ignore());

            // 3. من التعديل → للكيان (Update)
            // ✅ تصحيح حرج: Subject حُذف من هذا الـ Mapping بالكامل. كان موجوداً
            // بالنسخة السابقة ويسمح بتغيير المادة من الـ Client، بينما القرار
            // الموثق ينص أنها "ثابتة على Math" ويجب ألا تُعدَّل عبر API عام.
            CreateMap<TeacherUpdateDto, Teacher>()
                .ForMember(dest => dest.Name, opt => {
                    opt.Condition(src => src.Name != null);
                    opt.MapFrom(src => src.Name);
                })
                .ForMember(dest => dest.Bio, opt => {
                    opt.Condition(src => src.Bio != null);
                    opt.MapFrom(src => src.Bio);
                })
                .ForMember(dest => dest.ProfileImage, opt => {
                    opt.Condition(src => src.ProfileImage != null);
                    opt.MapFrom(src => src.ProfileImage);
                })
                .ForMember(dest => dest.Subject, opt => opt.Ignore())
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.LastModifiedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.IsDeleted, opt => opt.Ignore())
                .ForMember(dest => dest.Courses, opt => opt.Ignore());
        }
    }
}