namespace SchoolLearningSystem.Applicationf.DTOs.Result
{
    public class ResultReadDto
    {
        public int Id { get; set; }
        public string StudentName { get; set; } = string.Empty;
        public string? LessonTitle { get; set; }
        public string? ExamTitle { get; set; }
        public string ResultType { get; set; } = string.Empty;
        public double Score { get; set; }
        public int DurationInSeconds { get; set; }
        public DateTime Date { get; set; }
    }
}