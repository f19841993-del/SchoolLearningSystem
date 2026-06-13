namespace SchoolLearningSystem.Applicationf.DTOs.CourseStudent
{
    public class CourseStudentReadDto
    {
        public int Id { get; set; } // معرف عملية التسجيل (Enrollment ID)

        public int CourseId { get; set; }
        public string CourseTitle { get; set; } = string.Empty; // لجعل العرض أسهل للفرونت

        public int StudentId { get; set; }
        public string StudentName { get; set; } = string.Empty; // اسم الطالب

        public DateTime EnrollmentDate { get; set; } // تاريخ التسجيل مهم جداً في خوارزميات الـ SRS
    }
}