namespace SchoolLearningSystem.Applicationf.DTOs.StudentAnswer
{
    // نادراً ما سنحتاج لتعديل إجابة طالب، ولكن هذا متاح إذا لزم الأمر
    public class StudentAnswerDetailUpdateDto
    {
        public string? SelectedAnswer { get; set; }
        public bool? IsCorrect { get; set; }
    }
}