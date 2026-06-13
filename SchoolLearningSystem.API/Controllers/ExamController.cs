using Microsoft.AspNetCore.Mvc;
using SchoolLearningSystem.API.Responses;
using SchoolLearningSystem.Applicationf.DTOs.ExamDto;
using SchoolLearningSystem.Applicationf.DTOs.Lesson;
using SchoolLearningSystem.Applicationf.DTOs.Question;
using SchoolLearningSystem.Applicationf.DTOs.Result;
using SchoolLearningSystem.Applicationf.Interfaces;

namespace SchoolLearningSystem.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ExamController : ControllerBase
    {
        private readonly IExamService _examService;

        public ExamController(IExamService examService)
        {
            _examService = examService;
        }

        // 🔹 CRUD الأساسي

        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<ExamReadDto>>), StatusCodes.Status200OK)]
        public async Task<ActionResult<ApiResponse<IEnumerable<ExamReadDto>>>> GetAll()
        {
            var exams = await _examService.GetAllAsync();
            return Ok(new ApiResponse<IEnumerable<ExamReadDto>>(200, "Exams retrieved successfully", exams));
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ApiResponse<ExamReadDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApiResponse<ExamReadDto>>> GetById(int id)
        {
            var exam = await _examService.GetByIdAsync(id);
            if (exam == null)
                return NotFound(new ApiResponse<string>(404, "Exam not found"));

            return Ok(new ApiResponse<ExamReadDto>(200, "Exam retrieved successfully", exam));
        }

        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<ExamReadDto>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ApiResponse<ExamReadDto>>> Add(ExamCreateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ApiResponse<string>(400, "Invalid input data"));

            var createdExam = await _examService.CreateAsync(dto);
            return StatusCode(201, new ApiResponse<ExamReadDto>(201, "Exam created successfully", createdExam));
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApiResponse<string>>> Update(int id, ExamUpdateDto dto)
        {
            try
            {
                await _examService.UpdateAsync(id, dto);
                return Ok(new ApiResponse<string>(200, "Exam updated successfully"));
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
                await _examService.DeleteAsync(id);
                return Ok(new ApiResponse<string>(200, "Exam deleted successfully"));
            }
            catch (Exception ex)
            {
                return NotFound(new ApiResponse<string>(404, ex.Message));
            }
        }

        // 🔹 علاقات إضافية

        [HttpGet("{id}/questions")]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<QuestionReadDto>>), StatusCodes.Status200OK)]
        public async Task<ActionResult<ApiResponse<IEnumerable<QuestionReadDto>>>> GetQuestionsByExamId(int id)
        {
            var questions = await _examService.GetQuestionsByExamIdAsync(id);
            return Ok(new ApiResponse<IEnumerable<QuestionReadDto>>(200, "Questions retrieved successfully", questions));
        }

        [HttpGet("{id}/results")]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<ResultReadDto>>), StatusCodes.Status200OK)]
        public async Task<ActionResult<ApiResponse<IEnumerable<ResultReadDto>>>> GetResultsByExamId(int id)
        {
            var results = await _examService.GetResultsByExamIdAsync(id);
            return Ok(new ApiResponse<IEnumerable<ResultReadDto>>(200, "Results retrieved successfully", results));
        }

        [HttpGet("{id}/lessons")]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<LessonReadDto>>), StatusCodes.Status200OK)]
        public async Task<ActionResult<ApiResponse<IEnumerable<LessonReadDto>>>> GetLessonsByExamId(int id)
        {
            var lessons = await _examService.GetLessonsByExamIdAsync(id);
            return Ok(new ApiResponse<IEnumerable<LessonReadDto>>(200, "Lessons retrieved successfully", lessons));
        }
    }
}