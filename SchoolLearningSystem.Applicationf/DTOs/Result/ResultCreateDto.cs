using System.ComponentModel.DataAnnotations;

namespace SchoolLearningSystem.Applicationf.DTOs.Result
{
    public class ResultCreateDto
    {
        public int StudentId { get; set; }

        // Nullable بدون [Required] - النتيجة قد تكون خاصة بامتحان
        // بدون درس محدد (تحقق "أحدهما على الأقل" يتم بمنطق ResultService.CreateAsync)
        public int? LessonId { get; set; }

        public int? ExamId { get; set; }
        public string ResultType { get; set; } = "Homework";
        public double Score { get; set; }

        public int DurationInSeconds { get; set; }
    }
}