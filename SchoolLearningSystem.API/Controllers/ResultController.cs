using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolLearningSystem.API.Responses;
using SchoolLearningSystem.Applicationf.DTOs.Result;
using SchoolLearningSystem.Applicationf.Interfaces;

namespace SchoolLearningSystem.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ResultController : ControllerBase
    {
        private readonly IResultService _resultService;

        public ResultController(IResultService resultService)
        {
            _resultService = resultService;
        }

        // 🔹 CRUD الأساسي

        /// <summary>كل النتائج (كل الطلاب مجتمعين)</summary>
        /// <remarks>الصلاحيات: Admin, Teacher فقط.</remarks>
        [Tags("Result - الأساسيات (CRUD)")]
        [HttpGet]
        [Authorize(Roles = "Admin,Teacher")]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<ResultReadDto>>), StatusCodes.Status200OK)]
        public async Task<ActionResult<ApiResponse<IEnumerable<ResultReadDto>>>> GetAll()
        {
            var data = await _resultService.GetAllAsync();
            return Ok(new ApiResponse<IEnumerable<ResultReadDto>>(200, "Results retrieved successfully", data));
        }

        /// <summary>نتيجة واحدة بالتفصيل</summary>
        /// <remarks>الصلاحيات: Admin, Teacher فقط.</remarks>
        /// <response code="404">النتيجة غير موجودة</response>
        [Tags("Result - الأساسيات (CRUD)")]
        [HttpGet("{id}")]
        [Authorize(Roles = "Admin,Teacher")]
        [ProducesResponseType(typeof(ApiResponse<ResultReadDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApiResponse<ResultReadDto>>> GetById(int id)
        {
            var data = await _resultService.GetByIdAsync(id);
            if (data == null)
                return NotFound(new ApiResponse(404, "Result not found"));

            return Ok(new ApiResponse<ResultReadDto>(200, "Result retrieved successfully", data));
        }

        /// <summary>تسجيل نتيجة جديدة</summary>
        /// <remarks>
        /// الصلاحيات: Admin, Teacher فقط. لازم إما lessonId أو examId على الأقل (تحقق داخل الـ Service).
        ///
        /// مثال Request:
        /// {
        ///   "studentId": 1,
        ///   "lessonId": 1,
        ///   "examId": null,
        ///   "resultType": "Homework",
        ///   "score": 85,
        ///   "durationInSeconds": 300
        /// }
        /// </remarks>
        /// <response code="400">بيانات غير صالحة (مثلاً lessonId وexamId كلاهما فاضي)</response>
        /// <response code="401">لم يتم تسجيل الدخول</response>
        /// <response code="403">الدور الحالي غير مسموح له</response>
        [Tags("Result - الأساسيات (CRUD)")]
        [HttpPost]
        [Authorize(Roles = "Admin,Teacher")]
        [ProducesResponseType(typeof(ApiResponse<ResultReadDto>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<ApiResponse<ResultReadDto>>> Add([FromBody] ResultCreateDto dto)
        {
            // ⚠️ تذكير: قاعدة "لازم Lesson أو Exam واحد على الأقل" يجب أن تُفرض
            // داخل ResultService.CreateAsync (وترمي CustomValidationException عند خرقها)
            var createdResult = await _resultService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = createdResult.Id },
                new ApiResponse<ResultReadDto>(201, "Result created successfully", createdResult));
        }

        /// <summary>تعديل نتيجة موجودة</summary>
        /// <remarks>
        /// الصلاحيات: Admin, Teacher فقط. كل الحقول اختيارية.
        ///
        /// مثال Request:
        /// { "score": 90 }
        /// </remarks>
        /// <response code="404">النتيجة غير موجودة</response>
        /// <response code="401">لم يتم تسجيل الدخول</response>
        /// <response code="403">الدور الحالي غير مسموح له</response>
        [Tags("Result - الأساسيات (CRUD)")]
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin,Teacher")]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<ApiResponse>> Update(int id, [FromBody] ResultUpdateDto dto)
        {
            await _resultService.UpdateAsync(id, dto);
            return Ok(new ApiResponse(200, "Result updated successfully"));
        }

        /// <summary>حذف نتيجة (Soft Delete)</summary>
        /// <remarks>الصلاحيات: Admin, Teacher فقط.</remarks>
        /// <response code="404">النتيجة غير موجودة</response>
        /// <response code="401">لم يتم تسجيل الدخول</response>
        /// <response code="403">الدور الحالي غير مسموح له</response>
        [Tags("Result - الأساسيات (CRUD)")]
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin,Teacher")]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<ApiResponse>> Delete(int id)
        {
            await _resultService.DeleteAsync(id);
            return Ok(new ApiResponse(200, "Result deleted successfully"));
        }

        // 🔹 علاقات إضافية (Custom Business Logic)

        /// <summary>كل نتائج طالب معيّن</summary>
        /// <remarks>
        /// الصلاحيات: Admin, Teacher, Student. الطالب يشوف بس نتائجه هو (يتحقق studentId بالتوكن).
        /// </remarks>
        /// <response code="403">طالب يحاول يشوف نتائج طالب ثاني</response>
        [Tags("Result - الاستعلام والعلاقات (Queries)")]
        [HttpGet("student/{studentId}")]
        [Authorize(Roles = "Admin,Teacher,Student")]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<ResultReadDto>>), StatusCodes.Status200OK)]
        public async Task<ActionResult<ApiResponse<IEnumerable<ResultReadDto>>>> GetByStudentId(int studentId)
        {
            if (User.IsInRole("Student") && User.FindFirstValue("studentId") != studentId.ToString())
                return Forbid();

            var data = await _resultService.GetResultsByStudentIdAsync(studentId);
            return Ok(new ApiResponse<IEnumerable<ResultReadDto>>(200, "Results retrieved successfully", data));
        }

        /// <summary>كل نتائج درس معيّن (كل الطلاب)</summary>
        /// <remarks>الصلاحيات: Admin, Teacher فقط.</remarks>
        [Tags("Result - الاستعلام والعلاقات (Queries)")]
        [HttpGet("lesson/{lessonId}")]
        [Authorize(Roles = "Admin,Teacher")]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<ResultReadDto>>), StatusCodes.Status200OK)]
        public async Task<ActionResult<ApiResponse<IEnumerable<ResultReadDto>>>> GetByLessonId(int lessonId)
        {
            var data = await _resultService.GetResultsByLessonIdAsync(lessonId);
            return Ok(new ApiResponse<IEnumerable<ResultReadDto>>(200, "Results retrieved successfully", data));
        }

        /// <summary>كل نتائج امتحان معيّن (كل الطلاب)</summary>
        /// <remarks>الصلاحيات: Admin, Teacher فقط.</remarks>
        [Tags("Result - الاستعلام والعلاقات (Queries)")]
        [HttpGet("exam/{examId}")]
        [Authorize(Roles = "Admin,Teacher")]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<ResultReadDto>>), StatusCodes.Status200OK)]
        public async Task<ActionResult<ApiResponse<IEnumerable<ResultReadDto>>>> GetByExamId(int examId)
        {
            var data = await _resultService.GetResultsByExamIdAsync(examId);
            return Ok(new ApiResponse<IEnumerable<ResultReadDto>>(200, "Results retrieved successfully", data));
        }

        // 🔹 إحصائيات

        /// <summary>متوسط درجات طالب معيّن</summary>
        /// <remarks>
        /// الصلاحيات: Admin, Teacher, Student. الطالب يشوف بس متوسطه هو.
        /// </remarks>
        /// <response code="403">طالب يحاول يشوف متوسط طالب ثاني</response>
        [Tags("Result - الإحصائيات (Statistics)")]
        [HttpGet("student/{studentId}/average")]
        [Authorize(Roles = "Admin,Teacher,Student")]
        [ProducesResponseType(typeof(ApiResponse<double>), StatusCodes.Status200OK)]
        public async Task<ActionResult<ApiResponse<double>>> GetAverageByStudentId(int studentId)
        {
            if (User.IsInRole("Student") && User.FindFirstValue("studentId") != studentId.ToString())
                return Forbid();

            var avg = await _resultService.GetAverageScoreByStudentIdAsync(studentId);
            return Ok(new ApiResponse<double>(200, "Average score retrieved successfully", avg));
        }

        /// <summary>متوسط درجات درس معيّن (كل الطلاب)</summary>
        /// <remarks>الصلاحيات: Admin, Teacher فقط.</remarks>
        [Tags("Result - الإحصائيات (Statistics)")]
        [HttpGet("lesson/{lessonId}/average")]
        [Authorize(Roles = "Admin,Teacher")]
        [ProducesResponseType(typeof(ApiResponse<double>), StatusCodes.Status200OK)]
        public async Task<ActionResult<ApiResponse<double>>> GetAverageByLessonId(int lessonId)
        {
            var avg = await _resultService.GetAverageScoreByLessonIdAsync(lessonId);
            return Ok(new ApiResponse<double>(200, "Average score retrieved successfully", avg));
        }

        /// <summary>متوسط درجات امتحان معيّن (كل الطلاب)</summary>
        /// <remarks>الصلاحيات: Admin, Teacher فقط.</remarks>
        [Tags("Result - الإحصائيات (Statistics)")]
        [HttpGet("exam/{examId}/average")]
        [Authorize(Roles = "Admin,Teacher")]
        [ProducesResponseType(typeof(ApiResponse<double>), StatusCodes.Status200OK)]
        public async Task<ActionResult<ApiResponse<double>>> GetAverageByExamId(int examId)
        {
            var avg = await _resultService.GetAverageScoreByExamIdAsync(examId);
            return Ok(new ApiResponse<double>(200, "Average score retrieved successfully", avg));
        }
    }
}
