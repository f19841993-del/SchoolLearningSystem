using Microsoft.AspNetCore.Mvc;
using SchoolLearningSystem.API.Responses;
using SchoolLearningSystem.Applicationf.DTOs.Question;
using SchoolLearningSystem.Applicationf.Interfaces;
using SchoolLearningSystem.Domain.Enums;

namespace SchoolLearningSystem.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class QuestionController : ControllerBase
    {
        private readonly IQuestionService _questionService;

        public QuestionController(IQuestionService questionService)
        {
            _questionService = questionService;
        }

        // 🔹 CRUD الأساسي

        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<QuestionReadDto>>), StatusCodes.Status200OK)]
        public async Task<ActionResult<ApiResponse<IEnumerable<QuestionReadDto>>>> GetAll()
        {
            var data = await _questionService.GetAllAsync();
            return Ok(new ApiResponse<IEnumerable<QuestionReadDto>>(200, "Questions retrieved successfully", data));
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ApiResponse<QuestionReadDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApiResponse<QuestionReadDto>>> GetById(int id)
        {
            var data = await _questionService.GetByIdAsync(id);
            if (data == null)
                return NotFound(new ApiResponse(404, "Question not found"));

            return Ok(new ApiResponse<QuestionReadDto>(200, "Question retrieved successfully", data));
        }

        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<QuestionReadDto>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ApiResponse<QuestionReadDto>>> Add([FromBody] QuestionCreateDto dto)
        {
            var createdQuestion = await _questionService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = createdQuestion.Id },
                new ApiResponse<QuestionReadDto>(201, "Question created successfully", createdQuestion));
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApiResponse>> Update(int id, [FromBody] QuestionUpdateDto dto)
        {
            await _questionService.UpdateAsync(id, dto);
            return Ok(new ApiResponse(200, "Question updated successfully"));
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApiResponse>> Delete(int id)
        {
            await _questionService.DeleteAsync(id);
            return Ok(new ApiResponse(200, "Question deleted successfully"));
        }

        // 🔹 علاقات إضافية — مطابقة حرفياً لـ IQuestionService

        [HttpGet("exam/{examId}")]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<QuestionReadDto>>), StatusCodes.Status200OK)]
        public async Task<ActionResult<ApiResponse<IEnumerable<QuestionReadDto>>>> GetByExamId(int examId)
        {
            var data = await _questionService.GetQuestionsByExamIdAsync(examId);
            return Ok(new ApiResponse<IEnumerable<QuestionReadDto>>(200, "Questions retrieved successfully", data));
        }

        [HttpGet("lesson/{lessonId}")]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<QuestionReadDto>>), StatusCodes.Status200OK)]
        public async Task<ActionResult<ApiResponse<IEnumerable<QuestionReadDto>>>> GetByLessonId(int lessonId)
        {
            var data = await _questionService.GetQuestionsByLessonIdAsync(lessonId);
            return Ok(new ApiResponse<IEnumerable<QuestionReadDto>>(200, "Questions retrieved successfully", data));
        }

        /// <summary>
        /// أسئلة حسب مستوى صعوبة معيّن — يستخدمها محرك الـ AI لبناء اختبار تكيّفي
        /// </summary>
        [HttpGet("difficulty/{difficulty}")]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<QuestionReadDto>>), StatusCodes.Status200OK)]
        public async Task<ActionResult<ApiResponse<IEnumerable<QuestionReadDto>>>> GetByDifficulty(DifficultyLevel difficulty)
        {
            var data = await _questionService.GetQuestionsByDifficultyAsync(difficulty);
            return Ok(new ApiResponse<IEnumerable<QuestionReadDto>>(200, "Questions retrieved successfully", data));
        }

        // 🔹 إحصائيات
        // ⚠️ تصحيح أسماء الدوال: كانت GetQuestionCountByExamIdAsync/GetQuestionCountByDifficultyAsync
        // (غير موجودتين) — الاسم الصحيح بالـ Interface هو CountByExamIdAsync/CountByDifficultyAsync

        [HttpGet("exam/{examId}/count")]
        [ProducesResponseType(typeof(ApiResponse<int>), StatusCodes.Status200OK)]
        public async Task<ActionResult<ApiResponse<int>>> GetCountByExamId(int examId)
        {
            var count = await _questionService.CountByExamIdAsync(examId);
            return Ok(new ApiResponse<int>(200, "Question count retrieved successfully", count));
        }

        [HttpGet("difficulty/{difficulty}/count")]
        [ProducesResponseType(typeof(ApiResponse<int>), StatusCodes.Status200OK)]
        public async Task<ActionResult<ApiResponse<int>>> GetCountByDifficulty(DifficultyLevel difficulty)
        {
            var count = await _questionService.CountByDifficultyAsync(difficulty);
            return Ok(new ApiResponse<int>(200, "Question count by difficulty retrieved successfully", count));
        }

        [HttpGet("lesson/{lessonId}/count")]
        [ProducesResponseType(typeof(ApiResponse<int>), StatusCodes.Status200OK)]
        public async Task<ActionResult<ApiResponse<int>>> GetTotalQuestionsByLessonId(int lessonId)
        {
            var count = await _questionService.GetTotalQuestionsByLessonIdAsync(lessonId);
            return Ok(new ApiResponse<int>(200, "Total questions count retrieved successfully", count));
        }
    }
}
