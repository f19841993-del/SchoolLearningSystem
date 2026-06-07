using SchoolLearningSystem.Applicationf.DTOs.CourseDto;

namespace SchoolLearningSystem.Applicationf.Interfaces
{
    public interface ICourseService
    {
        // العمليات الأساسية
        Task<IEnumerable<CourseReadDto>> GetAllCoursesAsync();
        Task<CourseReadDto?> GetCourseByIdAsync(int id);
        Task AddCourseAsync(CourseCreateDto dto);
        Task UpdateCourseAsync(int id, CourseUpdateDto dto);
        Task DeleteCourseAsync(int id);

        // علاقات إضافية
        Task<IEnumerable<StudentReadDto>> GetStudentsByCourseIdAsync(int courseId);
        Task<IEnumerable<LessonReadDto>> GetLessonsByCourseIdAsync(int courseId);
        Task<IEnumerable<ExamReadDto>> GetExamsByCourseIdAsync(int courseId);

        // ربط طالب بالكورس
        Task EnrollStudentAsync(int courseId, int studentId);
        Task RemoveStudentAsync(int courseId, int studentId);
    }
}
