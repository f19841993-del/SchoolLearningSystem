using SchoolLearningSystem.Domain.Enums;

namespace SchoolLearningSystem.Domain.Entities
{
    // 💡 إضافة الوراثة من BaseEntity
    public class Student : BaseEntity
    {
        // تم حذف Id لاعتمادنا على BaseEntity
        public string Name { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string Bio { get; set; } = string.Empty;
        public string Education { get; set; } = string.Empty;
        public string ProfileImage { get; set; } = string.Empty;

        public GradeLevel GradeLevel { get; set; }

        // العلاقات المهيأة
        public ICollection<MemorizeSession> MemorizeSessions { get; set; } = new List<MemorizeSession>();
        public ICollection<CourseStudent> CourseStudents { get; set; } = new List<CourseStudent>();
        public ICollection<Result> Results { get; set; } = new List<Result>();

        // 💡 التعديل: تهيئة القائمة
        public ICollection<StudentQuestionProgress> Progresses { get; set; } = new List<StudentQuestionProgress>();
    }
}