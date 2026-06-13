
using AutoMapper;
using SchoolLearningSystem.Applicationf;
using SchoolLearningSystem.Applicationf.Services;
using SchoolLearningSystem.Domain.Interfaces;
using SchoolLearningSystem.Infrastructure.Data;
using SchoolLearningSystem.Infrastructure;
using Microsoft.EntityFrameworkCore;

using System;
using SchoolLearningSystem.Infrastructure.Repositories;


namespace SchoolLearningSystem.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();

            // ›Ì Program.cs
            builder.Services.AddApplicationServices(); // „‰ „‘—Ê⁄ Application
            builder.Services.AddInfrastructureServices(builder.Configuration); // „‰ „‘—Ê⁄ Infrastructure
          
            #region «÷«›… «·Œœ„«  »‘ﬂ· ÌœÊÌ (»œÊ‰ «” Œœ«„ «·‹ Extension Methods)
            //// Repositories
            //builder.Services.AddScoped<ITeacherRepository, TeacherRepository>();
            //builder.Services.AddScoped<ICourseRepository, CourseRepository>();
            //builder.Services.AddScoped<ILessonRepository, LessonRepository>();
            //builder.Services.AddScoped<IExamRepository, ExamRepository>();
            //builder.Services.AddScoped<IQuestionRepository, QuestionRepository>();
            //builder.Services.AddScoped<IResultRepository, ResultRepository>();
            //builder.Services.AddScoped<IMemorizeRepository, MemorizeRepository>();
            //builder.Services.AddScoped<IStudentRepository, StudentRepository>();
            //builder.Services.AddScoped<IExerciseRepository, ExerciseRepository>();
            //builder.Services.AddScoped<ICurriculumRepository, CurriculumRepository>();
            //builder.Services.AddScoped<ICourseStudentRepository, CourseStudentRepository>();

            //// Services
            //builder.Services.AddScoped<ITeacherService, TeacherService>();
            //builder.Services.AddScoped<ICourseService, CourseService>();
            //builder.Services.AddScoped<ILessonService, LessonService>();
            //builder.Services.AddScoped<IExamService, ExamService>();
            //builder.Services.AddScoped<IQuestionService, QuestionService>();
            //builder.Services.AddScoped<IResultService, ResultService>();
            //builder.Services.AddScoped<IMemorizeService, MemorizeService>();
            //builder.Services.AddScoped<IStudentService, StudentService>();
            //builder.Services.AddScoped<IExerciseService, ExerciseService>();
            //builder.Services.AddScoped<ICurriculumService, CurriculumService>();
            //builder.Services.AddScoped<ICourseStudentService, CourseStudentService>();
            //builder.Services.AddScoped<ISrsService, SrsService>();

            //// AutoMapper
            //builder.Services.AddAutoMapper(typeof(Program));

            #endregion


            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            
            builder.Services.AddSwaggerGen(c =>
            {
                var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
