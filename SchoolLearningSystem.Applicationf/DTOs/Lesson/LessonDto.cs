using SchoolLearningSystem.Applicationf.DTOs.Exercise;
using SchoolLearningSystem.Applicationf.DTOs.MemorizeSession;
using SchoolLearningSystem.Applicationf.DTOs.Question;

namespace SchoolLearningSystem.Applicationf.DTOs.Lesson
{
    public class LessonDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;

        // المرحلة الدراسية (من Curriculum عبر Course)
        public string GradeLevel { get; set; } = string.Empty;

        // محتوى الدرس
        public string Content { get; set; } = string.Empty;

        // الكورس المرتبط
        public int CourseId { get; set; }
        public string CourseTitle { get; set; } = string.Empty;

        // الامتحانات المرتبطة بالدرس
        public List<ExamDto> Exams { get; set; } = new List<ExamDto>();

        // التدريبات المرتبطة بالدرس
        public List<ExerciseDto> Exercises { get; set; } = new List<ExerciseDto>();

        // النتائج المرتبطة بالدرس
        public List<ResultDto> Results { get; set; } = new List<ResultDto>();

        // جلسات المراجعة المرتبطة بالدرس
        public List<MemorizeSessionDto> MemorizeSessions { get; set; } = new List<MemorizeSessionDto>();

        // 🔹 الأسئلة المرتبطة بالدرس
        public List<QuestionDto> Questions { get; set; } = new List<QuestionDto>();
    }
}
