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
        public DbSet<StudentQuestionProgress> StudentQuestionProgresses { get; set; }
        public DbSet<StudentAnswerDetail> StudentAnswerDetails { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // 🔹 السطر الذهبي: يقرأ كل كلاسات IEntityTypeConfiguration بمجلد
            // Infrastructure/Configurations تلقائياً، ويشمل ذلك:
            //   - تعريف المفاتيح المركّبة (CourseStudent, StudentQuestionProgress)
            //   - فلاتر الحذف المنطقي (HasQueryFilter) الخاصة بكل كيان على حِدة
            //
            // ⚠️ مهم جداً: لا تضف حلقة foreach تطبّق HasQueryFilter تلقائياً هنا.
            // كل كيان له فلتره الخاص مُعرَّف صريحاً بملف Configuration المخصص له
            // (بعضها أدق من فلتر عام، مثل Lesson الذي يستبعد أيضاً دروس الكورسات
            // المحذوفة: !l.IsDeleted && !l.Course.IsDeleted). فلتر عام تلقائي هنا
            // سيستبدل (لا يدمج) هذي الفلاتر الدقيقة ويفسدها.
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}