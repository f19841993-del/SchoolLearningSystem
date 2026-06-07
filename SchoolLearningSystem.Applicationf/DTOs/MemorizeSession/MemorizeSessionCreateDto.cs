namespace SchoolLearningSystem.Applicationf.DTOs.MemorizeSession
{
    public class MemorizeSessionCreateDto
    {
        public int StudentId { get; set; }
        public int LessonId { get; set; }
        public int ExerciseId { get; set; }

        public int Attempts { get; set; }
        public double SuccessRate { get; set; }
        public DateTime Date { get; set; }
    }
}
