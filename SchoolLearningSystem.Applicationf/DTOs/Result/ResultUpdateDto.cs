using System.ComponentModel.DataAnnotations;

namespace SchoolLearningSystem.Applicationf.DTOs.Result
{
    public class ResultUpdateDto
    {
        public string? ResultType { get; set; }

        [Range(0.0, 100.0, ErrorMessage = "Score must be between 0 and 100")]
        public double? Score { get; set; }
    }
}