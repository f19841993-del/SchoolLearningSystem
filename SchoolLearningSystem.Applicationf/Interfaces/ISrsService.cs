using SchoolLearningSystem.Applicationf.DTOs.Srs; // استدعاء مسار DTO الإجابة الجديد
using SchoolLearningSystem.Applicationf.DTOs.StudentQuestionProgress;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SchoolLearningSystem.Applicationf.Interfaces
{
    /// <summary>
    /// واجهة الخدمة الذكية المسؤولة عن تطبيق خوارزمية التكرار المتباعد (SRS)
    /// </summary>
    public interface ISrsService
    {
        /// <summary>
        /// يعالج إجابة الطالب، يطبق خوارزمية SRS لحساب المستوى وموعد المراجعة القادم، ويحدث السجل.
        /// </summary>
        /// <param name="dto">كائن يحتوي على (رقم الطالب، رقم السؤال، النتيجة، والوقت المستغرق)</param>
        Task ProcessAnswerAsync(AnswerSubmissionDto dto);

        /// <summary>
        /// يجلب قائمة الأسئلة التي حان موعد مراجعتها للطالب في جلسته الحالية.
        /// </summary>
        /// <param name="studentId">رقم الطالب التعريفي</param>
        Task<IEnumerable<StudentQuestionProgressReadDto>> GetDueQuestionsForSessionAsync(int studentId, DateTime? currentDate);
    }
}