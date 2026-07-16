using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolLearningSystem.API.Responses;
using SchoolLearningSystem.Applicationf.DTOs.CourseDto;
using SchoolLearningSystem.Applicationf.DTOs.CurriculumDto;
using SchoolLearningSystem.Applicationf.Interfaces;
using SchoolLearningSystem.Domain.Enums;

namespace SchoolLearningSystem.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class CurriculumController : ControllerBase
    {
        private readonly ICurriculumService _curriculumService;

        public CurriculumController(ICurriculumService curriculumService)
        {
            _curriculumService = curriculumService;
        }

        // 🔹 CRUD الأساسي (يستخدم دوال BaseService الموحدة)

        /// <summary>كل المناهج</summary>
        /// <remarks>الصلاحيات: عام (بدون تسجيل دخول).</remarks>
        [Tags("Curriculum - الأساسيات (CRUD)")]
        [HttpGet]
        [AllowAnonymous]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<CurriculumReadDto>>), StatusCodes.Status200OK)]
        public async Task<ActionResult<ApiResponse<IEnumerable<CurriculumReadDto>>>> GetAll()
        {
            var data = await _curriculumService.GetAllAsync();
            return Ok(new ApiResponse<IEnumerable<CurriculumReadDto>>(200, "Curriculums retrieved successfully", data));
        }

        /// <summary>منهج واحد بالتفصيل</summary>
        /// <remarks>الصلاحيات: عام (بدون تسجيل دخول).</remarks>
        /// <response code="404">المنهج غير موجود</response>
        [Tags("Curriculum - الأساسيات (CRUD)")]
        [HttpGet("{id}")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(ApiResponse<CurriculumReadDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApiResponse<CurriculumReadDto>>> GetById(int id)
        {
            var data = await _curriculumService.GetByIdAsync(id);
            if (data == null)
                return NotFound(new ApiResponse(404, "Curriculum not found"));

            return Ok(new ApiResponse<CurriculumReadDto>(200, "Curriculum retrieved successfully", data));
        }

        /// <summary>إنشاء منهج جديد</summary>
        /// <remarks>
        /// الصلاحيات: Admin, Teacher فقط.
        ///
        /// مثال Request:
        /// {
        ///   "gradeLevel": 4,
        ///   "name": "رياضيات الصف الرابع",
        ///   "description": "منهج الرياضيات - الرابع الابتدائي"
        /// }
        /// </remarks>
        /// <response code="400">بيانات غير صالحة</response>
        /// <response code="401">لم يتم تسجيل الدخول</response>
        /// <response code="403">الدور الحالي غير مسموح له</response>
        [Tags("Curriculum - الأساسيات (CRUD)")]
        [HttpPost]
        [Authorize(Roles = "Admin,Teacher")]
        [ProducesResponseType(typeof(ApiResponse<CurriculumReadDto>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<ApiResponse<CurriculumReadDto>>> Add([FromBody] CurriculumCreateDto dto)
        {
            // ⚠️ ملاحظة: عدّل الـ Service ليرجع الكيان المُنشأ (CurriculumReadDto) بدل void
            // حتى نقدر نستخدم CreatedAtAction بشكل صحيح مع رابط الموقع (Location header)
            var created = await _curriculumService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = created.Id },
                new ApiResponse<CurriculumReadDto>(201, "Curriculum created successfully", created));
        }

        /// <summary>تعديل منهج موجود</summary>
        /// <remarks>
        /// الصلاحيات: Admin, Teacher فقط. كل الحقول اختيارية.
        ///
        /// مثال Request:
        /// { "name": "رياضيات الصف الرابع (محدّث)" }
        /// </remarks>
        /// <response code="404">المنهج غير موجود</response>
        /// <response code="401">لم يتم تسجيل الدخول</response>
        /// <response code="403">الدور الحالي غير مسموح له</response>
        [Tags("Curriculum - الأساسيات (CRUD)")]
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin,Teacher")]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<ApiResponse>> Update(int id, [FromBody] CurriculumUpdateDto dto)
        {
            // 🗑️ حُذف try-catch: NotFoundException تُعالَج مركزياً بالـ ExceptionMiddleware
            await _curriculumService.UpdateAsync(id, dto);
            return Ok(new ApiResponse(200, "Curriculum updated successfully"));
        }

        /// <summary>حذف منهج (Soft Delete)</summary>
        /// <remarks>الصلاحيات: Admin, Teacher فقط.</remarks>
        /// <response code="404">المنهج غير موجود</response>
        /// <response code="401">لم يتم تسجيل الدخول</response>
        /// <response code="403">الدور الحالي غير مسموح له</response>
        [Tags("Curriculum - الأساسيات (CRUD)")]
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin,Teacher")]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<ApiResponse>> Delete(int id)
        {
            await _curriculumService.DeleteAsync(id);
            return Ok(new ApiResponse(200, "Curriculum deleted successfully"));
        }

        // 🔹 علاقات إضافية (Business Logic)

        /// <summary>كورسات منهج معيّن</summary>
        /// <remarks>الصلاحيات: عام (بدون تسجيل دخول).</remarks>
        [Tags("Curriculum - الاستعلام والعلاقات (Queries)")]
        [HttpGet("{id}/courses")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<CourseReadDto>>), StatusCodes.Status200OK)]
        public async Task<ActionResult<ApiResponse<IEnumerable<CourseReadDto>>>> GetCoursesByCurriculumId(int id)
        {
            var courses = await _curriculumService.GetCoursesByCurriculumIdAsync(id);
            return Ok(new ApiResponse<IEnumerable<CourseReadDto>>(200, "Courses retrieved successfully", courses));
        }

        /// <summary>منهج مرحلة دراسية معيّنة</summary>
        /// <remarks>الصلاحيات: عام (بدون تسجيل دخول).</remarks>
        /// <response code="404">لا يوجد منهج لهذه المرحلة</response>
        [Tags("Curriculum - الاستعلام والعلاقات (Queries)")]
        [HttpGet("grade/{gradeLevel}")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(ApiResponse<CurriculumReadDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApiResponse<CurriculumReadDto>>> GetByGradeLevel(GradeLevel gradeLevel)
        {
            var curriculum = await _curriculumService.GetCurriculumByGradeLevelAsync(gradeLevel);
            if (curriculum == null)
                return NotFound(new ApiResponse(404, "Curriculum not found"));

            return Ok(new ApiResponse<CurriculumReadDto>(200, "Curriculum retrieved successfully", curriculum));
        }

        /// <summary>عدد كورسات منهج معيّن</summary>
        /// <remarks>الصلاحيات: عام (بدون تسجيل دخول).</remarks>
        [Tags("Curriculum - الإحصائيات (Statistics)")]
        [HttpGet("{id}/total-courses")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(ApiResponse<int>), StatusCodes.Status200OK)]
        public async Task<ActionResult<ApiResponse<int>>> GetTotalCoursesByCurriculumId(int id)
        {
            var count = await _curriculumService.GetTotalCoursesByCurriculumIdAsync(id);
            return Ok(new ApiResponse<int>(200, "Total courses retrieved successfully", count));
        }
    }
}
