using SchoolLearningSystem.Applicationf.DTOs.MemorizeSession;

namespace SchoolLearningSystem.Applicationf.DTOs.Exercise
{
    public class ExerciseDto
    {
        public int Id { get; set; }

        // عنوان التدريب أو السؤال
        public string Title { get; set; } = string.Empty;

        // محتوى التدريب (مثلاً سؤال/جواب)
        public string Question { get; set; } = string.Empty;
        public string Answer { get; set; } = string.Empty;

        // الدرس المرتبط
        public int LessonId { get; set; }

        // جلسات المراجعة المرتبطة بهذا التدريب
        public List<MemorizeSessionDto> MemorizeSessions { get; set; } = new List<MemorizeSessionDto>();
    }
}
