using Microsoft.AspNetCore.Mvc;
using SchoolLearningSystem.API.Responses;
using SchoolLearningSystem.Applicationf.DTOs.Auth;
using SchoolLearningSystem.Applicationf.Interfaces;

namespace SchoolLearningSystem.API.Authentication
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        /// <summary>تسجيل حساب طالب جديد</summary>
        /// <remarks>
        /// الصلاحيات: عام (بدون تسجيل دخول) - تسجيل ذاتي مفتوح، الحساب يصير فعّال فوراً.
        /// يرجع توكن JWT جاهز للاستخدام مباشرة بعد التسجيل (بدون حاجة لتسجيل دخول منفصل).
        ///
        /// مثال Request:
        /// {
        ///   "username": "stud1",
        ///   "email": "stud1@test.com",
        ///   "password": "Pass123!",
        ///   "name": "طالب تجريبي",
        ///   "gradeLevel": 4
        /// }
        /// </remarks>
        /// <response code="400">اسم المستخدم أو البريد الإلكتروني مستخدم مسبقاً، أو بيانات غير صالحة</response>
        [HttpPost("register/student")]
        [ProducesResponseType(typeof(ApiResponse<AuthResponseDto>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ApiResponse<AuthResponseDto>>> RegisterStudent([FromBody] RegisterStudentDto dto)
        {
            var result = await _authService.RegisterStudentAsync(dto);
            return StatusCode(201, new ApiResponse<AuthResponseDto>(201, "تم إنشاء حساب الطالب بنجاح", result));
        }

        /// <summary>تسجيل حساب معلم جديد</summary>
        /// <remarks>
        /// الصلاحيات: عام (بدون تسجيل دخول) - تسجيل ذاتي مفتوح، الحساب يصير فعّال فوراً.
        /// يرجع توكن JWT جاهز للاستخدام مباشرة بعد التسجيل.
        ///
        /// مثال Request:
        /// {
        ///   "username": "teach1",
        ///   "email": "teach1@test.com",
        ///   "password": "Pass123!",
        ///   "name": "أ. أحمد الرياضي"
        /// }
        /// </remarks>
        /// <response code="400">اسم المستخدم أو البريد الإلكتروني مستخدم مسبقاً، أو بيانات غير صالحة</response>
        [HttpPost("register/teacher")]
        [ProducesResponseType(typeof(ApiResponse<AuthResponseDto>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ApiResponse<AuthResponseDto>>> RegisterTeacher([FromBody] RegisterTeacherDto dto)
        {
            var result = await _authService.RegisterTeacherAsync(dto);
            return StatusCode(201, new ApiResponse<AuthResponseDto>(201, "تم إنشاء حساب المعلم بنجاح", result));
        }

        /// <summary>تسجيل الدخول (Student, Teacher, أو Admin)</summary>
        /// <remarks>
        /// الصلاحيات: عام (بدون تسجيل دخول). يقبل username أو email بنفس الحقل usernameOrEmail.
        /// حساب Admin لا يُسجَّل هنا إطلاقاً - جاهز مسبقاً (مزروع تلقائياً بأول تشغيل للمشروع)،
        /// تدخل بيه مباشرة بنفس الـ Endpoint هذا.
        ///
        /// الرد يحتوي role (1=Admin, 2=Teacher, 3=Student) وstudentId/teacherId (أحدهما null دائماً
        /// حسب الدور) - استخدمهم مباشرة بالفرونت بدل فك تشفير التوكن يدوياً.
        ///
        /// مثال Request:
        /// {
        ///   "usernameOrEmail": "stud1",
        ///   "password": "Pass123!"
        /// }
        /// </remarks>
        /// <response code="400">اسم المستخدم/البريد أو كلمة المرور غير صحيحة، أو الحساب غير مفعّل</response>
        [HttpPost("login")]
        [ProducesResponseType(typeof(ApiResponse<AuthResponseDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ApiResponse<AuthResponseDto>>> Login([FromBody] LoginDto dto)
        {
            var result = await _authService.LoginAsync(dto);
            return Ok(new ApiResponse<AuthResponseDto>(200, "تم تسجيل الدخول بنجاح", result));
        }
    }
}
