using SchoolLearningSystem.Applicationf.DTOs.ExamDto;
using SchoolLearningSystem.Applicationf.DTOs.Lesson;

namespace SchoolLearningSystem.Applicationf.DTOs.CourseDto
{
    public class CourseReadDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty; // تمت الإضافة
        public string? Image { get; set; } // تمت الإضافة

        // بيانات المعلم (Flattened)
        public int TeacherId { get; set; }
        public string TeacherName { get; set; } = string.Empty;

        // بيانات المنهج (Flattened)
        public int CurriculumId { get; set; }
        public string CurriculumTitle { get; set; } = string.Empty;

        // العلاقات
        public List<int> StudentIds { get; set; } = new();
        public List<LessonDto> Lessons { get; set; } = new();
        public List<ExamReadDto> Exams { get; set; } = new();
    }
}