using SchoolLearningSystem.Applicationf.DTOs.CourseDto;
using SchoolLearningSystem.Applicationf.DTOs.Lesson;
using SchoolLearningSystem.Applicationf.DTOs.Teacher;
using SchoolLearningSystem.Applicationf.Interfaces.Base;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SchoolLearningSystem.Applicationf.Interfaces
{
    public interface ITeacherService: IBaseService<TeacherReadDto, TeacherCreateDto, TeacherUpdateDto>
    {
        // العمليات الأساسية
       

        // علاقات إضافية
        Task<IEnumerable<CourseReadDto>> GetCoursesByTeacherIdAsync(int teacherId);
        Task<IEnumerable<LessonReadDto>> GetLessonsByTeacherIdAsync(int teacherId);

        // إحصائيات
        Task<int> GetTotalCoursesByTeacherIdAsync(int teacherId);
        Task<int> GetTotalLessonsByTeacherIdAsync(int teacherId);
    }
}
