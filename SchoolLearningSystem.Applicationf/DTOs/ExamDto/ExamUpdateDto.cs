using System.ComponentModel.DataAnnotations;

namespace SchoolLearningSystem.Applicationf.DTOs.ExamDto
{
    public class ExamUpdateDto
    {
        [StringLength(200)]
        public string? Title { get; set; }

        public int? DurationInMinutes { get; set; }

        public int? PassingScore { get; set; }

        public DateTime? StartDate { get; set; }

        public bool? IsActive { get; set; }
    }
}