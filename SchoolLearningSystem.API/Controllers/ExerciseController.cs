using Microsoft.AspNetCore.Mvc;
using SchoolLearningSystem.API.Responses;
using SchoolLearningSystem.Applicationf.DTOs.ExerciseDto;
using SchoolLearningSystem.Applicationf.Interfaces;
using SchoolLearningSystem.Domain.Enums;

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

        // 🔹 CRUD الأساسي

        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<ExerciseReadDto>>), StatusCodes.Status200OK)]
        public async Task<ActionResult<ApiResponse<IEnumerable<ExerciseReadDto>>>> GetAll()
        {
            var data = await _exerciseService.GetAllAsync();
            return Ok(new ApiResponse<IEnumerable<ExerciseReadDto>>(200, "Exercises retrieved successfully", data));
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ApiResponse<ExerciseReadDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApiResponse<ExerciseReadDto>>> GetById(int id)
        {
            var data = await _exerciseService.GetByIdAsync(id);
            if (data == null)
                return NotFound(new ApiResponse(404, "Exercise not found"));

            return Ok(new ApiResponse<ExerciseReadDto>(200, "Exercise retrieved successfully", data));
        }

        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<ExerciseReadDto>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ApiResponse<ExerciseReadDto>>> Add([FromBody] ExerciseCreateDto dto)
        {
            var createdExercise = await _exerciseService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = createdExercise.Id },
                new ApiResponse<ExerciseReadDto>(201, "Exercise created successfully", createdExercise));
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApiResponse>> Update(int id, [FromBody] ExerciseUpdateDto dto)
        {
            await _exerciseService.UpdateAsync(id, dto);
            return Ok(new ApiResponse(200, "Exercise updated successfully"));
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApiResponse>> Delete(int id)
        {
            await _exerciseService.DeleteAsync(id);
            return Ok(new ApiResponse(200, "Exercise deleted successfully"));
        }

        // 🔹 علاقات إضافية — مطابقة حرفياً لـ IExerciseService
        // ⚠️ حُذفت: GetSessionsByExerciseId / GetLessonByExerciseId — كانتا تنادي دوال
        // (GetMemorizeSessionsByExerciseIdAsync, GetLessonByExerciseIdAsync) غير موجودة
        // إطلاقاً بـ IExerciseService. "الدرس المرتبط بتمرين" موجود فعلاً وبمكانه الصحيح
        // بـ LessonController.GetByExerciseId (عبر ILessonService.GetLessonByExerciseIdAsync).

        [HttpGet("lesson/{lessonId}")]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<ExerciseReadDto>>), StatusCodes.Status200OK)]
        public async Task<ActionResult<ApiResponse<IEnumerable<ExerciseReadDto>>>> GetExercisesByLessonId(int lessonId)
        {
            var exercises = await _exerciseService.GetExercisesByLessonIdAsync(lessonId);
            return Ok(new ApiResponse<IEnumerable<ExerciseReadDto>>(200, "Exercises retrieved successfully", exercises));
        }

        /// <summary>
        /// تمارين حسب مستوى صعوبة معيّن — لبناء مسار تعليمي تصاعدي (سهل ← متوسط ← صعب)
        /// </summary>
        [HttpGet("difficulty/{difficulty}")]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<ExerciseReadDto>>), StatusCodes.Status200OK)]
        public async Task<ActionResult<ApiResponse<IEnumerable<ExerciseReadDto>>>> GetByDifficulty(DifficultyLevel difficulty)
        {
            var data = await _exerciseService.GetExercisesByDifficultyAsync(difficulty);
            return Ok(new ApiResponse<IEnumerable<ExerciseReadDto>>(200, "Exercises retrieved successfully", data));
        }
    }
}
