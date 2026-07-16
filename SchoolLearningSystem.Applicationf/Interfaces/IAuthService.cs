using SchoolLearningSystem.Applicationf.DTOs.Auth;

namespace SchoolLearningSystem.Applicationf.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResponseDto> RegisterStudentAsync(RegisterStudentDto dto);
        Task<AuthResponseDto> RegisterTeacherAsync(RegisterTeacherDto dto);
        Task<AuthResponseDto> LoginAsync(LoginDto dto);
    }
}