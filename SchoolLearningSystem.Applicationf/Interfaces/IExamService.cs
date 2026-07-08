using SchoolLearningSystem.Applicationf.DTOs.ExamDto;
using SchoolLearningSystem.Applicationf.DTOs.Lesson;
using SchoolLearningSystem.Applicationf.DTOs.Question;
using SchoolLearningSystem.Applicationf.DTOs.Result;
using SchoolLearningSystem.Applicationf.Interfaces.Base;

namespace SchoolLearningSystem.Applicationf.Interfaces
{
    public interface IExamService : IBaseService<ExamReadDto, ExamCreateDto, ExamUpdateDto>
    {
        // 🔹 العمليات الأساسية (GetAll, GetById, Create, Update, Delete, GetPaged)
        // موجودة مسبقاً بفضل الوراثة من IBaseService

        // ==========================================
        // 🔹 علاقات إضافية
        // ==========================================

        // جلب أسئلة امتحان معيّن
        Task<IEnumerable<QuestionReadDto>> GetQuestionsByExamIdAsync(int examId);

        // جلب نتائج امتحان معيّن (كل الطلاب اللي أدّوا الامتحان)
        Task<IEnumerable<ResultReadDto>> GetResultsByExamIdAsync(int examId);

        // جلب الدرس المرتبط بامتحان معيّن
        // 💡 Nullable لأن الامتحان قد يكون "عاماً" (شاملاً للكورس) وغير مرتبط بدرس محدد.
        // ترمي NotFoundException فقط إذا كان الـ examId نفسه غير موجود بقاعدة البيانات.
        Task<LessonReadDto?> GetLessonByExamIdAsync(int examId);

        // جلب كل امتحانات درس معيّن
        Task<IEnumerable<ExamReadDto>> GetExamsByLessonIdAsync(int lessonId);

        // ==========================================
        // 🔹 إحصائيات
        // ==========================================

        Task<int> GetTotalExamsByLessonIdAsync(int lessonId);
    }
}