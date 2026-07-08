using AutoMapper;
using SchoolLearningSystem.Applicationf.DTOs.CourseStudent;
using SchoolLearningSystem.Domain.Entities;

namespace SchoolLearningSystem.Applicationf.Mappings
{
    public class CourseStudentProfile : Profile
    {
        public CourseStudentProfile()
        {
            // 1. من الكيان → للعرض (Read)
            CreateMap<CourseStudent, CourseStudentReadDto>()
                .ForMember(dest => dest.CourseTitle,
                    opt => opt.MapFrom(src => src.Course != null ? src.Course.Title : string.Empty))
                .ForMember(dest => dest.StudentName,
                    opt => opt.MapFrom(src => src.Student != null ? src.Student.Name : string.Empty));
            // EnrolledAt, IsActive, ProgressPercentage, LastAccessedAt تتطابق أسماؤها تلقائياً

            // 2. من الإنشاء → للكيان (Create) - عملية تسجيل (Enrollment)
            CreateMap<CourseStudentCreateDto, CourseStudent>()
                .ForMember(dest => dest.EnrolledAt, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => true))
                .ForMember(dest => dest.ProgressPercentage, opt => opt.MapFrom(src => 0.0))
                .ForMember(dest => dest.LastAccessedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.Course, opt => opt.Ignore())
                .ForMember(dest => dest.Student, opt => opt.Ignore());

            // 3. من التعديل → للكيان (Update)
            // ⚠️ EnrolledAt غير موجود هنا عمداً - حدث تاريخي غير قابل للتعديل.
            // ProgressPercentage/LastAccessedAt غير موجودين أيضاً - يُحسبان من نشاط
            // الطالب الفعلي بالـ Service، لا يُرسلان جاهزين من الـ Client (راجع
            // تحذير CourseStudentUpdateDto). الحقل الوحيد المسموح: IsActive.
            CreateMap<CourseStudentUpdateDto, CourseStudent>()
                .ForMember(dest => dest.IsActive, opt => {
                    opt.Condition(src => src.IsActive.HasValue);
                    opt.MapFrom(src => src.IsActive);
                })
                .ForMember(dest => dest.EnrolledAt, opt => opt.Ignore())
                .ForMember(dest => dest.ProgressPercentage, opt => opt.Ignore())
                .ForMember(dest => dest.LastAccessedAt, opt => opt.Ignore())
                .ForMember(dest => dest.Course, opt => opt.Ignore())
                .ForMember(dest => dest.Student, opt => opt.Ignore());
        }
    }
}