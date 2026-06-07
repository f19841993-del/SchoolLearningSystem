using System;

namespace SchoolLearningSystem.Applicationf.DTOs.MemorizeSession
{
    public class MemorizeSessionDto
    {
        public int Id { get; set; }

        // الطالب المرتبط
        public int StudentId { get; set; }
        public string StudentName { get; set; } = string.Empty;

        // الدرس المرتبط
        public int LessonId { get; set; }
        public string LessonTitle { get; set; } = string.Empty;

        // التدريب المرتبط
        public int ExerciseId { get; set; }
        public string ExerciseTitle { get; set; } = string.Empty;

        // الأداء
        public int Attempts { get; set; }
        public double SuccessRate { get; set; }

        // تاريخ الجلسة
        public DateTime Date { get; set; }
    }

}
