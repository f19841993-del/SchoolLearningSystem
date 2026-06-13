namespace SchoolLearningSystem.Applicationf.DTOs.Srs
{
    public class AnswerSubmissionDto
    {
        public int StudentId { get; set; }
        public int QuestionId { get; set; }
        public bool IsCorrect { get; set; }
        public int TimeTakenInSeconds { get; set; }
    }
}