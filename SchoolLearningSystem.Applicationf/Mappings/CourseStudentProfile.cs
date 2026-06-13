using AutoMapper;
using SchoolLearningSystem.Domain.Entities;
using SchoolLearningSystem.Applicationf.DTOs.CourseStudent;


    public class CourseStudentProfile : Profile
    {
        public CourseStudentProfile()
        {
            // 1. من الكيان → للعرض
            CreateMap<CourseStudent, CourseStudentReadDto>()
                .ForMember(dest => dest.CourseTitle, opt => opt.MapFrom(src => src.Course != null ? src.Course.Title : string.Empty))
                .ForMember(dest => dest.StudentName, opt => opt.MapFrom(src => src.Student != null ? src.Student.Name : string.Empty))
                .ForMember(dest => dest.EnrollmentDate, opt => opt.MapFrom(src => src.EnrolledAt));

            // 2. من الإنشاء → للكيان
            CreateMap<CourseStudentCreateDto, CourseStudent>()
                .ForMember(dest => dest.EnrolledAt, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => true))
                .ForMember(dest => dest.Course, opt => opt.Ignore())
                .ForMember(dest => dest.Student, opt => opt.Ignore());

            // 3. من التعديل → للكيان (مع الحماية من Null Overwrite)
            CreateMap<CourseStudentUpdateDto, CourseStudent>()
                .ForMember(dest => dest.EnrolledAt, opt => {
                    opt.Condition(src => src.EnrollmentDate.HasValue); // التحديث فقط إذا كانت هناك قيمة
                    opt.MapFrom(src => src.EnrollmentDate);
                })
                .ForMember(dest => dest.IsActive, opt => {
                    opt.Condition(src => src.IsActive.HasValue); // التحديث فقط إذا كانت هناك قيمة
                    opt.MapFrom(src => src.IsActive);
                })
                .ForMember(dest => dest.Course, opt => opt.Ignore())
                .ForMember(dest => dest.Student, opt => opt.Ignore());
        }
    }

