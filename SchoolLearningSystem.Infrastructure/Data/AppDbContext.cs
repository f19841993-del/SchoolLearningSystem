using Microsoft.EntityFrameworkCore;
using SchoolLearningSystem.Domain.Entities;

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

        // الكيانات التحليلية للذكاء الاصطناعي
        public DbSet<StudentQuestionProgress> StudentQuestionProgresses { get; set; }
        public DbSet<StudentAnswerDetail> StudentAnswerDetails { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // 1. ضبط المفاتيح المركبة (Composite Keys)

            // علاقة الطالب بالكورس
            modelBuilder.Entity<CourseStudent>()
                .HasKey(cs => new { cs.CourseId, cs.StudentId });


            // علاقة التقدم في السؤال (محرك الـ SRS)
            modelBuilder.Entity<StudentQuestionProgress>()
                .HasKey(sqp => new { sqp.StudentId, sqp.QuestionId });

            // 2. ضبط العلاقات (Relationships) وحذف البيانات (Delete Behavior)
            // نمنع الحذف المتتالي (Cascade) لبعض العلاقات لتجنب فقدان البيانات التاريخية
            modelBuilder.Entity<CourseStudent>()
                .HasOne(cs => cs.Course)
                .WithMany(c => c.CourseStudents)
                .OnDelete(DeleteBehavior.Restrict);

            // يمكنك إضافة المزيد من الإعدادات هنا لاحقاً حسب الحاجة
        }
    }
}