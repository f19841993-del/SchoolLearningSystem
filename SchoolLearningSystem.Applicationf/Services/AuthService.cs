using SchoolLearningSystem.Applicationf.DTOs.Auth;
using SchoolLearningSystem.Applicationf.DTOs.Student;
using SchoolLearningSystem.Applicationf.DTOs.Teacher;
using SchoolLearningSystem.Applicationf.Exceptions;
using SchoolLearningSystem.Applicationf.Interfaces;
using SchoolLearningSystem.Domain.Entities;
using SchoolLearningSystem.Domain.Enums;
using SchoolLearningSystem.Domain.Interfaces;

namespace SchoolLearningSystem.Applicationf.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IStudentService _studentService;
        private readonly ITeacherService _teacherService;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;

        public AuthService(
            IUserRepository userRepository,
            IStudentService studentService,
            ITeacherService teacherService,
            IJwtTokenGenerator jwtTokenGenerator)
        {
            _userRepository = userRepository;
            _studentService = studentService;
            _teacherService = teacherService;
            _jwtTokenGenerator = jwtTokenGenerator;
        }

        public async Task<AuthResponseDto> RegisterStudentAsync(RegisterStudentDto dto)
        {
            await EnsureUsernameAndEmailAreFreeAsync(dto.Username, dto.Email);

            var studentReadDto = await _studentService.CreateAsync(new StudentCreateDto
            {
                Name = dto.Name,
                Username = dto.Username,
                Email = dto.Email,
                GradeLevel = dto.GradeLevel
            });

            var user = new User
            {
                Username = dto.Username,
                Email = dto.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                Role = UserRole.Student,
                IsActive = true,
                StudentId = studentReadDto.Id
            };

            await _userRepository.AddAsync(user);
            await _userRepository.SaveChangesAsync();

            return BuildAuthResponse(user);
        }

        public async Task<AuthResponseDto> RegisterTeacherAsync(RegisterTeacherDto dto)
        {
            await EnsureUsernameAndEmailAreFreeAsync(dto.Username, dto.Email);

            var teacherReadDto = await _teacherService.CreateAsync(new TeacherCreateDto
            {
                Name = dto.Name,
                
            });

            var user = new User
            {
                Username = dto.Username,
                Email = dto.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                Role = UserRole.Teacher,
                IsActive = true,
                TeacherId = teacherReadDto.Id
            };

            await _userRepository.AddAsync(user);
            await _userRepository.SaveChangesAsync();

            return BuildAuthResponse(user);
        }

        public async Task<AuthResponseDto> LoginAsync(LoginDto dto)
        {
            var user = await _userRepository.GetByUsernameOrEmailAsync(dto.UsernameOrEmail);

            if (user == null || !BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
                throw new BadRequestException("اسم المستخدم أو كلمة المرور غير صحيحة.");
            if (!user.IsActive)
                throw new BadRequestException("الحساب غير مفعّل.");

            return BuildAuthResponse(user);
        }

        private async Task EnsureUsernameAndEmailAreFreeAsync(string username, string email)
        {
            if (await _userRepository.ExistsByUsernameAsync(username))
                throw new BadRequestException("اسم المستخدم مستخدم مسبقاً.");
            if (await _userRepository.ExistsByEmailAsync(email))
                throw new BadRequestException("البريد الإلكتروني مستخدم مسبقاً.");
        }

        private AuthResponseDto BuildAuthResponse(User user)
        {
            var (token, expiresAt) = _jwtTokenGenerator.GenerateToken(user);

            return new AuthResponseDto
            {
                Token = token,
                ExpiresAt = expiresAt,
                UserId = user.Id,
                Username = user.Username,
                Role = user.Role,
                StudentId = user.StudentId,
                TeacherId = user.TeacherId
            };
        }
    }
}