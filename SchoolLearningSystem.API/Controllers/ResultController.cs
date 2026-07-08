using Microsoft.AspNetCore.Mvc;
using SchoolLearningSystem.API.Responses;
using SchoolLearningSystem.Applicationf.DTOs.Result;
using SchoolLearningSystem.Applicationf.Interfaces;

namespace SchoolLearningSystem.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ResultController : ControllerBase
    {
        private readonly IResultService _resultService;

        public ResultController(IResultService resultService)
        {
            _resultService = resultService;
        }

        // 🔹 CRUD الأساسي

        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<ResultReadDto>>), StatusCodes.Status200OK)]
        public async Task<ActionResult<ApiResponse<IEnumerable<ResultReadDto>>>> GetAll()
        {
            var data = await _resultService.GetAllAsync();
            return Ok(new ApiResponse<IEnumerable<ResultReadDto>>(200, "Results retrieved successfully", data));
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ApiResponse<ResultReadDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApiResponse<ResultReadDto>>> GetById(int id)
        {
            var data = await _resultService.GetByIdAsync(id);
            if (data == null)
                return NotFound(new ApiResponse(404, "Result not found"));

            return Ok(new ApiResponse<ResultReadDto>(200, "Result retrieved successfully", data));
        }

        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<ResultReadDto>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ApiResponse<ResultReadDto>>> Add([FromBody] ResultCreateDto dto)
        {
            // ⚠️ تذكير: قاعدة "لازم Lesson أو Exam واحد على الأقل" يجب أن تُفرض
            // داخل ResultService.CreateAsync (وترمي CustomValidationException عند خرقها)
            var createdResult = await _resultService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = createdResult.Id },
                new ApiResponse<ResultReadDto>(201, "Result created successfully", createdResult));
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApiResponse>> Update(int id, [FromBody] ResultUpdateDto dto)
        {
            await _resultService.UpdateAsync(id, dto);
            return Ok(new ApiResponse(200, "Result updated successfully"));
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApiResponse>> Delete(int id)
        {
            await _resultService.DeleteAsync(id);
            return Ok(new ApiResponse(200, "Result deleted successfully"));
        }

        // 🔹 علاقات إضافية (Custom Business Logic)

        [HttpGet("student/{studentId}")]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<ResultReadDto>>), StatusCodes.Status200OK)]
        public async Task<ActionResult<ApiResponse<IEnumerable<ResultReadDto>>>> GetByStudentId(int studentId)
        {
            var data = await _resultService.GetResultsByStudentIdAsync(studentId);
            return Ok(new ApiResponse<IEnumerable<ResultReadDto>>(200, "Results retrieved successfully", data));
        }

        [HttpGet("lesson/{lessonId}")]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<ResultReadDto>>), StatusCodes.Status200OK)]
        public async Task<ActionResult<ApiResponse<IEnumerable<ResultReadDto>>>> GetByLessonId(int lessonId)
        {
            var data = await _resultService.GetResultsByLessonIdAsync(lessonId);
            return Ok(new ApiResponse<IEnumerable<ResultReadDto>>(200, "Results retrieved successfully", data));
        }

        [HttpGet("exam/{examId}")]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<ResultReadDto>>), StatusCodes.Status200OK)]
        public async Task<ActionResult<ApiResponse<IEnumerable<ResultReadDto>>>> GetByExamId(int examId)
        {
            var data = await _resultService.GetResultsByExamIdAsync(examId);
            return Ok(new ApiResponse<IEnumerable<ResultReadDto>>(200, "Results retrieved successfully", data));
        }

        // 🔹 إحصائيات

        [HttpGet("student/{studentId}/average")]
        [ProducesResponseType(typeof(ApiResponse<double>), StatusCodes.Status200OK)]
        public async Task<ActionResult<ApiResponse<double>>> GetAverageByStudentId(int studentId)
        {
            var avg = await _resultService.GetAverageScoreByStudentIdAsync(studentId);
            return Ok(new ApiResponse<double>(200, "Average score retrieved successfully", avg));
        }

        [HttpGet("lesson/{lessonId}/average")]
        [ProducesResponseType(typeof(ApiResponse<double>), StatusCodes.Status200OK)]
        public async Task<ActionResult<ApiResponse<double>>> GetAverageByLessonId(int lessonId)
        {
            var avg = await _resultService.GetAverageScoreByLessonIdAsync(lessonId);
            return Ok(new ApiResponse<double>(200, "Average score retrieved successfully", avg));
        }

        [HttpGet("exam/{examId}/average")]
        [ProducesResponseType(typeof(ApiResponse<double>), StatusCodes.Status200OK)]
        public async Task<ActionResult<ApiResponse<double>>> GetAverageByExamId(int examId)
        {
            var avg = await _resultService.GetAverageScoreByExamIdAsync(examId);
            return Ok(new ApiResponse<double>(200, "Average score retrieved successfully", avg));
        }
    }
}
