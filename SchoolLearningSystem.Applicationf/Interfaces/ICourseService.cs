using SchoolLearningSystem.Applicationf.DTOs.CourseDto;
using SchoolLearningSystem.Applicationf.DTOs.ExamDto;
using SchoolLearningSystem.Applicationf.DTOs.Lesson;
using SchoolLearningSystem.Applicationf.DTOs.Student;
using SchoolLearningSystem.Applicationf.Interfaces.Base;

namespace SchoolLearningSystem.Applicationf.Interfaces
{
    public interface ICourseService: IBaseService<CourseReadDto, CourseCreateDto, CourseUpdateDto>
    {
        // العمليات الأساسية IBaseService


        // علاقات إضافية
        Task<IEnumerable<StudentReadDto>> GetStudentsByCourseIdAsync(int courseId);
        Task<IEnumerable<LessonReadDto>> GetLessonsByCourseIdAsync(int courseId);
        Task<IEnumerable<ExamReadDto>> GetExamsByCourseIdAsync(int courseId);

        // ربط طالب بالكورس
        Task EnrollStudentAsync(int courseId, int studentId);
        Task RemoveStudentAsync(int courseId, int studentId);
    }
}
