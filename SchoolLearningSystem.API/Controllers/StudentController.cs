using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolLearningSystem.API.Responses;
using SchoolLearningSystem.Applicationf.DTOs.Student;
using SchoolLearningSystem.Applicationf.Interfaces;
using SchoolLearningSystem.Domain.Enums;

namespace SchoolLearningSystem.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class StudentController : ControllerBase
    {
        private readonly IStudentService _studentService;

        public StudentController(IStudentService studentService)
        {
            _studentService = studentService;
        }

        // 🔹 CRUD الأساسي

        /// <summary>كل الطلاب</summary>
        /// <remarks>الصلاحيات: Admin, Teacher فقط.</remarks>
        [HttpGet]
        [Authorize(Roles = "Admin,Teacher")]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<StudentReadDto>>), StatusCodes.Status200OK)]
        public async Task<ActionResult<ApiResponse<IEnumerable<StudentReadDto>>>> GetAll()
        {
            var students = await _studentService.GetAllAsync();
            return Ok(new ApiResponse<IEnumerable<StudentReadDto>>(200, "Students retrieved successfully", students));
        }

        /// <summary>بروفايل طالب واحد</summary>
        /// <remarks>الصلاحيات: Admin, Teacher, Student. الطالب يشوف بس بروفايله هو.</remarks>
        /// <response code="404">الطالب غير موجود</response>
        /// <response code="403">طالب يحاول يشوف بروفايل طالب ثاني</response>
        [HttpGet("{id}")]
        [Authorize(Roles = "Admin,Teacher,Student")]
        [ProducesResponseType(typeof(ApiResponse<StudentReadDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApiResponse<StudentReadDto>>> GetById(int id)
        {
            if (User.IsInRole("Student") && User.FindFirstValue("studentId") != id.ToString())
                return Forbid();

            var student = await _studentService.GetByIdAsync(id);
            if (student == null)
                return NotFound(new ApiResponse(404, "Student not found"));

            return Ok(new ApiResponse<StudentReadDto>(200, "Student retrieved successfully", student));
        }

        /// <summary>إنشاء طالب (مسار إداري قديم)</summary>
        /// <remarks>
        /// الصلاحيات: Admin فقط. إنشاء طالب فعلياً (بحساب دخول كامل) يصير عبر
        /// POST /api/auth/register/student - هذا الـ Endpoint مسار إداري قديم مقيّد لـ Admin
        /// حتى ما يُستخدم كثغرة تسجيل بدون حساب.
        ///
        /// مثال Request:
        /// {
        ///   "name": "طالب تجريبي",
        ///   "username": "student_x",
        ///   "email": "student_x@test.com",
        ///   "phone": "07700000000",
        ///   "gradeLevel": 4
        /// }
        /// </remarks>
        /// <response code="400">بيانات غير صالحة</response>
        /// <response code="401">لم يتم تسجيل الدخول</response>
        /// <response code="403">الدور الحالي غير مسموح له (غير Admin)</response>
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<ApiResponse>> Add([FromBody] StudentCreateDto dto)
        {
            await _studentService.CreateAsync(dto);
            return StatusCode(201, new ApiResponse(201, "Student created successfully"));
        }

        /// <summary>تعديل بروفايل طالب</summary>
        /// <remarks>
        /// الصلاحيات: Admin, Teacher, Student. الطالب يقدر يعدّل بس بروفايله هو. كل الحقول اختيارية.
        ///
        /// مثال Request:
        /// { "bio": "نبذة محدّثة", "phone": "07701111111" }
        /// </remarks>
        /// <response code="404">الطالب غير موجود</response>
        /// <response code="403">طالب يحاول يعدّل بروفايل طالب ثاني</response>
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin,Teacher,Student")]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApiResponse>> Update(int id, [FromBody] StudentUpdateDto dto)
        {
            if (User.IsInRole("Student") && User.FindFirstValue("studentId") != id.ToString())
                return Forbid();

            await _studentService.UpdateAsync(id, dto);
            return Ok(new ApiResponse(200, "Student updated successfully"));
        }

        /// <summary>حذف طالب (Soft Delete)</summary>
        /// <remarks>الصلاحيات: Admin, Teacher فقط.</remarks>
        /// <response code="404">الطالب غير موجود</response>
        /// <response code="401">لم يتم تسجيل الدخول</response>
        /// <response code="403">الدور الحالي غير مسموح له</response>
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin,Teacher")]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<ApiResponse>> Delete(int id)
        {
            await _studentService.DeleteAsync(id);
            return Ok(new ApiResponse(200, "Student deleted successfully"));
        }

        // 🔹 علاقات إضافية (Business Logic) — مطابقة حرفياً لـ IStudentService

        /// <summary>طلاب مرحلة دراسية معيّنة</summary>
        /// <remarks>الصلاحيات: Admin, Teacher فقط.</remarks>
        [HttpGet("grade/{gradeLevel}")]
        [Authorize(Roles = "Admin,Teacher")]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<StudentReadDto>>), StatusCodes.Status200OK)]
        public async Task<ActionResult<ApiResponse<IEnumerable<StudentReadDto>>>> GetByGradeLevel(GradeLevel gradeLevel)
        {
            var data = await _studentService.GetStudentsByGradeLevelAsync(gradeLevel);
            return Ok(new ApiResponse<IEnumerable<StudentReadDto>>(200, "Students retrieved successfully", data));
        }

        /// <summary>الطالب مع كل بيانات تقدمه (لمحرك SRS / لوحة الأداء الشخصية)</summary>
        /// <remarks>الصلاحيات: Admin, Teacher, Student. الطالب يشوف بس تقدمه هو.</remarks>
        /// <response code="403">طالب يحاول يشوف تقدم طالب ثاني</response>
        [HttpGet("{id}/progress")]
        [Authorize(Roles = "Admin,Teacher,Student")]
        [ProducesResponseType(typeof(ApiResponse<StudentReadDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<ApiResponse<StudentReadDto>>> GetWithProgress(int id)
        {
            if (User.IsInRole("Student") && User.FindFirstValue("studentId") != id.ToString())
                return Forbid();

            var data = await _studentService.GetStudentWithProgressAsync(id);
            return Ok(new ApiResponse<StudentReadDto>(200, "Student progress retrieved successfully", data));
        }

        // ⚠️ حُذفت من هنا نهائياً (كانت تنادي دوال غير موجودة إطلاقاً بـ IStudentService،
        // وهي أصلاً "مصدر حقيقة" بخدمات ثانية) — الفرونت يستخدم مباشرة:
        //   كورسات الطالب  → GET /api/coursestudent/student/{studentId}/courses/paged   (ICourseStudentService)
        //   نتائج الطالب   → GET /api/result/student/{studentId}                        (IResultService)
        //   جلسات الطالب   → GET /api/memorizesession/student/{studentId}/history        (IMemorizeService)
        //   متوسط الدرجات  → GET /api/result/student/{studentId}/average                  (IResultService)
        //   عدد الكورسات   → GET /api/coursestudent/student/{studentId}/courses/count     (ICourseStudentService)
    }
}
