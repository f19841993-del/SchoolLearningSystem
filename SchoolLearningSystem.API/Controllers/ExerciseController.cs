using Microsoft.AspNetCore.Mvc;
using SchoolLearningSystem.API.Responses;
using SchoolLearningSystem.Applicationf.DTOs.ExerciseDto;
using SchoolLearningSystem.Applicationf.DTOs.Lesson;
using SchoolLearningSystem.Applicationf.DTOs.MemorizeSession;
using SchoolLearningSystem.Applicationf.Interfaces;

namespace SchoolLearningSystem.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ExerciseController : ControllerBase
    {
        private readonly IExerciseService _exerciseService;

        public ExerciseController(IExerciseService exerciseService)
        {
            _exerciseService = exerciseService;
        }

        // 🔹 CRUD الأساسي (موروث من BaseService)

        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<ExerciseReadDto>>), StatusCodes.Status200OK)]
        public async Task<ActionResult<ApiResponse<IEnumerable<ExerciseReadDto>>>> GetAll()
        {
            var data = await _exerciseService.GetAllAsync();
            return Ok(new ApiResponse<IEnumerable<ExerciseReadDto>>(200, "Exercises retrieved successfully", data));
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ApiResponse<ExerciseReadDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApiResponse<ExerciseReadDto>>> GetById(int id)
        {
            var data = await _exerciseService.GetByIdAsync(id);
            if (data == null)
                return NotFound(new ApiResponse<string>(404, "Exercise not found"));

            return Ok(new ApiResponse<ExerciseReadDto>(200, "Exercise retrieved successfully", data));
        }

        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<ExerciseReadDto>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ApiResponse<ExerciseReadDto>>> Add(ExerciseCreateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ApiResponse<string>(400, "Invalid input data"));

            var createdExercise = await _exerciseService.CreateAsync(dto);
            return StatusCode(201, new ApiResponse<ExerciseReadDto>(201, "Exercise created successfully", createdExercise));
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApiResponse<string>>> Update(int id, ExerciseUpdateDto dto)
        {
            try
            {
                await _exerciseService.UpdateAsync(id, dto);
                return Ok(new ApiResponse<string>(200, "Exercise updated successfully"));
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
                await _exerciseService.DeleteAsync(id);
                return Ok(new ApiResponse<string>(200, "Exercise deleted successfully"));
            }
            catch (Exception ex)
            {
                return NotFound(new ApiResponse<string>(404, ex.Message));
            }
        }

        // 🔹 علاقات إضافية (Custom Logic)

        [HttpGet("lesson/{lessonId}")]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<ExerciseReadDto>>), StatusCodes.Status200OK)]
        public async Task<ActionResult<ApiResponse<IEnumerable<ExerciseReadDto>>>> GetExercisesByLessonId(int lessonId)
        {
            var exercises = await _exerciseService.GetExercisesByLessonIdAsync(lessonId);
            return Ok(new ApiResponse<IEnumerable<ExerciseReadDto>>(200, "Exercises retrieved successfully", exercises));
        }

        [HttpGet("{id}/sessions")]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<MemorizeSessionReadDto>>), StatusCodes.Status200OK)]
        public async Task<ActionResult<ApiResponse<IEnumerable<MemorizeSessionReadDto>>>> GetSessionsByExerciseId(int id)
        {
            var sessions = await _exerciseService.GetMemorizeSessionsByExerciseIdAsync(id);
            return Ok(new ApiResponse<IEnumerable<MemorizeSessionReadDto>>(200, "Sessions retrieved successfully", sessions));
        }

        [HttpGet("{id}/lesson")]
        [ProducesResponseType(typeof(ApiResponse<LessonReadDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<ApiResponse<LessonReadDto>>> GetLessonByExerciseId(int id)
        {
            var lesson = await _exerciseService.GetLessonByExerciseIdAsync(id);
            return Ok(new ApiResponse<LessonReadDto>(200, "Lesson retrieved successfully", lesson));
        }
    }
}