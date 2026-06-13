using SchoolLearningSystem.Applicationf.DTOs.StudentAnswer; // تأكد من المسار الصحيح
using SchoolLearningSystem.Applicationf.Interfaces.Base;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SchoolLearningSystem.Applicationf.Interfaces
{
    public interface IStudentAnswerDetailService : IBaseService<StudentAnswerDetailReadDto, StudentAnswerDetailCreateDto, StudentAnswerDetailUpdateDto>
    {
        // 🔹 CRUD الأساسي: موروث من IBaseService

        // 🔹 علاقات إضافية (Logic)
        Task<IEnumerable<StudentAnswerDetailReadDto>> GetByStudentIdAsync(int studentId);
        Task<IEnumerable<StudentAnswerDetailReadDto>> GetByQuestionIdAsync(int questionId);
        Task<IEnumerable<StudentAnswerDetailReadDto>> GetRecentAnswersAsync(int studentId, int count);
        Task<IEnumerable<StudentAnswerDetailReadDto>> GetIncorrectAnswersByStudentIdAsync(int studentId, int lessonId);
    }
}