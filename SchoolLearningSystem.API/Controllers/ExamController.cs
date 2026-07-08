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
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApiResponse<ExamReadDto>>> GetById(int id)
        {
            var exam = await _examService.GetByIdAsync(id);
            if (exam == null)
                return NotFound(new ApiResponse(404, "Exam not found"));

            return Ok(new ApiResponse<ExamReadDto>(200, "Exam retrieved successfully", exam));
        }

        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<ExamReadDto>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ApiResponse<ExamReadDto>>> Add([FromBody] ExamCreateDto dto)
        {
            var createdExam = await _examService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = createdExam.Id },
                new ApiResponse<ExamReadDto>(201, "Exam created successfully", createdExam));
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApiResponse>> Update(int id, [FromBody] ExamUpdateDto dto)
        {
            await _examService.UpdateAsync(id, dto);
            return Ok(new ApiResponse(200, "Exam updated successfully"));
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApiResponse>> Delete(int id)
        {
            // ملاحظة معمارية موثقة: حذف الامتحان لا يحذف أسئلته (ExamId تصير null لها)
            await _examService.DeleteAsync(id);
            return Ok(new ApiResponse(200, "Exam deleted successfully"));
        }

        // 🔹 علاقات إضافية — مطابقة حرفياً لـ IExamService

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

        /// <summary>
        /// الدرس المرتبط بامتحان معيّن (Nullable — الامتحان قد يكون عاماً/شاملاً للكورس)
        /// </summary>
        [HttpGet("{id}/lesson")]
        [ProducesResponseType(typeof(ApiResponse<LessonReadDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApiResponse<LessonReadDto>>> GetLessonByExamId(int id)
        {
            var lesson = await _examService.GetLessonByExamIdAsync(id);
            if (lesson == null)
                return NotFound(new ApiResponse(404, "This exam is not linked to a specific lesson"));

            return Ok(new ApiResponse<LessonReadDto>(200, "Lesson retrieved successfully", lesson));
        }

        /// <summary>
        /// كل امتحانات درس معيّن
        /// </summary>
        [HttpGet("lesson/{lessonId}")]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<ExamReadDto>>), StatusCodes.Status200OK)]
        public async Task<ActionResult<ApiResponse<IEnumerable<ExamReadDto>>>> GetExamsByLessonId(int lessonId)
        {
            var exams = await _examService.GetExamsByLessonIdAsync(lessonId);
            return Ok(new ApiResponse<IEnumerable<ExamReadDto>>(200, "Exams retrieved successfully", exams));
        }

        [HttpGet("lesson/{lessonId}/count")]
        [ProducesResponseType(typeof(ApiResponse<int>), StatusCodes.Status200OK)]
        public async Task<ActionResult<ApiResponse<int>>> GetTotalExamsByLessonId(int lessonId)
        {
            var count = await _examService.GetTotalExamsByLessonIdAsync(lessonId);
            return Ok(new ApiResponse<int>(200, "Total exams count retrieved successfully", count));
        }
    }
}
