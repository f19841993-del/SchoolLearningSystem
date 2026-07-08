using AutoMapper;
using SchoolLearningSystem.Applicationf.DTOs.Student;
using SchoolLearningSystem.Domain.Entities;

namespace SchoolLearningSystem.Applicationf.Mappings
{
    public class StudentProfile : Profile
    {
        public StudentProfile()
        {
            // 1. من الكيان → للعرض (Read)
            // Name/Username/Email/Phone/Bio/Address/Education/ProfileImage/GradeLevel
            // تتطابق أسماؤها تلقائياً - لا حاجة لأي ForMember
            CreateMap<Student, StudentReadDto>();

            // 2. من الإنشاء → للكيان (Create)
            CreateMap<StudentCreateDto, Student>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.LastModifiedAt, opt => opt.Ignore())
                .ForMember(dest => dest.IsDeleted, opt => opt.Ignore())
                .ForMember(dest => dest.CourseStudents, opt => opt.Ignore())
                .ForMember(dest => dest.Results, opt => opt.Ignore())
                .ForMember(dest => dest.MemorizeSessions, opt => opt.Ignore())
                .ForMember(dest => dest.Progresses, opt => opt.Ignore());

            // 3. من التعديل → للكيان (Update) - الحماية من Null Overwrite
            // ✅ Phone/Education مُضافين هنا - كانا مفقودين تماماً بالنسخة
            // السابقة (يُسمح بمسحهما بالغلط بأي تحديث جزئي لا يرسلهما)
            CreateMap<StudentUpdateDto, Student>()
                .ForMember(dest => dest.Name, opt => {
                    opt.Condition(src => src.Name != null);
                    opt.MapFrom(src => src.Name);
                })
                .ForMember(dest => dest.Phone, opt => {
                    opt.Condition(src => src.Phone != null);
                    opt.MapFrom(src => src.Phone);
                })
                .ForMember(dest => dest.Bio, opt => {
                    opt.Condition(src => src.Bio != null);
                    opt.MapFrom(src => src.Bio);
                })
                .ForMember(dest => dest.Address, opt => {
                    opt.Condition(src => src.Address != null);
                    opt.MapFrom(src => src.Address);
                })
                .ForMember(dest => dest.Education, opt => {
                    opt.Condition(src => src.Education != null);
                    opt.MapFrom(src => src.Education);
                })
                .ForMember(dest => dest.ProfileImage, opt => {
                    opt.Condition(src => src.ProfileImage != null);
                    opt.MapFrom(src => src.ProfileImage);
                })
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.LastModifiedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.IsDeleted, opt => opt.Ignore())
                .ForMember(dest => dest.CourseStudents, opt => opt.Ignore())
                .ForMember(dest => dest.Results, opt => opt.Ignore())
                .ForMember(dest => dest.MemorizeSessions, opt => opt.Ignore())
                .ForMember(dest => dest.Progresses, opt => opt.Ignore());
        }
    }
}