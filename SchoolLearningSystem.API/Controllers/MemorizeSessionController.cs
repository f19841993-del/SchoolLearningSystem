using Microsoft.AspNetCore.Mvc;
using SchoolLearningSystem.Applicationf.Interfaces;
using SchoolLearningSystem.Applicationf.DTOs.MemorizeSession;
using SchoolLearningSystem.API.Responses;

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

        // 🔹 CRUD الأساسي

        /// <summary>
        /// يرجع كل جلسات المراجعة
        /// </summary>
        /// <response code="200">تم جلب الجلسات بنجاح</response>
        [HttpGet]
        public async Task<IActionResult> GetAllSessions()
        {
            var sessions = await _memorizeService.GetAllSessionsAsync();
            return Ok(new ApiResponse<IEnumerable<MemorizeSessionReadDto>>(200, "Sessions retrieved successfully", sessions));
        }

        /// <summary>
        /// يرجع جلسة مراجعة حسب الـ Id
        /// </summary>
        /// <response code="200">تم جلب الجلسة بنجاح</response>
        /// <response code="404">الجلسة غير موجودة</response>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetSessionById(int id)
        {
            var session = await _memorizeService.GetSessionByIdAsync(id);
            if (session == null)
                return NotFound(new ApiResponse<string>(404, "Session not found"));

            return Ok(new ApiResponse<MemorizeSessionReadDto>(200, "Session retrieved successfully", session));
        }

        /// <summary>
        /// إضافة جلسة مراجعة جديدة
        /// </summary>
        /// <response code="201">تم إنشاء الجلسة بنجاح</response>
        /// <response code="400">خطأ في البيانات المدخلة</response>
        [HttpPost]
        public async Task<IActionResult> AddSession(MemorizeSessionCreateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ApiResponse<string>(400, "Invalid input data"));

            await _memorizeService.AddSessionAsync(dto);
            return StatusCode(201, new ApiResponse<string>(201, "Session created successfully"));
        }

        /// <summary>
        /// تحديث جلسة مراجعة موجودة
        /// </summary>
        /// <response code="200">تم تحديث الجلسة بنجاح</response>
        /// <response code="404">الجلسة غير موجودة</response>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSession(int id, MemorizeSessionUpdateDto dto)
        {
            try
            {
                await _memorizeService.UpdateSessionAsync(id, dto);
                return Ok(new ApiResponse<string>(200, "Session updated successfully"));
            }
            catch (Exception ex)
            {
                return NotFound(new ApiResponse<string>(404, ex.Message));
            }
        }

        /// <summary>
        /// حذف جلسة مراجعة
        /// </summary>
        /// <response code="200">تم حذف الجلسة بنجاح</response>
        /// <response code="404">الجلسة غير موجودة</response>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSession(int id)
        {
            try
            {
                await _memorizeService.DeleteSessionAsync(id);
                return Ok(new ApiResponse<string>(200, "Session deleted successfully"));
            }
            catch (Exception ex)
            {
                return NotFound(new ApiResponse<string>(404, ex.Message));
            }
        }

        // 🔹 علاقات إضافية

        /// <summary>
        /// يرجع جلسات المراجعة لطالب معين
        /// </summary>
        /// <response code="200">تم جلب الجلسات بنجاح</response>
        [HttpGet("student/{studentId}")]
        public async Task<IActionResult> GetSessionsByStudentId(int studentId)
        {
            var sessions = await _memorizeService.GetSessionsByStudentIdAsync(studentId);
            return Ok(new ApiResponse<IEnumerable<MemorizeSessionReadDto>>(200, "Sessions retrieved successfully", sessions));
        }

        /// <summary>
        /// يرجع جلسات المراجعة لدرس معين
        /// </summary>
        /// <response code="200">تم جلب الجلسات بنجاح</response>
        [HttpGet("lesson/{lessonId}")]
        public async Task<IActionResult> GetSessionsByLessonId(int lessonId)
        {
            var sessions = await _memorizeService.GetSessionsByLessonIdAsync(lessonId);
            return Ok(new ApiResponse<IEnumerable<MemorizeSessionReadDto>>(200, "Sessions retrieved successfully", sessions));
        }

        /// <summary>
        /// يرجع جلسات المراجعة لتدريب معين
        /// </summary>
        /// <response code="200">تم جلب الجلسات بنجاح</response>
        [HttpGet("exercise/{exerciseId}")]
        public async Task<IActionResult> GetSessionsByExerciseId(int exerciseId)
        {
            var sessions = await _memorizeService.GetSessionsByExerciseIdAsync(exerciseId);
            return Ok(new ApiResponse<IEnumerable<MemorizeSessionReadDto>>(200, "Sessions retrieved successfully", sessions));
        }

        // 🔹 دوال إضافية ترجع أسماء مرتبطة بالجلسة

        /// <summary>
        /// يرجع اسم الطالب المرتبط بالجلسة
        /// </summary>
        /// <response code="200">تم جلب الاسم بنجاح</response>
        /// <response code="404">الجلسة غير موجودة</response>
        [HttpGet("{id}/student-name")]
        public async Task<IActionResult> GetStudentNameBySessionId(int id)
        {
            var name = await _memorizeService.GetStudentNameBySessionIdAsync(id);
            return Ok(new ApiResponse<string>(200, "Student name retrieved successfully", name));
        }

        /// <summary>
        /// يرجع عنوان الدرس المرتبط بالجلسة
        /// </summary>
        /// <response code="200">تم جلب العنوان بنجاح</response>
        /// <response code="404">الجلسة غير موجودة</response>
        [HttpGet("{id}/lesson-title")]
        public async Task<IActionResult> GetLessonTitleBySessionId(int id)
        {
            var title = await _memorizeService.GetLessonTitleBySessionIdAsync(id);
            return Ok(new ApiResponse<string>(200, "Lesson title retrieved successfully", title));
        }

        /// <summary>
        /// يرجع سؤال التدريب المرتبط بالجلسة
        /// </summary>
        /// <response code="200">تم جلب السؤال بنجاح</response>
        /// <response code="404">الجلسة غير موجودة</response>
        [HttpGet("{id}/exercise-question")]
        public async Task<IActionResult> GetExerciseQuestionBySessionId(int id)
        {
            var question = await _memorizeService.GetExerciseQuestionBySessionIdAsync(id);
            return Ok(new ApiResponse<string>(200, "Exercise question retrieved successfully", question));
        }
    }
}
