namespace SchoolLearningSystem.Applicationf.DTOs.Student
{
    public class StudentUpdateDto
    {
        public string? Name { get; set; }
        public string? Phone { get; set; }        // ✅ إضافة - كان ناقصاً
        public string? Bio { get; set; }
        public string? Address { get; set; }
        public string? Education { get; set; }    // ✅ إضافة - كان ناقصاً
        public string? ProfileImage { get; set; }
    }
}