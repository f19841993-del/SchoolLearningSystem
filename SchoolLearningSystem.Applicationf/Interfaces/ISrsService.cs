using SchoolLearningSystem.Applicationf.DTOs.StudentQuestionProgress;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SchoolLearningSystem.Applicationf.Interfaces
{
    public interface ISrsService
    {
        /// <summary>
        /// يعالج إجابة الطالب ويحدث سجل تقدمه (يُستدعى بعد كل محاولة إجابة).
        /// </summary>
        Task ProcessAnswerAsync(int studentId, int questionId, bool isCorrect, int timeTakenInSeconds);

        /// <summary>
        /// يجلب الأسئلة التي يجب على الطالب مراجعتها الآن بناءً على خوارزمية التكرار.
        /// </summary>
        Task<IEnumerable<StudentQuestionProgressReadDto>> GetDueQuestionsForSessionAsync(int studentId);
    }
}