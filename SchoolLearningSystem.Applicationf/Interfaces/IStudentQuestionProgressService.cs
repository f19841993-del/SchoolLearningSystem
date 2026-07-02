using SchoolLearningSystem.Applicationf.DTOs.StudentQuestionProgress;
using SchoolLearningSystem.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SchoolLearningSystem.Applicationf.Interfaces
{
    public interface IStudentQuestionProgressService
    {
        // 🔹 عمليات الاستعلام (Queries)
        // 🔹 عمليات الـ SRS (الأهم في هذا النظام)
        Task<StudentQuestionProgressReadDto?> GetByStudentAndQuestionAsync(int studentId, int questionId);
        Task<IEnumerable<StudentQuestionProgressReadDto>> GetDueQuestionsAsync(int studentId);
        //Task<IEnumerable<StudentQuestionProgressReadDto>> GetDueQuestionsAsync(int studentId, DateTime currentDate);

        // 🔹 عمليات CRUD خاصة
        Task<IEnumerable<StudentQuestionProgressReadDto>> GetByStudentIdAsync(int studentId);

            // 🔹 عمليات التعديل (Commands)
        Task AddProgressAsync(StudentQuestionProgressCreateDto dto);
        Task UpdateProgressAsync(StudentQuestionProgressUpdateDto dto);
        }
    }

