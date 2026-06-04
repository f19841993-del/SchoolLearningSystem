namespace SchoolLearningSystem.Applicationf.DTOs
{
    public class CurriculumDto
    {
        public int Id { get; set; }

        // عنوان المنهج أو المرحلة الدراسية
        public string Title { get; set; } = string.Empty;

        // المستوى الدراسي (مثلاً: أول متوسط، ثاني ثانوي...)
        public string GradeLevel { get; set; } = string.Empty;

        // الكورسات المرتبطة بهذا المنهج
        public List<CourseDto> Courses { get; set; } = new List<CourseDto>();
    }
}
