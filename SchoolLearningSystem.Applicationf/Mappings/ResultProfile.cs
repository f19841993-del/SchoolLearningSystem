using AutoMapper;
using SchoolLearningSystem.Applicationf.DTOs.Result;
using SchoolLearningSystem.Domain.Entities;

namespace SchoolLearningSystem.Applicationf.Mappings
{
    public class ResultProfile : Profile
    {
        public ResultProfile()
        {
            // 1. من الكيان → للعرض (Read)
            // ⚠️ Date تتطابق أسماؤها تلقائياً مع Result.Date (خاصية مستقلة عن
            // BaseEntity.CreatedAt) - لا تستبدلها بـ CreatedAt، هذا كان خطأً
            // بالنسخة السابقة يُفقد القيمة الحقيقية لـ Date.
            CreateMap<Result, ResultReadDto>()
                .ForMember(dest => dest.StudentName,
                    opt => opt.MapFrom(src => src.Student != null ? src.Student.Name : string.Empty))
                .ForMember(dest => dest.LessonTitle,
                    opt => opt.MapFrom(src => src.Lesson != null ? src.Lesson.Title : null))
                .ForMember(dest => dest.ExamTitle,
                    opt => opt.MapFrom(src => src.Exam != null ? src.Exam.Title : null));
            // 💡 ملاحظة: منطق "عرض ExamType بدل ResultType لو مرتبط بامتحان"
            // (كان موجوداً بالنسخة السابقة) نُقل عمداً - هذا قرار عرض/منطق
            // أعمال يجب أن يعيش بالـ Service، ليس بطبقة الـ Mapping.

            // 2. من الإنشاء → للكيان (Create)
            CreateMap<ResultCreateDto, Result>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Date, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.LastModifiedAt, opt => opt.Ignore())
                .ForMember(dest => dest.IsDeleted, opt => opt.Ignore())
                .ForMember(dest => dest.Student, opt => opt.Ignore())
                .ForMember(dest => dest.Lesson, opt => opt.Ignore())
                .ForMember(dest => dest.Exam, opt => opt.Ignore());

            // 3. من التعديل → للكيان (Update)
            CreateMap<ResultUpdateDto, Result>()
                .ForMember(dest => dest.ResultType, opt => {
                    opt.Condition(src => src.ResultType != null);
                    opt.MapFrom(src => src.ResultType);
                })
                .ForMember(dest => dest.Score, opt => {
                    opt.Condition(src => src.Score.HasValue);
                    opt.MapFrom(src => src.Score);
                })
                .ForMember(dest => dest.LastModifiedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Date, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.IsDeleted, opt => opt.Ignore())
                .ForMember(dest => dest.Student, opt => opt.Ignore())
                .ForMember(dest => dest.Lesson, opt => opt.Ignore())
                .ForMember(dest => dest.Exam, opt => opt.Ignore());
        }
    }
}