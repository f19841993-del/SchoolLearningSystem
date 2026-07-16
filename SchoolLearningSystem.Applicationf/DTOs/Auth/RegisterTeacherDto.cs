namespace SchoolLearningSystem.Applicationf.DTOs.Auth
{
    public class RegisterTeacherDto
    {
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Subject { get; set; } = string.Empty;
    }
}