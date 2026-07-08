using Microsoft.AspNetCore.Mvc;
using SchoolLearningSystem.API.Responses;
using SchoolLearningSystem.Applicationf.DTOs.Srs;
using SchoolLearningSystem.Applicationf.DTOs.StudentQuestionProgress;
using SchoolLearningSystem.Applicationf.Interfaces;

namespace SchoolLearningSystem.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
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

        /// <summary>
        /// يستقبل إجابة الطالب ويقوم بتحديث مساره التعليمي (خوارزمية SM-2)
        /// </summary>
        [HttpPost("submit-answer")]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ApiResponse>> SubmitAnswer([FromBody] AnswerSubmissionDto dto)
        {
            await _srsService.ProcessAnswerAsync(dto);
            return Ok(new ApiResponse(200, "تم تسجيل الإجابة وتحديث مسار الطالب بنجاح."));
        }

        /// <summary>
        /// الأسئلة المستحقة المراجعة الآن لطالب معيّن — تُستخدم عند بدء جلسة جديدة.
        /// يمكن تمرير تاريخ وهمي عبر currentDate لاختبار الخوارزمية (Time Travel Simulation)
        /// </summary>
        [HttpGet("due-questions/{studentId}")]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<StudentQuestionProgressReadDto>>), StatusCodes.Status200OK)]
        public async Task<ActionResult<ApiResponse<IEnumerable<StudentQuestionProgressReadDto>>>> GetDueQuestions(
            int studentId, [FromQuery] DateTime? currentDate = null)
        {
            var dueQuestions = await _srsService.GetDueQuestionsForSessionAsync(studentId, currentDate);
            return Ok(new ApiResponse<IEnumerable<StudentQuestionProgressReadDto>>(200, "تم جلب أسئلة المراجعة بنجاح.", dueQuestions));
        }

        // ==========================================
        // 🔹 تحليلات ولوحات تحكم (منقولة من IStudentQuestionProgressService المحذوفة)
        // ==========================================

        /// <summary>
        /// سجل تقدم طالب معيّن بكل الأسئلة (لتحليل شامل لمستواه)
        /// </summary>
        [HttpGet("progress/student/{studentId}")]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<StudentQuestionProgressReadDto>>), StatusCodes.Status200OK)]
        public async Task<ActionResult<ApiResponse<IEnumerable<StudentQuestionProgressReadDto>>>> GetProgressByStudent(int studentId)
        {
            var data = await _srsService.GetProgressByStudentIdAsync(studentId);
            return Ok(new ApiResponse<IEnumerable<StudentQuestionProgressReadDto>>(200, "Progress records retrieved successfully", data));
        }

        /// <summary>
        /// تقدم طالب بسؤال معيّن بالتحديد
        /// </summary>
        [HttpGet("progress/{studentId}/{questionId}")]
        [ProducesResponseType(typeof(ApiResponse<StudentQuestionProgressReadDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApiResponse<StudentQuestionProgressReadDto>>> GetProgressByStudentAndQuestion(
            int studentId, int questionId)
        {
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
