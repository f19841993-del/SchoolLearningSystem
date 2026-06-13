using Microsoft.AspNetCore.Mvc;
using SchoolLearningSystem.API.Responses;
using SchoolLearningSystem.Applicationf.DTOs.StudentAnswer;
using SchoolLearningSystem.Applicationf.Interfaces;

namespace SchoolLearningSystem.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StudentAnswerDetailController : ControllerBase
    {
        private readonly IStudentAnswerDetailService _service;

        public StudentAnswerDetailController(IStudentAnswerDetailService service)
        {
            _service = service;
        }

        // 🔹 CRUD الأساسي (موروث من BaseService)

        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<StudentAnswerDetailReadDto>>), StatusCodes.Status200OK)]
        public async Task<ActionResult<ApiResponse<IEnumerable<StudentAnswerDetailReadDto>>>> GetAll()
        {
            var data = await _service.GetAllAsync();
            return Ok(new ApiResponse<IEnumerable<StudentAnswerDetailReadDto>>(200, "Answers retrieved successfully", data));
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ApiResponse<StudentAnswerDetailReadDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApiResponse<StudentAnswerDetailReadDto>>> GetById(int id)
        {
            var data = await _service.GetByIdAsync(id);
            if (data == null)
                return NotFound(new ApiResponse<string>(404, "Answer record not found"));

            return Ok(new ApiResponse<StudentAnswerDetailReadDto>(200, "Answer retrieved successfully", data));
        }

        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<StudentAnswerDetailReadDto>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ApiResponse<StudentAnswerDetailReadDto>>> Add(StudentAnswerDetailCreateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ApiResponse<string>(400, "Invalid input data"));

            var createdAnswer = await _service.CreateAsync(dto);
            return StatusCode(201, new ApiResponse<StudentAnswerDetailReadDto>(201, "Answer recorded successfully", createdAnswer));
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApiResponse<string>>> Update(int id, StudentAnswerDetailUpdateDto dto)
        {
            try
            {
                await _service.UpdateAsync(id, dto);
                return Ok(new ApiResponse<string>(200, "Answer updated successfully"));
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
                await _service.DeleteAsync(id);
                return Ok(new ApiResponse<string>(200, "Answer deleted successfully"));
            }
            catch (Exception ex)
            {
                return NotFound(new ApiResponse<string>(404, ex.Message));
            }
        }

        // 🔹 علاقات إضافية (Custom Business Logic)

        [HttpGet("student/{studentId}")]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<StudentAnswerDetailReadDto>>), StatusCodes.Status200OK)]
        public async Task<ActionResult<ApiResponse<IEnumerable<StudentAnswerDetailReadDto>>>> GetByStudent(int studentId)
        {
            var data = await _service.GetByStudentIdAsync(studentId);
            return Ok(new ApiResponse<IEnumerable<StudentAnswerDetailReadDto>>(200, "Answers by student retrieved successfully", data));
        }

        [HttpGet("question/{questionId}")]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<StudentAnswerDetailReadDto>>), StatusCodes.Status200OK)]
        public async Task<ActionResult<ApiResponse<IEnumerable<StudentAnswerDetailReadDto>>>> GetByQuestion(int questionId)
        {
            var data = await _service.GetByQuestionIdAsync(questionId);
            return Ok(new ApiResponse<IEnumerable<StudentAnswerDetailReadDto>>(200, "Answers by question retrieved successfully", data));
        }

        [HttpGet("recent/{studentId}/{count}")]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<StudentAnswerDetailReadDto>>), StatusCodes.Status200OK)]
        public async Task<ActionResult<ApiResponse<IEnumerable<StudentAnswerDetailReadDto>>>> GetRecent(int studentId, int count)
        {
            var data = await _service.GetRecentAnswersAsync(studentId, count);
            return Ok(new ApiResponse<IEnumerable<StudentAnswerDetailReadDto>>(200, "Recent answers retrieved successfully", data));
        }

        [HttpGet("incorrect/{studentId}/{lessonId}")]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<StudentAnswerDetailReadDto>>), StatusCodes.Status200OK)]
        public async Task<ActionResult<ApiResponse<IEnumerable<StudentAnswerDetailReadDto>>>> GetIncorrectAnswers(int studentId, int lessonId)
        {
            var data = await _service.GetIncorrectAnswersByStudentIdAsync(studentId, lessonId);
            return Ok(new ApiResponse<IEnumerable<StudentAnswerDetailReadDto>>(200, "Incorrect answers retrieved successfully", data));
        }
    }
}