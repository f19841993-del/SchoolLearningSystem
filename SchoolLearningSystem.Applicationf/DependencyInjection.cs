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
            // 2. تسجيل الـ Validators (هنا مكان السطر الذي سألت عنه)
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            // 2. تسجيل الخدمات (Services)
            services.AddScoped<ICourseService, CourseService>();
            services.AddScoped<ICourseStudentService, CourseStudentService>();
            services.AddScoped<ICurriculumService, CurriculumService>();
            services.AddScoped<IExamService, ExamService>();
            services.AddScoped<IExerciseService, ExerciseService>();
            services.AddScoped<ILessonService, LessonService>();
            services.AddScoped<IMemorizeService, MemorizeService>();
            services.AddScoped<IQuestionService, QuestionService>();
            services.AddScoped<IResultService, ResultService>();
            services.AddScoped<IStudentAnswerDetailService, StudentAnswerDetailService>();
            services.AddScoped<IStudentQuestionProgressService, StudentQuestionProgressService>();
            services.AddScoped<ISrsService, SrsService>();

            return services;
        }
    }
}