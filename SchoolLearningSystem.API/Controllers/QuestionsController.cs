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

        // 🔹 CRUD الأساسي (موروث من BaseService)

        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<QuestionReadDto>>), StatusCodes.Status200OK)]
        public async Task<ActionResult<ApiResponse<IEnumerable<QuestionReadDto>>>> GetAll()
        {
            var data = await _questionService.GetAllAsync();
            return Ok(new ApiResponse<IEnumerable<QuestionReadDto>>(200, "Questions retrieved successfully", data));
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ApiResponse<QuestionReadDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApiResponse<QuestionReadDto>>> GetById(int id)
        {
            var data = await _questionService.GetByIdAsync(id);
            if (data == null)
                return NotFound(new ApiResponse<string>(404, "Question not found"));

            return Ok(new ApiResponse<QuestionReadDto>(200, "Question retrieved successfully", data));
        }

        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<QuestionReadDto>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ApiResponse<QuestionReadDto>>> Add(QuestionCreateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ApiResponse<string>(400, "Invalid input data"));

            var createdQuestion = await _questionService.CreateAsync(dto);
            return StatusCode(201, new ApiResponse<QuestionReadDto>(201, "Question created successfully", createdQuestion));
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApiResponse<string>>> Update(int id, QuestionUpdateDto dto)
        {
            try
            {
                await _questionService.UpdateAsync(id, dto);
                return Ok(new ApiResponse<string>(200, "Question updated successfully"));
            }
            catch (Exception ex)
            {
                return NotFound(new ApiResponse<string>(404, ex.Message));
            }
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApiResponse<string>>> Delete(int id)
        {
            try
            {
                await _questionService.DeleteAsync(id);
                return Ok(new ApiResponse<string>(200, "Question deleted successfully"));
            }
            catch (Exception ex)
            {
                return NotFound(new ApiResponse<string>(404, ex.Message));
            }
        }

        // 🔹 علاقات إضافية (Custom Business Logic)

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

        // 🔹 إحصائيات

        [HttpGet("exam/{examId}/count")]
        [ProducesResponseType(typeof(ApiResponse<int>), StatusCodes.Status200OK)]
        public async Task<ActionResult<ApiResponse<int>>> GetCountByExamId(int examId)
        {
            var count = await _questionService.GetQuestionCountByExamIdAsync(examId);
            return Ok(new ApiResponse<int>(200, "Question count retrieved successfully", count));
        }

        [HttpGet("difficulty/{difficultyLevel}/count")]
        [ProducesResponseType(typeof(ApiResponse<int>), StatusCodes.Status200OK)]
        public async Task<ActionResult<ApiResponse<int>>> GetCountByDifficulty(DifficultyLevel difficultyLevel)
        {
            var count = await _questionService.GetQuestionCountByDifficultyAsync(difficultyLevel);
            return Ok(new ApiResponse<int>(200, "Question count by difficulty retrieved successfully", count));
        }
    }
}