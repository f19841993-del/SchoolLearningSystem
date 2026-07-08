using System.ComponentModel.DataAnnotations;

namespace SchoolLearningSystem.Applicationf.DTOs.Result
{
    public class ResultUpdateDto
    {
        public string? ResultType { get; set; }
        public double? Score { get; set; }
    }
}