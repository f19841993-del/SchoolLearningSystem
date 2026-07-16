using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SchoolLearningSystem.Domain.Interfaces;
using SchoolLearningSystem.Domain.Interfaces.Base;
using SchoolLearningSystem.Infrastructure.Data;
using SchoolLearningSystem.Infrastructure.Repositories;
using SchoolLearningSystem.Infrastructure.Repositories.Base;
using System.Reflection;


namespace SchoolLearningSystem.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            // 1. تسجيل الـ DbContext
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            // 2. تسجيل الـ Generic Repository (Open Generic)
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

            // 3. تسجيل الـ Specific Repositories
            services.AddScoped<ICourseRepository, CourseRepository>();
            services.AddScoped<ICourseStudentRepository, CourseStudentRepository>();
            services.AddScoped<ICurriculumRepository, CurriculumRepository>();
            services.AddScoped<IExamRepository, ExamRepository>();
            services.AddScoped<IExerciseRepository, ExerciseRepository>();
            services.AddScoped<ILessonRepository, LessonRepository>();
            services.AddScoped<IMemorizeRepository, MemorizeRepository>();
            services.AddScoped<IQuestionRepository, QuestionRepository>();
            services.AddScoped<IResultRepository, ResultRepository>();
            services.AddScoped<IStudentAnswerDetailRepository, StudentAnswerDetailRepository>();
            services.AddScoped<IStudentQuestionProgressRepository, StudentQuestionProgressRepository>();
            services.AddScoped<IStudentRepository, StudentRepository>();
            services.AddScoped<ITeacherRepository, TeacherRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            return services;
        }
    }
}