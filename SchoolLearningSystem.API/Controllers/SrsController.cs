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

        /// <summary>
        /// تسجيل إجابة الطالب وتحديث حالة التعلم (SRS)
        /// </summary>
        [HttpPost("submit-answer")]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ApiResponse<string>>> SubmitAnswer([FromBody] AnswerSubmissionDto dto)
        {
            try
            {
                await _srsService.ProcessAnswerAsync(dto.StudentId, dto.QuestionId, dto.IsCorrect, dto.TimeTakenInSeconds);
                return Ok(new ApiResponse<string>(200, "Answer processed and progress updated"));
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse<string>(400, ex.Message));
            }
        }

        /// <summary>
        /// جلب الأسئلة التي يحتاج الطالب لمراجعتها الآن
        /// </summary>
        [HttpGet("due-questions/{studentId}")]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<StudentQuestionProgressReadDto>>), StatusCodes.Status200OK)]
        public async Task<ActionResult<ApiResponse<IEnumerable<StudentQuestionProgressReadDto>>>> GetDueQuestions(int studentId)
        {
            var dueQuestions = await _srsService.GetDueQuestionsForSessionAsync(studentId);
            return Ok(new ApiResponse<IEnumerable<StudentQuestionProgressReadDto>>(200, "Due questions retrieved successfully", dueQuestions));
        }
    }
}