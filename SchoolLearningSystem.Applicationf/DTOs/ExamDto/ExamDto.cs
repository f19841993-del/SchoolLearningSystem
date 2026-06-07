using SchoolLearningSystem.Applicationf.DTOs.Question;

namespace SchoolLearningSystem.Applicationf. DTOs.ExamDto
{
    public class ExamDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;

        // نوع الامتحان (مثلاً: Final, Quiz)
        public string ExamType { get; set; } = string.Empty;

        // الكورس المرتبط
        public int CourseId { get; set; }

        // الدرس المرتبط
        public int LessonId { get; set; }

        // الأسئلة المرتبطة بالامتحان
        public List<QuestionDto> Questions { get; set; } = new List<QuestionDto>();

        // النتائج المرتبطة بالامتحان
        public List<ResultDto> Results { get; set; } = new List<ResultDto>();
    }
}
