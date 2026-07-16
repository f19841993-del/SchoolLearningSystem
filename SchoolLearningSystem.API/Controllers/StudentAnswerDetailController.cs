using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolLearningSystem.API.Responses;
using SchoolLearningSystem.Applicationf.DTOs.Analytics;
using SchoolLearningSystem.Applicationf.DTOs.StudentAnswer;
using SchoolLearningSystem.Applicationf.Interfaces;

namespace SchoolLearningSystem.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class StudentAnswerDetailController : ControllerBase
    {
        private readonly IStudentAnswerDetailService _service;

        public StudentAnswerDetailController(IStudentAnswerDetailService service)
        {
            _service = service;
        }

        // 🔹 CRUD الأساسي

        /// <summary>كل تفاصيل إجابات كل الطلاب</summary>
        /// <remarks>الصلاحيات: Admin, Teacher فقط.</remarks>
        [Tags("StudentAnswerDetail - الأساسيات (CRUD)")]
        [HttpGet]
        [Authorize(Roles = "Admin,Teacher")]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<StudentAnswerDetailReadDto>>), StatusCodes.Status200OK)]
        public async Task<ActionResult<ApiResponse<IEnumerable<StudentAnswerDetailReadDto>>>> GetAll()
        {
            var data = await _service.GetAllAsync();
            return Ok(new ApiResponse<IEnumerable<StudentAnswerDetailReadDto>>(200, "Answers retrieved successfully", data));
        }

        /// <summary>تفاصيل إجابة واحدة</summary>
        /// <remarks>الصلاحيات: Admin, Teacher فقط.</remarks>
        /// <response code="404">سجل الإجابة غير موجود</response>
        [Tags("StudentAnswerDetail - الأساسيات (CRUD)")]
        [HttpGet("{id}")]
        [Authorize(Roles = "Admin,Teacher")]
        [ProducesResponseType(typeof(ApiResponse<StudentAnswerDetailReadDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApiResponse<StudentAnswerDetailReadDto>>> GetById(int id)
        {
            var data = await _service.GetByIdAsync(id);
            if (data == null)
                return NotFound(new ApiResponse(404, "Answer record not found"));

            return Ok(new ApiResponse<StudentAnswerDetailReadDto>(200, "Answer retrieved successfully", data));
        }

        /// <summary>تسجيل إجابة جديدة مباشرة (استخدام إداري/اختباري)</summary>
        /// <remarks>
        /// الصلاحيات: Admin, Teacher فقط. المسار الرسمي لتسجيل إجابات الطلاب فعلياً
        /// هو POST /api/srs/submit-answer (يحدّث خوارزمية SM-2 بنفس الوقت).
        ///
        /// مثال Request:
        /// {
        ///   "studentId": 1,
        ///   "questionId": 3,
        ///   "memorizeSessionId": 5,
        ///   "selectedAnswer": "4",
        ///   "isCorrect": true,
        ///   "quality": 4,
        ///   "timeTakenInSeconds": 12
        /// }
        /// </remarks>
        /// <response code="400">بيانات غير صالحة</response>
        /// <response code="401">لم يتم تسجيل الدخول</response>
        /// <response code="403">الدور الحالي غير مسموح له</response>
        [Tags("StudentAnswerDetail - الأساسيات (CRUD)")]
        [HttpPost]
        [Authorize(Roles = "Admin,Teacher")]
        [ProducesResponseType(typeof(ApiResponse<StudentAnswerDetailReadDto>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<ApiResponse<StudentAnswerDetailReadDto>>> Add([FromBody] StudentAnswerDetailCreateDto dto)
        {
            var created = await _service.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = created.Id },
                new ApiResponse<StudentAnswerDetailReadDto>(201, "Answer recorded successfully", created));
        }

        /// <summary>تعديل تفاصيل إجابة (نادراً ما تُستخدم)</summary>
        /// <remarks>
        /// الصلاحيات: Admin, Teacher فقط. كل الحقول اختيارية.
        ///
        /// مثال Request:
        /// { "quality": 3 }
        /// </remarks>
        /// <response code="404">سجل الإجابة غير موجود</response>
        /// <response code="401">لم يتم تسجيل الدخول</response>
        /// <response code="403">الدور الحالي غير مسموح له</response>
        [Tags("StudentAnswerDetail - الأساسيات (CRUD)")]
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin,Teacher")]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<ApiResponse>> Update(int id, [FromBody] StudentAnswerDetailUpdateDto dto)
        {
            await _service.UpdateAsync(id, dto);
            return Ok(new ApiResponse(200, "Answer updated successfully"));
        }

        /// <summary>حذف تفاصيل إجابة</summary>
        /// <remarks>الصلاحيات: Admin, Teacher فقط.</remarks>
        /// <response code="404">سجل الإجابة غير موجود</response>
        /// <response code="401">لم يتم تسجيل الدخول</response>
        /// <response code="403">الدور الحالي غير مسموح له</response>
        [Tags("StudentAnswerDetail - الأساسيات (CRUD)")]
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin,Teacher")]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<ApiResponse>> Delete(int id)
        {
            await _service.DeleteAsync(id);
            return Ok(new ApiResponse(200, "Answer deleted successfully"));
        }

        // 🔹 علاقات إضافية — مطابقة حرفياً لـ IStudentAnswerDetailService

        /// <summary>كل إجابات طالب معيّن</summary>
        /// <remarks>الصلاحيات: Admin, Teacher, Student. الطالب يشوف بس إجاباته هو.</remarks>
        /// <response code="403">طالب يحاول يشوف إجابات طالب ثاني</response>
        [Tags("StudentAnswerDetail - الاستعلام والعلاقات (Queries)")]
        [HttpGet("student/{studentId}")]
        [Authorize(Roles = "Admin,Teacher,Student")]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<StudentAnswerDetailReadDto>>), StatusCodes.Status200OK)]
        public async Task<ActionResult<ApiResponse<IEnumerable<StudentAnswerDetailReadDto>>>> GetByStudent(int studentId)
        {
            if (User.IsInRole("Student") && User.FindFirstValue("studentId") != studentId.ToString())
                return Forbid();

            var data = await _service.GetAnswersByStudentIdAsync(studentId);
            return Ok(new ApiResponse<IEnumerable<StudentAnswerDetailReadDto>>(200, "Answers by student retrieved successfully", data));
        }

        /// <summary>كل إجابات سؤال معيّن (كل الطلاب)</summary>
        /// <remarks>الصلاحيات: Admin, Teacher فقط.</remarks>
        [Tags("StudentAnswerDetail - الاستعلام والعلاقات (Queries)")]
        [HttpGet("question/{questionId}")]
        [Authorize(Roles = "Admin,Teacher")]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<StudentAnswerDetailReadDto>>), StatusCodes.Status200OK)]
        public async Task<ActionResult<ApiResponse<IEnumerable<StudentAnswerDetailReadDto>>>> GetByQuestion(int questionId)
        {
            var data = await _service.GetAnswersByQuestionIdAsync(questionId);
            return Ok(new ApiResponse<IEnumerable<StudentAnswerDetailReadDto>>(200, "Answers by question retrieved successfully", data));
        }

        /// <summary>آخر N إجابة لطالب معيّن</summary>
        /// <remarks>الصلاحيات: Admin, Teacher, Student. الطالب يشوف بس إجاباته هو.</remarks>
        /// <response code="403">طالب يحاول يشوف إجابات طالب ثاني</response>
        [Tags("StudentAnswerDetail - الاستعلام والعلاقات (Queries)")]
        [HttpGet("recent/{studentId}/{count}")]
        [Authorize(Roles = "Admin,Teacher,Student")]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<StudentAnswerDetailReadDto>>), StatusCodes.Status200OK)]
        public async Task<ActionResult<ApiResponse<IEnumerable<StudentAnswerDetailReadDto>>>> GetRecent(int studentId, int count)
        {
            if (User.IsInRole("Student") && User.FindFirstValue("studentId") != studentId.ToString())
                return Forbid();

            var data = await _service.GetRecentAnswersByStudentIdAsync(studentId, count);
            return Ok(new ApiResponse<IEnumerable<StudentAnswerDetailReadDto>>(200, "Recent answers retrieved successfully", data));
        }

        /// <summary>الإجابات الخاطئة لطالب معيّن بدرس معيّن</summary>
        /// <remarks>الصلاحيات: Admin, Teacher, Student. الطالب يشوف بس إجاباته هو.</remarks>
        /// <response code="403">طالب يحاول يشوف إجابات طالب ثاني</response>
        [Tags("StudentAnswerDetail - الاستعلام والعلاقات (Queries)")]
        [HttpGet("incorrect/{studentId}/{lessonId}")]
        [Authorize(Roles = "Admin,Teacher,Student")]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<StudentAnswerDetailReadDto>>), StatusCodes.Status200OK)]
        public async Task<ActionResult<ApiResponse<IEnumerable<StudentAnswerDetailReadDto>>>> GetIncorrectAnswers(int studentId, int lessonId)
        {
            if (User.IsInRole("Student") && User.FindFirstValue("studentId") != studentId.ToString())
                return Forbid();

            var data = await _service.GetIncorrectAnswersByStudentIdAsync(studentId, lessonId);
            return Ok(new ApiResponse<IEnumerable<StudentAnswerDetailReadDto>>(200, "Incorrect answers retrieved successfully", data));
        }

        /// <summary>الأسئلة الأصعب (حسب نسبة الخطأ) — لتحليل المحتوى الأصعب على الطلاب</summary>
        /// <remarks>الصلاحيات: Admin, Teacher فقط.</remarks>
        [Tags("StudentAnswerDetail - الإحصائيات (Statistics)")]
        [HttpGet("hardest-questions")]
        [Authorize(Roles = "Admin,Teacher")]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<QuestionDifficultyStatsDto>>), StatusCodes.Status200OK)]
        public async Task<ActionResult<ApiResponse<IEnumerable<QuestionDifficultyStatsDto>>>> GetHardestQuestions(
    [FromQuery] int? lessonId, [FromQuery] int topN = 10)
        {
            var data = await _service.GetHardestQuestionsAsync(lessonId, topN);
            return Ok(new ApiResponse<IEnumerable<QuestionDifficultyStatsDto>>(200, "تم جلب الأسئلة الأصعب بنجاح.", data));
        }
    }
}
