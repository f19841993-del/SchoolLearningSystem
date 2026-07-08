using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using SchoolLearningSystem.Applicationf.Interfaces;
using SchoolLearningSystem.Applicationf.Services;
using System.Reflection;

namespace SchoolLearningSystem.Applicationf
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            // 1. تسجيل الـ AutoMapper ليقوم بمسح الـ Profiles تلقائياً
            services.AddAutoMapper(Assembly.GetExecutingAssembly());

            // 2. تسجيل الـ Validators تلقائياً (يمسح كل AbstractValidator<T> بالمشروع)
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

            // 3. تسجيل الخدمات (Services) — الـ 13 كاملة حسب توثيق_مشروع_SchoolLearningSystem.md §7
            services.AddScoped<ICourseService, CourseService>();
            services.AddScoped<ICourseStudentService, CourseStudentService>();
            services.AddScoped<ICurriculumService, CurriculumService>();
            services.AddScoped<ITeacherService, TeacherService>();          // ✅ كانت ناقصة بالكامل
            services.AddScoped<ILessonService, LessonService>();
            services.AddScoped<IExamService, ExamService>();
            services.AddScoped<IQuestionService, QuestionService>();
            services.AddScoped<IExerciseService, ExerciseService>();
            services.AddScoped<IResultService, ResultService>();
            services.AddScoped<IStudentService, StudentService>();          // ✅ كانت ناقصة بالكامل
            services.AddScoped<IMemorizeService, MemorizeService>();
            services.AddScoped<IStudentAnswerDetailService, StudentAnswerDetailService>();

            // ✅ IStudentQuestionProgressService/StudentQuestionProgressService حُذفا نهائياً
            // ودُمج كل منطقهما داخل ISrsService/SrsService
            // (راجع توثيق_مشروع_SchoolLearningSystem.md §4). كانا مسجّلين هنا رغم حذفهما
            // فعلياً من الكود — هذا كان سيمنع المشروع من البناء (Compile Error).
            services.AddScoped<ISrsService, SrsService>();

            return services;
        }
    }
}
