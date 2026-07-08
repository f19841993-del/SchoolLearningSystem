namespace SchoolLearningSystem.Applicationf.DTOs.MemorizeSession
{
    public class MemorizeSessionReadDto
    {
        public int Id { get; set; }
        public string StudentName { get; set; } = string.Empty;

        // اختياري لأن ExerciseId نفسه اختياري بالـ Entity
        public string? ExerciseTitle { get; set; }

        public int TotalAttempts { get; set; }
        public double SuccessRate { get; set; }
        public int DurationInSeconds { get; set; }
        public bool IsCompleted { get; set; }
        public DateTime? CompletedAt { get; set; }

        // من BaseEntity - تاريخ إنشاء الجلسة
        public DateTime CreatedAt { get; set; }
    }
}