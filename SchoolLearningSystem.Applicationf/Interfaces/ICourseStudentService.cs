using SchoolLearningSystem.Applicationf.Common.Models;
using SchoolLearningSystem.Applicationf.Common.Parameters;
using SchoolLearningSystem.Applicationf.DTOs.CourseDto;
using SchoolLearningSystem.Applicationf.DTOs.CourseStudent;
using SchoolLearningSystem.Applicationf.DTOs.Student;

namespace SchoolLearningSystem.Applicationf.Interfaces
{
    // ملاحظة: لا يرث من IBaseService لأن CourseStudent كيان وسيط
    // بمفتاح مركّب (courseId + studentId) وليس مفتاحاً بسيطاً (int id)
    public interface ICourseStudentService
    {
        // ==========================================
        // 🔹 العمليات الأساسية (CRUD)
        // ==========================================
        Task<IEnumerable<CourseStudentReadDto>> GetAllCourseStudentsAsync();

        // 💡 غير Nullable الآن: السلوك الفعلي يرمي NotFoundException دائماً إذا لم يوجد السجل
        // (نمط Fail Fast، متسق مع RemoveStudentAsync و UpdateCourseStudentAsync)
        Task<CourseStudentReadDto> GetCourseStudentByIdAsync(int courseId, int studentId);

        Task UpdateCourseStudentAsync(int courseId, int studentId, CourseStudentUpdateDto dto);

        // ==========================================
        // 🔹 علاقات إضافية (Query Logic)
        // ==========================================
        // 💡 هذه هي المصدر الوحيد لهذا المنطق في كامل المشروع
        // (تم حذفها من ICourseService لتفادي التكرار)
        Task<IEnumerable<StudentReadDto>> GetStudentsByCourseIdAsync(int courseId);
        Task<IEnumerable<CourseReadDto>> GetCoursesByStudentIdAsync(int studentId);
        // 💡 يُستدعى بعد أي حدث "تفاعل حقيقي" مع محتوى الكورس (تسجيل نتيجة درس حالياً)
        // يعيد حساب ProgressPercentage من جدول Result، ويحدّث LastAccessedAt = الآن
        Task UpdateProgressAsync(int studentId, int courseId);

        // ==========================================
        // 🔹 عمليات التسجيل والإزالة (Business Rules)
        // ==========================================
        // 💡 المصدر الوحيد للتسجيل/الإزالة - يُستدعى من أي Controller (Course أو Student)
        Task EnrollStudentAsync(int courseId, int studentId);
        Task RemoveStudentAsync(int courseId, int studentId);

        // ==========================================
        // 🔹 إحصائيات (Statistics)
        // ==========================================
        Task<int> GetTotalStudentsByCourseIdAsync(int courseId);
        Task<int> GetTotalCoursesByStudentIdAsync(int studentId);

        // ==========================================
        // 🔹 الترقيم والفلترة (Pagination)
        // ==========================================
        Task<PagedList<StudentReadDto>> GetPagedStudentsByCourseIdAsync(int courseId, QueryParameters parameters);
        Task<PagedList<CourseReadDto>> GetPagedCoursesByStudentIdAsync(int studentId, QueryParameters parameters);
    }
}