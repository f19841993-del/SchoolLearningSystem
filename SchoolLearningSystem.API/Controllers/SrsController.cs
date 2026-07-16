using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolLearningSystem.API.Responses;
using SchoolLearningSystem.Applicationf.DTOs.Srs;
using SchoolLearningSystem.Applicationf.DTOs.StudentQuestionProgress;
using SchoolLearningSystem.Applicationf.Interfaces;

namespace SchoolLearningSystem.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin,Teacher,Student")]
    public class SrsController : ControllerBase
    {
        private readonly ISrsService _srsService;

        public SrsController(ISrsService srsService)
        {
            _srsService = srsService;
        }

        // ==========================================
        // 🔹 قلب محرك التكرار المتباعد (SRS Engine)
        // ==========================================

        /// <summary>يستقبل إجابة الطالب ويقوم بتحديث مساره التعليمي (خوارزمية SM-2)</summary>
        /// <remarks>
        /// الصلاحيات: Admin, Teacher, Student. الطالب يقدر يرسل بس إجاباته هو (يتحقق dto.StudentId بالتوكن).
        ///
        /// مثال Request:
        /// {
        ///   "studentId": 1,
        ///   "questionId": 3,
        ///   "memorizeSessionId": 5,
        ///   "selectedAnswer": "4",
        ///   "quality": 4,
        ///   "timeTakenInSeconds": 12
        /// }
        /// </remarks>
        /// <response code="400">بيانات غير صالحة</response>
        /// <response code="403">طالب يحاول يرسل إجابة نيابة عن طالب ثاني</response>
        [HttpPost("submit-answer")]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ApiResponse>> SubmitAnswer([FromBody] AnswerSubmissionDto dto)
        {
            if (User.IsInRole("Student") && User.FindFirstValue("studentId") != dto.StudentId.ToString())
                return Forbid();

            await _srsService.ProcessAnswerAsync(dto);
            return Ok(new ApiResponse(200, "تم تسجيل الإجابة وتحديث مسار الطالب بنجاح."));
        }

        /// <summary>الأسئلة المستحقة المراجعة الآن لطالب معيّن — تُستخدم عند بدء جلسة جديدة</summary>
        /// <remarks>
        /// الصلاحيات: Admin, Teacher, Student. الطالب يشوف بس أسئلته هو.
        /// يمكن تمرير تاريخ وهمي عبر currentDate لاختبار الخوارزمية (Time Travel Simulation).
        /// </remarks>
        /// <response code="403">طالب يحاول يشوف أسئلة طالب ثاني</response>
        [HttpGet("due-questions/{studentId}")]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<StudentQuestionProgressReadDto>>), StatusCodes.Status200OK)]
        public async Task<ActionResult<ApiResponse<IEnumerable<StudentQuestionProgressReadDto>>>> GetDueQuestions(
            int studentId, [FromQuery] DateTime? currentDate = null)
        {
            if (User.IsInRole("Student") && User.FindFirstValue("studentId") != studentId.ToString())
                return Forbid();

            var dueQuestions = await _srsService.GetDueQuestionsForSessionAsync(studentId, currentDate);
            return Ok(new ApiResponse<IEnumerable<StudentQuestionProgressReadDto>>(200, "تم جلب أسئلة المراجعة بنجاح.", dueQuestions));
        }

        // ==========================================
        // 🔹 تحليلات ولوحات تحكم (منقولة من IStudentQuestionProgressService المحذوفة)
        // ==========================================

        /// <summary>سجل تقدم طالب معيّن بكل الأسئلة (لتحليل شامل لمستواه)</summary>
        /// <remarks>الصلاحيات: Admin, Teacher, Student. الطالب يشوف بس تقدمه هو.</remarks>
        /// <response code="403">طالب يحاول يشوف تقدم طالب ثاني</response>
        [HttpGet("progress/student/{studentId}")]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<StudentQuestionProgressReadDto>>), StatusCodes.Status200OK)]
        public async Task<ActionResult<ApiResponse<IEnumerable<StudentQuestionProgressReadDto>>>> GetProgressByStudent(int studentId)
        {
            if (User.IsInRole("Student") && User.FindFirstValue("studentId") != studentId.ToString())
                return Forbid();

            var data = await _srsService.GetProgressByStudentIdAsync(studentId);
            return Ok(new ApiResponse<IEnumerable<StudentQuestionProgressReadDto>>(200, "Progress records retrieved successfully", data));
        }

        /// <summary>تقدم طالب بسؤال معيّن بالتحديد</summary>
        /// <remarks>الصلاحيات: Admin, Teacher, Student. الطالب يشوف بس تقدمه هو.</remarks>
        /// <response code="404">لا يوجد سجل تقدم لهذا الطالب بهذا السؤال</response>
        /// <response code="403">طالب يحاول يشوف تقدم طالب ثاني</response>
        [HttpGet("progress/{studentId}/{questionId}")]
        [ProducesResponseType(typeof(ApiResponse<StudentQuestionProgressReadDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApiResponse<StudentQuestionProgressReadDto>>> GetProgressByStudentAndQuestion(
            int studentId, int questionId)
        {
            if (User.IsInRole("Student") && User.FindFirstValue("studentId") != studentId.ToString())
                return Forbid();

            var data = await _srsService.GetProgressByStudentAndQuestionAsync(studentId, questionId);
            if (data == null)
                return NotFound(new ApiResponse(404, "Progress record not found"));

            return Ok(new ApiResponse<StudentQuestionProgressReadDto>(200, "Progress record retrieved successfully", data));
        }

        // 🗑️ ملاحظة: لا يوجد AddProgressAsync/UpdateProgressAsync هنا عمداً — كانت بالكونترولر
        // القديم StudentQuestionProgressController (المحذوف كاملاً). حسب ISrsService الفعلية،
        // السجل يتحدّث فقط تلقائياً من داخل ProcessAnswerAsync (submit-answer) — مافيه تعديل
        // يدوي مباشر لسجل التقدّم، وهذا صحيح معمارياً (يمنع كسر دقة خوارزمية SM-2).
    }
}
