namespace SchoolLearningSystem.Applicationf.DTOs.ExerciseDto
{
    public class ExerciseReadDto
    {
        public int Id { get; set; }
        public string QuestionText { get; set; } = string.Empty;
        public string CorrectAnswer { get; set; } = string.Empty;

        public int LessonId { get; set; }
        public string LessonTitle { get; set; } = string.Empty; // لجعل العرض أسرع في الفرونت

        public int DifficultyLevel { get; set; }
    }
}