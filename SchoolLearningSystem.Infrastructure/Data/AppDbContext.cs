using Microsoft.EntityFrameworkCore;
using SchoolLearningSystem.Domain.Entities;
using System.Reflection;

namespace SchoolLearningSystem.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Curriculum> Curriculums { get; set; }
        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<Student> Students { get; set; }

        public DbSet<Course> Courses { get; set; }
        public DbSet<Lesson> Lessons { get; set; }
        public DbSet<Exam> Exams { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Result> Results { get; set; }
        public DbSet<MemorizeSession> MemorizeSessions { get; set; }
        public DbSet<Exercise> Exercises { get; set; }
        public DbSet<CourseStudent> CourseStudents { get; set; }
      


        // الكيانات التحليلية للذكاء الاصطناعي
        public DbSet<StudentQuestionProgress> StudentQuestionProgresses { get; set; }
        public DbSet<StudentAnswerDetail> StudentAnswerDetails { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);






            // 2. السطر الذهبي: هذا السطر يبحث في المشروع الحالي عن أي كلاس يطبق IEntityTypeConfiguration وينفذه تلقائياً
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

           
    }
    }
}