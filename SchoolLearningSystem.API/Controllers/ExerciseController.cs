using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolLearningSystem.API.Responses;
using SchoolLearningSystem.Applicationf.DTOs.ExerciseDto;
using SchoolLearningSystem.Applicationf.Interfaces;
using SchoolLearningSystem.Domain.Enums;

namespace SchoolLearningSystem.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    // ⚠️ ExerciseReadDto يحتوي حقل Answer مباشرة بكل استجابة - الكونترولر كامل مقيّد
    // لـ Admin/Teacher فقط، ولا حتى الطالب المسجّل يوصله (يكشف إجابات التمارين)
    [Authorize(Roles = "Admin,Teacher")]
    public class ExerciseController : ControllerBase
    {
        private readonly IExerciseService _exerciseService;

        public ExerciseController(IExerciseService exerciseService)
        {
            _exerciseService = exerciseService;
        }

        // 🔹 CRUD الأساسي

        /// <summary>كل التمارين (بالإجابات الكاملة)</summary>
        /// <remarks>الصلاحيات: Admin, Teacher فقط - يكشف حقل Answer، لا يُفتح للطالب ولا للعامة.</remarks>
        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<ExerciseReadDto>>), StatusCodes.Status200OK)]
        public async Task<ActionResult<ApiResponse<IEnumerable<ExerciseReadDto>>>> GetAll()
        {
            var data = await _exerciseService.GetAllAsync();
            return Ok(new ApiResponse<IEnumerable<ExerciseReadDto>>(200, "Exercises retrieved successfully", data));
        }

        /// <summary>تمرين واحد بالتفصيل (بالإجابة الكاملة)</summary>
        /// <remarks>الصلاحيات: Admin, Teacher فقط.</remarks>
        /// <response code="404">التمرين غير موجود</response>
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

        /// <summary>إنشاء تمرين جديد</summary>
        /// <remarks>
        /// الصلاحيات: Admin, Teacher فقط.
        ///
        /// مثال Request:
        /// {
        ///   "question": "كم ناتج 2 + 2؟",
        ///   "answer": "4",
        ///   "lessonId": 1,
        ///   "difficulty": 1
        /// }
        /// </remarks>
        /// <response code="400">بيانات غير صالحة</response>
        /// <response code="401">لم يتم تسجيل الدخول</response>
        /// <response code="403">الدور الحالي غير مسموح له</response>
        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<ExerciseReadDto>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<ApiResponse<ExerciseReadDto>>> Add([FromBody] ExerciseCreateDto dto)
        {
            var createdExercise = await _exerciseService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = createdExercise.Id },
                new ApiResponse<ExerciseReadDto>(201, "Exercise created successfully", createdExercise));
        }

        /// <summary>تعديل تمرين موجود</summary>
        /// <remarks>
        /// الصلاحيات: Admin, Teacher فقط. كل الحقول اختيارية.
        ///
        /// مثال Request:
        /// { "answer": "4 (محدّث)" }
        /// </remarks>
        /// <response code="404">التمرين غير موجود</response>
        /// <response code="401">لم يتم تسجيل الدخول</response>
        /// <response code="403">الدور الحالي غير مسموح له</response>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<ApiResponse>> Update(int id, [FromBody] ExerciseUpdateDto dto)
        {
            await _exerciseService.UpdateAsync(id, dto);
            return Ok(new ApiResponse(200, "Exercise updated successfully"));
        }

        /// <summary>حذف تمرين (Soft Delete)</summary>
        /// <remarks>الصلاحيات: Admin, Teacher فقط.</remarks>
        /// <response code="404">التمرين غير موجود</response>
        /// <response code="401">لم يتم تسجيل الدخول</response>
        /// <response code="403">الدور الحالي غير مسموح له</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
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

        /// <summary>تمارين درس معيّن</summary>
        /// <remarks>الصلاحيات: Admin, Teacher فقط.</remarks>
        [HttpGet("lesson/{lessonId}")]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<ExerciseReadDto>>), StatusCodes.Status200OK)]
        public async Task<ActionResult<ApiResponse<IEnumerable<ExerciseReadDto>>>> GetExercisesByLessonId(int lessonId)
        {
            var exercises = await _exerciseService.GetExercisesByLessonIdAsync(lessonId);
            return Ok(new ApiResponse<IEnumerable<ExerciseReadDto>>(200, "Exercises retrieved successfully", exercises));
        }

        /// <summary>تمارين حسب مستوى صعوبة معيّن — لبناء مسار تعليمي تصاعدي (سهل ← متوسط ← صعب)</summary>
        /// <remarks>الصلاحيات: Admin, Teacher فقط.</remarks>
        [HttpGet("difficulty/{difficulty}")]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<ExerciseReadDto>>), StatusCodes.Status200OK)]
        public async Task<ActionResult<ApiResponse<IEnumerable<ExerciseReadDto>>>> GetByDifficulty(DifficultyLevel difficulty)
        {
            var data = await _exerciseService.GetExercisesByDifficultyAsync(difficulty);
            return Ok(new ApiResponse<IEnumerable<ExerciseReadDto>>(200, "Exercises retrieved successfully", data));
        }
    }
}
