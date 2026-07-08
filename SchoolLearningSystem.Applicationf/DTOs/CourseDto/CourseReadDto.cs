namespace SchoolLearningSystem.Applicationf.DTOs.CorseDto
{
    public class CourseReadDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Image { get; set; } = string.Empty;
        public int Order { get; set; }

        public int TeacherId { get; set; }
        public string TeacherName { get; set; } = string.Empty;

        public int CurriculumId { get; set; }
        public string CurriculumName { get; set; } = string.Empty;

        // مفيد للفرونت لعرض عدد الدروس/الطلاب بدون استعلام إضافي
        public int LessonsCount { get; set; }
        public int EnrolledStudentsCount { get; set; }
    }
}