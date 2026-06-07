namespace SchoolLearningSystem.Applicationf.DTOs.Exercise
{
    public class ExerciseCreateDto
    {
        public string Title { get; set; } = string.Empty;

        public string Question { get; set; } = string.Empty;
        public string Answer { get; set; } = string.Empty;

        public int LessonId { get; set; }
    }
}
