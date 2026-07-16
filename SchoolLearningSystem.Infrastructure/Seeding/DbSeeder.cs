using Microsoft.EntityFrameworkCore;
using SchoolLearningSystem.Domain.Entities;
using SchoolLearningSystem.Domain.Enums;
using SchoolLearningSystem.Infrastructure.Data;

namespace SchoolLearningSystem.Infrastructure.Seeding
{
    public static class DbSeeder
    {
        public static async Task SeedAdminUserAsync(AppDbContext context)
        {
            if (await context.Users.AnyAsync(u => u.Role == UserRole.Admin))
            {
                Console.WriteLine("⏭️ حساب Admin موجود أصلاً — تم التخطي.");
                return;
            }

            var admin = new User
            {
                Username = "admin",
                Email = "admin@schoollearning.local",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin@12345"),
                Role = UserRole.Admin,
                IsActive = true
            };

            context.Users.Add(admin);
            await context.SaveChangesAsync();

            Console.WriteLine("✅ حساب Admin الافتراضي جاهز — Username: admin / Password: Admin@12345");
        }
        public static async Task SeedTestDataAsync(AppDbContext context)
        {
            // 🛡️ حماية: لو الطالب التجريبي موجود أصلاً، لا تكرر البيانات
            if (await context.Students.AnyAsync(s => s.Username == "test_student"))
            {
                Console.WriteLine("⏭️ Seed Data موجودة أصلاً — تم التخطي.");
                return;
            }



            // 1️⃣ المستوى 0: Teacher + Curriculum
            var teacher = new Teacher { Name = "أ. أحمد الرياضي", Subject = "Math" };
            var curriculum = new Curriculum
            {
                Name = "رياضيات الصف الرابع",
                Description = "منهج الرياضيات - الرابع الابتدائي",
                GradeLevel = GradeLevel.FourthPrimary
            };
            context.Teachers.Add(teacher);
            context.Curriculums.Add(curriculum);
            await context.SaveChangesAsync(); // نحتاج الـ Ids قبل الخطوة الجاية

            // 2️⃣ المستوى 1: Course
            var course = new Course
            {
                Title = "رياضيات - الفصل الأول",
                Description = "كورس تجريبي لاختبار محرك SRS",
                Image = "https://placeholder.com/course.jpg",
                Order = 1,
                TeacherId = teacher.Id,
                CurriculumId = curriculum.Id
            };
            context.Courses.Add(course);
            await context.SaveChangesAsync();

            // 3️⃣ المستوى 2: Lesson
            var lesson = new Lesson
            {
                Title = "الجمع والطرح",
                Content = "درس تجريبي لأغراض الاختبار",
                VideoUrl = "https://placeholder.com/video.mp4",
                Order = 1,
                IsPublished = true,
                CourseId = course.Id
            };
            context.Lessons.Add(lesson);
            await context.SaveChangesAsync();

            // 4️⃣ المستوى 0: Student
            var student = new Student
            {
                Name = "طالب تجريبي",
                Username = "test_student",
                Email = "test@student.com",
                Phone = "07700000000",
                Address = "النجف",
                Bio = "طالب لأغراض اختبار SRS",
                Education = "الرابع الابتدائي",
                ProfileImage = "",
                GradeLevel = GradeLevel.FourthPrimary
            };
            context.Students.Add(student);
            await context.SaveChangesAsync();

            // 5️⃣ المستوى 4: 3 أسئلة
            var questions = new List<Question>
            {
                new() { Text = "كم ناتج 2 + 2؟", Answer = "4", DifficultyLevel = DifficultyLevel.Easy, LessonId = lesson.Id },
                new() { Text = "كم ناتج 5 - 3؟", Answer = "2", DifficultyLevel = DifficultyLevel.Easy, LessonId = lesson.Id },
                new() { Text = "كم ناتج 12 - 7؟", Answer = "5", DifficultyLevel = DifficultyLevel.Medium, LessonId = lesson.Id },
            };
            context.Questions.AddRange(questions);
            await context.SaveChangesAsync();

            // 6️⃣ 🎯 الأهم: StudentQuestionProgress بمواعيد مختلفة (لاختبار الفلترة الحقيقية)
            context.StudentQuestionProgresses.AddRange(
                new StudentQuestionProgress
                {
                    StudentId = student.Id,
                    QuestionId = questions[0].Id,
                    NextReviewDate = DateTime.UtcNow.AddDays(-2),   // ✅ مستحقة (بالماضي)
                    EaseFactor = 2.5,
                    Interval = 1,
                    RepetitionLevel = 0,
                    LastReviewedAt = DateTime.UtcNow.AddDays(-3)
                },
                new StudentQuestionProgress
                {
                    StudentId = student.Id,
                    QuestionId = questions[1].Id,
                    NextReviewDate = DateTime.UtcNow.AddHours(-1),  // ✅ مستحقة (منذ ساعة)
                    EaseFactor = 2.5,
                    Interval = 1,
                    RepetitionLevel = 0,
                    LastReviewedAt = DateTime.UtcNow.AddDays(-1)
                },
                new StudentQuestionProgress
                {
                    StudentId = student.Id,
                    QuestionId = questions[2].Id,
                    NextReviewDate = DateTime.UtcNow.AddDays(5),    // ❌ غير مستحقة بعد (اختبار سلبي)
                    EaseFactor = 2.5,
                    Interval = 6,
                    RepetitionLevel = 1,
                    LastReviewedAt = DateTime.UtcNow.AddDays(-1)
                }
            );
            await context.SaveChangesAsync();

            Console.WriteLine($"✅ Seed تم بنجاح — StudentId = {student.Id}");
        }
    }
}