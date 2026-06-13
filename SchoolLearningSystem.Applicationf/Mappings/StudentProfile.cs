using AutoMapper;
using SchoolLearningSystem.Applicationf.DTOs.Student;
using SchoolLearningSystem.Domain.Entities;
using System.Linq;

namespace SchoolLearningSystem.Applicationf.Mappings
{
    public class StudentProfile : Profile
    {
        public StudentProfile()
        {
            // 1. من الكيان → للعرض (Read)
            CreateMap<Student, StudentReadDto>()
                // الحقول البسيطة تُربط تلقائياً، نضيف العلاقات فقط
                .ForMember(dest => dest.GradeLevel, opt => opt.MapFrom(src => src.GradeLevel));

            // 2. من الإنشاء → للكيان (Create)
            CreateMap<StudentCreateDto, Student>()
                // الحماية الموحدة لحقول الـ BaseEntity
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                
                // تجاهل العلاقات
                .ForMember(dest => dest.CourseStudents, opt => opt.Ignore())
                .ForMember(dest => dest.Results, opt => opt.Ignore())
                .ForMember(dest => dest.MemorizeSessions, opt => opt.Ignore());

            // 3. من التعديل → للكيان (Update) - الحماية من Null Overwrite
            CreateMap<StudentUpdateDto, Student>()
                .ForMember(dest => dest.Name, opt => {
                    opt.Condition(src => src.Name != null);
                    opt.MapFrom(src => src.Name);
                })
                .ForMember(dest => dest.Bio, opt => {
                    opt.Condition(src => src.Bio != null);
                    opt.MapFrom(src => src.Bio);
                })
                .ForMember(dest => dest.Address, opt => {
                    opt.Condition(src => src.Address != null);
                    opt.MapFrom(src => src.Address);
                })
                .ForMember(dest => dest.ProfileImage, opt => {
                    opt.Condition(src => src.ProfileImage != null);
                    opt.MapFrom(src => src.ProfileImage);
                })
                // الحماية الموحدة لحقول الـ BaseEntity والعلاقات
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CourseStudents, opt => opt.Ignore())
                .ForMember(dest => dest.Results, opt => opt.Ignore())
                .ForMember(dest => dest.MemorizeSessions, opt => opt.Ignore());
        }
    }
}