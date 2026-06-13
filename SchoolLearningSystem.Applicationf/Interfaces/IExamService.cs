
using SchoolLearningSystem.Applicationf.DTOs.CurriculumDto;
using SchoolLearningSystem.Applicationf.DTOs.ExamDto;
using SchoolLearningSystem.Applicationf.DTOs.Lesson;
using SchoolLearningSystem.Applicationf.DTOs.Question;
using SchoolLearningSystem.Applicationf.DTOs.Result;
using SchoolLearningSystem.Applicationf.Interfaces.Base;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SchoolLearningSystem.Applicationf.Interfaces
{
    public interface IExamService : IBaseService<ExamReadDto, ExamCreateDto, ExamUpdateDto>
    {
        // الدوال الأساسية (GetAllAsync, GetByIdAsync, AddAsync, UpdateAsync, DeleteAsync) 
        // أصبحت موجودة تلقائياً بفضل الوراثة من IBaseService

        // 🔹 علاقات إضافية
        Task<IEnumerable<QuestionReadDto>> GetQuestionsByExamIdAsync(int examId);
        Task<IEnumerable<ResultReadDto>> GetResultsByExamIdAsync(int examId);
        Task<IEnumerable<LessonReadDto>> GetLessonsByExamIdAsync(int examId);
    }
}
