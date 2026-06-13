using Microsoft.AspNetCore.Mvc;
using SchoolLearningSystem.API.Responses;
using SchoolLearningSystem.Applicationf.DTOs.ExamDto;
using SchoolLearningSystem.Applicationf.DTOs.ExerciseDto;
using SchoolLearningSystem.Applicationf.DTOs.Lesson;
using SchoolLearningSystem.Applicationf.DTOs.MemorizeSession;
using SchoolLearningSystem.Applicationf.DTOs.Question;
using SchoolLearningSystem.Applicationf.DTOs.Result;
using SchoolLearningSystem.Applicationf.Interfaces;

namespace SchoolLearningSystem.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LessonController : ControllerBase
    {
        private readonly ILessonService _lessonService;

        public LessonController(ILessonService lessonService)
        {
            _lessonService = lessonService;
        }

        // 🔹 CRUD الأساسي (موروث من BaseService)

        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<LessonReadDto>>), StatusCodes.Status200OK)]
        public async Task<ActionResult<ApiResponse<IEnumerable<LessonReadDto>>>> GetAll()
        {
            var data = await _lessonService.GetAllAsync();
            return Ok(new ApiResponse<IEnumerable<LessonReadDto>>(200, "Lessons retrieved successfully", data));
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ApiResponse<LessonReadDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApiResponse<LessonReadDto>>> GetById(int id)
        {
            var data = await _lessonService.GetByIdAsync(id);
            if (data == null)
                return NotFound(new ApiResponse<string>(404, "Lesson not found"));

            return Ok(new ApiResponse<LessonReadDto>(200, "Lesson retrieved successfully", data));
        }

        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<LessonReadDto>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ApiResponse<LessonReadDto>>> Add(LessonCreateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ApiResponse<string>(400, "Invalid input data"));

            var createdLesson = await _lessonService.CreateAsync(dto);
            return StatusCode(201, new ApiResponse<LessonReadDto>(201, "Lesson created successfully", createdLesson));
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApiResponse<string>>> Update(int id, LessonUpdateDto dto)
        {
            try
            {
                await _lessonService.UpdateAsync(id, dto);
                return Ok(new ApiResponse<string>(200, "Lesson updated successfully"));
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
                await _lessonService.DeleteAsync(id);
                return Ok(new ApiResponse<string>(200, "Lesson deleted successfully"));
            }
            catch (Exception ex)
            {
                return NotFound(new ApiResponse<string>(404, ex.Message));
            }
        }

        // 🔹 علاقات إضافية (Custom Business Logic)

        [HttpGet("{id}/exams")]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<ExamReadDto>>), StatusCodes.Status200OK)]
        public async Task<ActionResult<ApiResponse<IEnumerable<ExamReadDto>>>> GetExams(int id)
        {
            var data = await _lessonService.GetExamsByLessonIdAsync(id);
            return Ok(new ApiResponse<IEnumerable<ExamReadDto>>(200, "Exams retrieved successfully", data));
        }

        [HttpGet("{id}/exercises")]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<ExerciseReadDto>>), StatusCodes.Status200OK)]
        public async Task<ActionResult<ApiResponse<IEnumerable<ExerciseReadDto>>>> GetExercises(int id)
        {
            var data = await _lessonService.GetExercisesByLessonIdAsync(id);
            return Ok(new ApiResponse<IEnumerable<ExerciseReadDto>>(200, "Exercises retrieved successfully", data));
        }

        [HttpGet("{id}/results")]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<ResultReadDto>>), StatusCodes.Status200OK)]
        public async Task<ActionResult<ApiResponse<IEnumerable<ResultReadDto>>>> GetResults(int id)
        {
            var data = await _lessonService.GetResultsByLessonIdAsync(id);
            return Ok(new ApiResponse<IEnumerable<ResultReadDto>>(200, "Results retrieved successfully", data));
        }

        [HttpGet("{id}/sessions")]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<MemorizeSessionReadDto>>), StatusCodes.Status200OK)]
        public async Task<ActionResult<ApiResponse<IEnumerable<MemorizeSessionReadDto>>>> GetSessions(int id)
        {
            var data = await _lessonService.GetMemorizeSessionsByLessonIdAsync(id);
            return Ok(new ApiResponse<IEnumerable<MemorizeSessionReadDto>>(200, "Sessions retrieved successfully", data));
        }

        [HttpGet("{id}/questions")]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<QuestionReadDto>>), StatusCodes.Status200OK)]
        public async Task<ActionResult<ApiResponse<IEnumerable<QuestionReadDto>>>> GetQuestions(int id)
        {
            var data = await _lessonService.GetQuestionsByLessonIdAsync(id);
            return Ok(new ApiResponse<IEnumerable<QuestionReadDto>>(200, "Questions retrieved successfully", data));
        }

        // 🔹 إحصائيات

        [HttpGet("{id}/total-questions")]
        [ProducesResponseType(typeof(ApiResponse<int>), StatusCodes.Status200OK)]
        public async Task<ActionResult<ApiResponse<int>>> GetTotalQuestions(int id)
        {
            var count = await _lessonService.GetTotalQuestionsByLessonIdAsync(id);
            return Ok(new ApiResponse<int>(200, "Total questions count retrieved", count));
        }

        [HttpGet("{id}/total-exams")]
        [ProducesResponseType(typeof(ApiResponse<int>), StatusCodes.Status200OK)]
        public async Task<ActionResult<ApiResponse<int>>> GetTotalExams(int id)
        {
            var count = await _lessonService.GetTotalExamsByLessonIdAsync(id);
            return Ok(new ApiResponse<int>(200, "Total exams count retrieved", count));
        }
    }
}