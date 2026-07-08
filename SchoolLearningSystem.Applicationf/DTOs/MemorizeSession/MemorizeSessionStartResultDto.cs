// Applicationf/DTOs/MemorizeSession/MemorizeSessionStartResultDto.cs
using SchoolLearningSystem.Applicationf.DTOs.MemorizeSession;
using SchoolLearningSystem.Applicationf.DTOs.StudentQuestionProgress;

namespace SchoolLearningSystem.Applicationf.DTOs.MemorizeSession
{
    // يمثل استجابة "بدء جلسة مراجعة جديدة"
    // يجمع بين الجلسة نفسها (Session Metadata) وقائمة الأسئلة المستحقة (Due Questions)
    // بنفس الاستجابة، عشان الفرونت يبدأ العرض فوراً بدون طلب ثاني
    public class MemorizeSessionStartResultDto
    {
        public MemorizeSessionReadDto Session { get; set; } = null!;
        public List<StudentQuestionProgressReadDto> DueQuestions { get; set; } = new();
    }
}