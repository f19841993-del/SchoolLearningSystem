using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolLearningSystem.API.Responses;
using SchoolLearningSystem.Applicationf.Common.Models;
using SchoolLearningSystem.Applicationf.Common.Parameters;
using SchoolLearningSystem.Applicationf.DTOs.Lesson;
using SchoolLearningSystem.Applicationf.Interfaces;

namespace SchoolLearningSystem.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class LessonController : ControllerBase
    {
        private readonly ILessonService _lessonService;

        public LessonController(ILessonService lessonService)
        {
            _lessonService = lessonService;
        }

        // 🔹 CRUD الأساسي (موروثة من IBaseService)

        /// <summary>كل الدروس</summary>
        /// <remarks>الصلاحيات: عام (بدون تسجيل دخول).</remarks>
        [Tags("Lesson - الأساسيات (CRUD)")]
        [HttpGet]
        [AllowAnonymous]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<LessonReadDto>>), StatusCodes.Status200OK)]
        public async Task<ActionResult<ApiResponse<IEnumerable<LessonReadDto>>>> GetAll()
        {
            var data = await _lessonService.GetAllAsync();
            return Ok(new ApiResponse<IEnumerable<LessonReadDto>>(200, "Lessons retrieved successfully", data));
        }

        /// <summary>درس واحد بالتفصيل</summary>
        /// <remarks>الصلاحيات: عام (بدون تسجيل دخول).</remarks>
        /// <response code="404">الدرس غير موجود</response>
        [Tags("Lesson - الأساسيات (CRUD)")]
        [HttpGet("{id}")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(ApiResponse<LessonReadDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApiResponse<LessonReadDto>>> GetById(int id)
        {
            var data = await _lessonService.GetByIdAsync(id);
            if (data == null)
                return NotFound(new ApiResponse(404, "Lesson not found"));

            return Ok(new ApiResponse<LessonReadDto>(200, "Lesson retrieved successfully", data));
        }

        /// <summary>الدروس مع ترقيم صفحات (Pagination)</summary>
        /// <remarks>الصلاحيات: عام (بدون تسجيل دخول).</remarks>
        [Tags("Lesson - الأساسيات (CRUD)")]
        [HttpGet("paged")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(ApiResponse<PagedList<LessonReadDto>>), StatusCodes.Status200OK)]
        public async Task<ActionResult<ApiResponse<PagedList<LessonReadDto>>>> GetPaged([FromQuery] QueryParameters parameters)
        {
            var paged = await _lessonService.GetPagedAsync(parameters);
            return Ok(new ApiResponse<PagedList<LessonReadDto>>(200, "Lessons retrieved successfully", paged));
        }

        /// <summary>إنشاء درس جديد</summary>
        /// <remarks>
        /// الصلاحيات: Admin, Teacher فقط.
        ///
        /// مثال Request:
        /// {
        ///   "title": "الجمع والطرح",
        ///   "content": "شرح الدرس بالتفصيل",
        ///   "videoUrl": "https://example.com/video.mp4",
        ///   "courseId": 1
        /// }
        /// </remarks>
        /// <response code="400">بيانات غير صالحة</response>
        /// <response code="401">لم يتم تسجيل الدخول</response>
        /// <response code="403">الدور الحالي غير مسموح له</response>
        [Tags("Lesson - الأساسيات (CRUD)")]
        [HttpPost]
        [Authorize(Roles = "Admin,Teacher")]
        [ProducesResponseType(typeof(ApiResponse<LessonReadDto>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<ApiResponse<LessonReadDto>>> Add([FromBody] LessonCreateDto dto)
        {
            var createdLesson = await _lessonService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = createdLesson.Id },
                new ApiResponse<LessonReadDto>(201, "Lesson created successfully", createdLesson));
        }

        /// <summary>تعديل درس موجود</summary>
        /// <remarks>
        /// الصلاحيات: Admin, Teacher فقط. كل الحقول اختيارية.
        ///
        /// مثال Request:
        /// { "title": "الجمع والطرح (محدّث)" }
        /// </remarks>
        /// <response code="404">الدرس غير موجود</response>
        /// <response code="401">لم يتم تسجيل الدخول</response>
        /// <response code="403">الدور الحالي غير مسموح له</response>
        [Tags("Lesson - الأساسيات (CRUD)")]
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin,Teacher")]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<ApiResponse>> Update(int id, [FromBody] LessonUpdateDto dto)
        {
            await _lessonService.UpdateAsync(id, dto);
            return Ok(new ApiResponse(200, "Lesson updated successfully"));
        }

        /// <summary>حذف درس (Soft Delete)</summary>
        /// <remarks>الصلاحيات: Admin, Teacher فقط. راجع Restore أدناه للتراجع.</remarks>
        /// <response code="404">الدرس غير موجود</response>
        /// <response code="401">لم يتم تسجيل الدخول</response>
        /// <response code="403">الدور الحالي غير مسموح له</response>
        [Tags("Lesson - الأساسيات (CRUD)")]
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin,Teacher")]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<ApiResponse>> Delete(int id)
        {
            // Soft Delete افتراضياً — راجع RestoreLessonAsync أدناه للتراجع عنه
            await _lessonService.DeleteAsync(id);
            return Ok(new ApiResponse(200, "Lesson deleted successfully"));
        }

        // 🔹 عمليات مخصصة (Business Logic) — مطابقة حرفياً لـ ILessonService

        /// <summary>التراجع عن حذف درس (Soft Delete) بالخطأ</summary>
        /// <remarks>الصلاحيات: Admin, Teacher فقط.</remarks>
        /// <response code="404">الدرس غير موجود</response>
        /// <response code="401">لم يتم تسجيل الدخول</response>
        /// <response code="403">الدور الحالي غير مسموح له</response>
        [Tags("Lesson - عمليات الحالة (Restore/Publish/Order)")]
        [HttpPatch("{id}/restore")]
        [Authorize(Roles = "Admin,Teacher")]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<ApiResponse>> Restore(int id)
        {
            await _lessonService.RestoreLessonAsync(id);
            return Ok(new ApiResponse(200, "Lesson restored successfully"));
        }

        /// <summary>نشر الدرس ليصبح مرئياً للطلاب</summary>
        /// <remarks>الصلاحيات: Admin, Teacher فقط.</remarks>
        /// <response code="404">الدرس غير موجود</response>
        /// <response code="401">لم يتم تسجيل الدخول</response>
        /// <response code="403">الدور الحالي غير مسموح له</response>
        [Tags("Lesson - عمليات الحالة (Restore/Publish/Order)")]
        [HttpPatch("{id}/publish")]
        [Authorize(Roles = "Admin,Teacher")]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<ApiResponse>> Publish(int id)
        {
            // ⚠️ ملاحظة: لا يوجد UnpublishLessonAsync بالـ Service حالياً — إذا محتاجينه
            // (api_contract.md يذكره)، لازم يُضاف أولاً بـ ILessonService قبل أي Endpoint هنا
            await _lessonService.PublishLessonAsync(id);
            return Ok(new ApiResponse(200, "Lesson published successfully"));
        }

        /// <summary>تغيير ترتيب الدرس داخل الكورس</summary>
        /// <remarks>الصلاحيات: Admin, Teacher فقط.</remarks>
        /// <response code="400">قيمة الترتيب الجديد غير صالحة</response>
        /// <response code="404">الدرس غير موجود</response>
        /// <response code="401">لم يتم تسجيل الدخول</response>
        /// <response code="403">الدور الحالي غير مسموح له</response>
        [Tags("Lesson - عمليات الحالة (Restore/Publish/Order)")]
        [HttpPatch("{id}/order")]
        [Authorize(Roles = "Admin,Teacher")]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<ApiResponse>> UpdateOrder(int id, [FromQuery] int newOrder)
        {
            await _lessonService.UpdateLessonOrderAsync(id, newOrder);
            return Ok(new ApiResponse(200, "Lesson order updated successfully"));
        }

        /// <summary>جلب الدرس المرتبط بتمرين معيّن</summary>
        /// <remarks>الصلاحيات: عام (بدون تسجيل دخول).</remarks>
        /// <response code="404">لا يوجد درس مرتبط بهذا التمرين</response>
        [Tags("Lesson - الاستعلام والعلاقات (Queries)")]
        [HttpGet("by-exercise/{exerciseId}")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(ApiResponse<LessonReadDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApiResponse<LessonReadDto>>> GetByExerciseId(int exerciseId)
        {
            var data = await _lessonService.GetLessonByExerciseIdAsync(exerciseId);
            if (data == null)
                return NotFound(new ApiResponse(404, "Lesson not found for this exercise"));

            return Ok(new ApiResponse<LessonReadDto>(200, "Lesson retrieved successfully", data));
        }


        /// <summary>جلب الدرس التالي بالتسلسل داخل نفس الكورس (لدعم مسار التعلم بالـ AI)</summary>
        /// <remarks>الصلاحيات: عام (بدون تسجيل دخول).</remarks>
        /// <response code="404">لا يوجد درس تالٍ (وصل آخر الكورس)</response>
        [Tags("Lesson - الاستعلام والعلاقات (Queries)")]
        [HttpGet("course/{courseId}/next")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(ApiResponse<LessonReadDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApiResponse<LessonReadDto>>> GetNext(
            int courseId, [FromQuery] int currentLessonOrder)
        {
            var data = await _lessonService.GetNextLessonAsync(courseId, currentLessonOrder);
            if (data == null)
                return NotFound(new ApiResponse(404, "No next lesson found"));

            return Ok(new ApiResponse<LessonReadDto>(200, "Next lesson retrieved successfully", data));
        }
    }
}
