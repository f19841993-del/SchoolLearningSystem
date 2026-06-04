using Microsoft.EntityFrameworkCore;
using SchoolLearningSystem.Domain.Entities;
using System.Collections.Generic;


namespace SchoolLearningSystem.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

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
        public DbSet<Curriculum> Curriculums { get; set; }

    }
}
