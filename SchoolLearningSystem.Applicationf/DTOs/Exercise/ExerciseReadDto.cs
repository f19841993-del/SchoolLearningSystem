namespace SchoolLearningSystem.Applicationf.DTOs.Exercise
{
    public class ExerciseReadDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;

        public string Question { get; set; } = string.Empty;
        public string Answer { get; set; } = string.Empty;

        public int LessonId { get; set; }

        public List<MemorizeSessionReadDto> MemorizeSessions { get; set; } = new List<MemorizeSessionReadDto>();
    }
}
