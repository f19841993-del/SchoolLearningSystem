
using SchoolLearningSystem.Applicationf.DTOs.CourseDto;
using SchoolLearningSystem.Applicationf.DTOs.ExamDto;
using SchoolLearningSystem.Applicationf.DTOs.Lesson;
using SchoolLearningSystem.Applicationf.DTOs.Student;
using SchoolLearningSystem.Applicationf.Interfaces.Base;

namespace SchoolLearningSystem.Applicationf.Interfaces
{
    public interface ICourseService : IBaseService<CourseReadDto, CourseCreateDto, CourseUpdateDto>
    {
        Task<IEnumerable<LessonReadDto>> GetLessonsByCourseIdAsync(int courseId);
        Task<IEnumerable<ExamReadDto>> GetExamsByCourseIdAsync(int courseId);
        // ⬅️ حذفنا: GetStudentsByCourseIdAsync, EnrollStudentAsync, RemoveStudentAsync
        // لأنها أصبحت مسؤولية ICourseStudentService حصرياً
    }
}