using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolLearningSystem.API.Responses;
using SchoolLearningSystem.Applicationf.DTOs.MemorizeSession;
using SchoolLearningSystem.Applicationf.Interfaces;

namespace SchoolLearningSystem.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin,Teacher,Student")]
    public class MemorizeSessionController : ControllerBase
    {
        private readonly IMemorizeService _memorizeService;

        public MemorizeSessionController(IMemorizeService memorizeService)
        {
            _memorizeService = memorizeService;
        }

        // 🔹 CRUD الأساسي (موروث من IBaseService)

        /// <summary>كل جلسات المراجعة (كل الطلاب)</summary>
        /// <remarks>الصلاحيات: Admin, Teacher فقط.</remarks>
        [Tags("MemorizeSession - الأساسيات (CRUD)")]
        [HttpGet]
        [Authorize(Roles = "Admin,Teacher")]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<MemorizeSessionReadDto>>), StatusCodes.Status200OK)]
        public async Task<ActionResult<ApiResponse<IEnumerable<MemorizeSessionReadDto>>>> GetAll()
        {
            var data = await _memorizeService.GetAllAsync();
            return Ok(new ApiResponse<IEnumerable<MemorizeSessionReadDto>>(200, "Sessions retrieved successfully", data));
        }

        /// <summary>جلسة واحدة بالتفصيل</summary>
        /// <remarks>الصلاحيات: Admin, Teacher فقط.</remarks>
        /// <response code="404">الجلسة غير موجودة</response>
        [Tags("MemorizeSession - الأساسيات (CRUD)")]
        [HttpGet("{id}")]
        [Authorize(Roles = "Admin,Teacher")]
        [ProducesResponseType(typeof(ApiResponse<MemorizeSessionReadDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApiResponse<MemorizeSessionReadDto>>> GetById(int id)
        {
            var data = await _memorizeService.GetByIdAsync(id);
            if (data == null)
                return NotFound(new ApiResponse(404, "Session not found"));

            return Ok(new ApiResponse<MemorizeSessionReadDto>(200, "Session retrieved successfully", data));
        }

        //[HttpPost]
        //[ProducesResponseType(typeof(ApiResponse<MemorizeSessionReadDto>), StatusCodes.Status201Created)]
        //[ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        //public async Task<ActionResult<ApiResponse<MemorizeSessionReadDto>>> Add([FromBody] MemorizeSessionCreateDto dto)
        //{
        //    // ⚠️ إنشاء "خام" غير مرتبط بفحص الجلسة النشطة أو الأسئلة المستحقة (SRS).
        //        // الطريق الرسمي المفضّل للفرونت هو POST /api/students/{studentId}/memorize-sessions/start
        //        // (StartNewSessionAsync أسفل هذا الملف) — هذا الـ Endpoint هنا بس للإدارة/الاختبار المباشر..
        //                var created = await _memorizeService.CreateAsync(dto);
        //    return CreatedAtAction(nameof(GetById), new { id = created.Id },
        //        new ApiResponse<MemorizeSessionReadDto>(201, "Session created successfully", created));
        //}

        //[HttpPut("{id}")]
        //[ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
        //[ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        //public async Task<ActionResult<ApiResponse>> Update(int id, [FromBody] MemorizeSessionUpdateDto dto)
        //{
        //    await _memorizeService.UpdateAsync(id, dto);
        //    return Ok(new ApiResponse(200, "Session updated successfully"));
        //}

        /// <summary>حذف جلسة مراجعة</summary>
        /// <remarks>الصلاحيات: Admin, Teacher فقط.</remarks>
        /// <response code="404">الجلسة غير موجودة</response>
        [Tags("MemorizeSession - الأساسيات (CRUD)")]
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin,Teacher")]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApiResponse>> Delete(int id)
        {
            await _memorizeService.DeleteAsync(id);
            return Ok(new ApiResponse(200, "Session deleted successfully"));
        }

        // 🔹 عمليات مخصصة لجلسات المراجعة (Business Logic) — مطابقة حرفياً لـ IMemorizeService

        /// <summary>الجلسة النشطة الحالية للطالب (إن وُجدت) — نقطة البداية عند فتح شاشة المراجعة</summary>
        /// <remarks>الصلاحيات: Admin, Teacher, Student. الطالب يشوف بس جلسته هو.</remarks>
        /// <response code="404">ما فيه جلسة نشطة لهذا الطالب حالياً</response>
        /// <response code="403">طالب يحاول يشوف جلسة طالب ثاني</response>
        [Tags("MemorizeSession - جلسات الطالب (Student Sessions)")]
        [HttpGet("student/{studentId}/active")]
        [ProducesResponseType(typeof(ApiResponse<MemorizeSessionReadDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApiResponse<MemorizeSessionReadDto>>> GetActiveSession(int studentId)
        {
            if (User.IsInRole("Student") && User.FindFirstValue("studentId") != studentId.ToString())
                return Forbid();

            var data = await _memorizeService.GetActiveSessionByStudentIdAsync(studentId);
            if (data == null)
                return NotFound(new ApiResponse(404, "No active session found for this student"));

            return Ok(new ApiResponse<MemorizeSessionReadDto>(200, "Active session retrieved successfully", data));
        }

        /// <summary>سجل كل جلسات الطالب (تاريخ المراجعات) لعرضه بلوحة تحكمه</summary>
        /// <remarks>الصلاحيات: Admin, Teacher, Student. الطالب يشوف بس سجله هو.</remarks>
        /// <response code="403">طالب يحاول يشوف سجل طالب ثاني</response>
        [Tags("MemorizeSession - جلسات الطالب (Student Sessions)")]
        [HttpGet("student/{studentId}/history")]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<MemorizeSessionReadDto>>), StatusCodes.Status200OK)]
        public async Task<ActionResult<ApiResponse<IEnumerable<MemorizeSessionReadDto>>>> GetHistory(int studentId)
        {
            if (User.IsInRole("Student") && User.FindFirstValue("studentId") != studentId.ToString())
                return Forbid();

            var data = await _memorizeService.GetSessionHistoryByStudentIdAsync(studentId);
            return Ok(new ApiResponse<IEnumerable<MemorizeSessionReadDto>>(200, "Session history retrieved successfully", data));
        }

        /// <summary>جلسة معيّنة مع كل تفاصيل إجاباتها (مراجعة كاملة بعد الانتهاء)</summary>
        /// <remarks>الصلاحيات: Admin, Teacher, Student. الطالب يشوف بس جلسته هو.</remarks>
        /// <response code="403">طالب يحاول يشوف جلسة طالب ثاني</response>
        [Tags("MemorizeSession - جلسات الطالب (Student Sessions)")]
        [HttpGet("{id}/with-answers")]
        [ProducesResponseType(typeof(ApiResponse<MemorizeSessionReadDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<ApiResponse<MemorizeSessionReadDto>>> GetWithAnswers(int id)
        {
            var data = await _memorizeService.GetSessionWithAnswersAsync(id);

            if (User.IsInRole("Student") && User.FindFirstValue("studentId") != data.StudentId.ToString())
                return Forbid();

            return Ok(new ApiResponse<MemorizeSessionReadDto>(200, "Session with answers retrieved successfully", data));
        }

        /// <summary>إنهاء الجلسة الحالية: IsCompleted=true + تسجيل CompletedAt ونسبة النجاح</summary>
        /// <remarks>الصلاحيات: Admin, Teacher, Student. الطالب يقدر يكمل بس جلسته هو.</remarks>
        /// <response code="404">الجلسة غير موجودة</response>
        /// <response code="403">طالب يحاول يكمل جلسة طالب ثاني</response>
        [Tags("MemorizeSession - جلسات الطالب (Student Sessions)")]
        [HttpPatch("{id}/complete")]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApiResponse>> CompleteSession(int id)
        {
            var session = await _memorizeService.GetByIdAsync(id);
            if (session == null)
                return NotFound(new ApiResponse(404, "Session not found"));

            if (User.IsInRole("Student") && User.FindFirstValue("studentId") != session.StudentId.ToString())
                return Forbid();

            await _memorizeService.CompleteSessionAsync(id);
            return Ok(new ApiResponse(200, "Session completed successfully"));
        }

        // API/Controllers/MemorizeSessionsController.cs

        /// <summary>بدء جلسة مراجعة جديدة (أسئلة مستحقة حسب خوارزمية SM-2)</summary>
        /// <remarks>الصلاحيات: Admin, Teacher, Student. الطالب يقدر يبدأ بس جلسة له هو.</remarks>
        /// <response code="403">طالب يحاول يبدأ جلسة نيابة عن طالب ثاني</response>
        [Tags("MemorizeSession - جلسات الطالب (Student Sessions)")]
        [HttpPost("/api/students/{studentId}/memorize-sessions/start")]
        public async Task<ActionResult<ApiResponse<MemorizeSessionStartResultDto>>> StartSession(int studentId)
        {
            if (User.IsInRole("Student") && User.FindFirstValue("studentId") != studentId.ToString())
                return Forbid();

            var result = await _memorizeService.StartNewSessionAsync(studentId);
            return Ok(ApiResponse<MemorizeSessionStartResultDto>.Success(result));
        }

        /// <summary>بدء جلسة "تدريب مكثف" مركّزة على نقاط ضعف الطالب بدرس معيّن</summary>
        /// <remarks>الصلاحيات: Admin, Teacher, Student. الطالب يقدر يبدأ بس جلسة له هو.</remarks>
        /// <response code="403">طالب يحاول يبدأ جلسة نيابة عن طالب ثاني</response>
        [Tags("MemorizeSession - جلسات الطالب (Student Sessions)")]
        [HttpPost("/api/students/{studentId}/memorize-sessions/remedial/{lessonId}")]
        public async Task<ActionResult<ApiResponse<MemorizeSessionStartResultDto>>> StartRemedialSession(int studentId, int lessonId)
        {
            if (User.IsInRole("Student") && User.FindFirstValue("studentId") != studentId.ToString())
                return Forbid();

            var result = await _memorizeService.StartRemedialSessionAsync(studentId, lessonId);
            return Ok(ApiResponse<MemorizeSessionStartResultDto>.Success(result));
        }
    }
}
