using Microsoft.AspNetCore.Mvc;
using SchoolLearningSystem.API.Responses;
using SchoolLearningSystem.Applicationf.DTOs.MemorizeSession;
using SchoolLearningSystem.Applicationf.Interfaces;

namespace SchoolLearningSystem.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MemorizeSessionController : ControllerBase
    {
        private readonly IMemorizeService _memorizeService;

        public MemorizeSessionController(IMemorizeService memorizeService)
        {
            _memorizeService = memorizeService;
        }

        // 🔹 CRUD الأساسي (موروث من BaseService)

        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<MemorizeSessionReadDto>>), StatusCodes.Status200OK)]
        public async Task<ActionResult<ApiResponse<IEnumerable<MemorizeSessionReadDto>>>> GetAll()
        {
            var data = await _memorizeService.GetAllAsync();
            return Ok(new ApiResponse<IEnumerable<MemorizeSessionReadDto>>(200, "Sessions retrieved successfully", data));
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ApiResponse<MemorizeSessionReadDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApiResponse<MemorizeSessionReadDto>>> GetById(int id)
        {
            var data = await _memorizeService.GetByIdAsync(id);
            if (data == null)
                return NotFound(new ApiResponse<string>(404, "Session not found"));

            return Ok(new ApiResponse<MemorizeSessionReadDto>(200, "Session retrieved successfully", data));
        }

        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<MemorizeSessionReadDto>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ApiResponse<MemorizeSessionReadDto>>> Add(MemorizeSessionCreateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ApiResponse<string>(400, "Invalid input data"));

            var createdSession = await _memorizeService.CreateAsync(dto);
            return StatusCode(201, new ApiResponse<MemorizeSessionReadDto>(201, "Session created successfully", createdSession));
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApiResponse<string>>> Update(int id, MemorizeSessionUpdateDto dto)
        {
            try
            {
                await _memorizeService.UpdateAsync(id, dto);
                return Ok(new ApiResponse<string>(200, "Session updated successfully"));
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
                await _memorizeService.DeleteAsync(id);
                return Ok(new ApiResponse<string>(200, "Session deleted successfully"));
            }
            catch (Exception ex)
            {
                return NotFound(new ApiResponse<string>(404, ex.Message));
            }
        }

        // 🔹 علاقات إضافية (Custom Queries)

        [HttpGet("student/{studentId}")]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<MemorizeSessionReadDto>>), StatusCodes.Status200OK)]
        public async Task<ActionResult<ApiResponse<IEnumerable<MemorizeSessionReadDto>>>> GetByStudent(int studentId)
        {
            var data = await _memorizeService.GetSessionsByStudentIdAsync(studentId);
            return Ok(new ApiResponse<IEnumerable<MemorizeSessionReadDto>>(200, "Sessions retrieved successfully", data));
        }

        [HttpGet("lesson/{lessonId}")]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<MemorizeSessionReadDto>>), StatusCodes.Status200OK)]
        public async Task<ActionResult<ApiResponse<IEnumerable<MemorizeSessionReadDto>>>> GetByLesson(int lessonId)
        {
            var data = await _memorizeService.GetSessionsByLessonIdAsync(lessonId);
            return Ok(new ApiResponse<IEnumerable<MemorizeSessionReadDto>>(200, "Sessions retrieved successfully", data));
        }

        [HttpGet("exercise/{exerciseId}")]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<MemorizeSessionReadDto>>), StatusCodes.Status200OK)]
        public async Task<ActionResult<ApiResponse<IEnumerable<MemorizeSessionReadDto>>>> GetByExercise(int exerciseId)
        {
            var data = await _memorizeService.GetSessionsByExerciseIdAsync(exerciseId);
            return Ok(new ApiResponse<IEnumerable<MemorizeSessionReadDto>>(200, "Sessions retrieved successfully", data));
        }

        // 🔹 استعلامات معلومات الجلسة

        [HttpGet("{id}/student-name")]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status200OK)]
        public async Task<ActionResult<ApiResponse<string>>> GetStudentName(int id)
        {
            var name = await _memorizeService.GetStudentNameBySessionIdAsync(id);
            return Ok(new ApiResponse<string>(200, "Student name retrieved", name));
        }

        [HttpGet("{id}/lesson-title")]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status200OK)]
        public async Task<ActionResult<ApiResponse<string>>> GetLessonTitle(int id)
        {
            var title = await _memorizeService.GetLessonTitleBySessionIdAsync(id);
            return Ok(new ApiResponse<string>(200, "Lesson title retrieved", title));
        }

        [HttpGet("{id}/exercise-question")]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status200OK)]
        public async Task<ActionResult<ApiResponse<string>>> GetExerciseQuestion(int id)
        {
            var question = await _memorizeService.GetExerciseQuestionBySessionIdAsync(id);
            return Ok(new ApiResponse<string>(200, "Question retrieved", question));
        }
    }
}