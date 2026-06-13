using System.ComponentModel.DataAnnotations;

namespace SchoolLearningSystem.Applicationf.DTOs.CourseStudent
{
    public class CourseStudentCreateDto
    {
        [Required(ErrorMessage = "CourseId is required")]
        public int CourseId { get; set; }

        [Required(ErrorMessage = "StudentId is required")]
        public int StudentId { get; set; }
    }
}