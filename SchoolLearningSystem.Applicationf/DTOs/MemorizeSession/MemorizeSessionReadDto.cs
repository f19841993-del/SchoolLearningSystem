namespace SchoolLearningSystem.Applicationf.DTOs.MemorizeSession
{
    public class MemorizeSessionReadDto
    {
        public int Id { get; set; }
        public string StudentName { get; set; } = string.Empty;
        public string LessonTitle { get; set; } = string.Empty;
        public int Attempts { get; set; }
        public double SuccessRate { get; set; }
        public DateTime Date { get; set; }
    }
}