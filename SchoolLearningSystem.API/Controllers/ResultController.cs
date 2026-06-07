using Microsoft.AspNetCore.Mvc;
using SchoolLearningSystem.Applicationf.Interfaces;
using SchoolLearningSystem.Applicationf.DTOs.Result;
using SchoolLearningSystem.API.Responses;

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

        /// <summary>
        /// يرجع كل النتائج الموجودة بالنظام
        /// </summary>
        /// <response code="200">تم جلب النتائج بنجاح</response>
        [HttpGet]
        public async Task<IActionResult> GetAllResults()
        {
            var results = await _resultService.GetAllResultsAsync();
            return Ok(new ApiResponse<IEnumerable<ResultReadDto>>(200, "Results retrieved successfully", results));
        }

        /// <summary>
        /// يرجع بيانات نتيجة محددة حسب الـ Id
        /// </summary>
        /// <response code="200">تم جلب النتيجة بنجاح</response>
        /// <response code="404">النتيجة غير موجودة</response>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetResultById(int id)
        {
            var result = await _resultService.GetResultByIdAsync(id);
            if (result == null)
                return NotFound(new ApiResponse<string>(404, "Result not found"));

            return Ok(new ApiResponse<ResultReadDto>(200, "Result retrieved successfully", result));
        }

        /// <summary>
        /// إضافة نتيجة جديدة
        /// </summary>
        /// <response code="201">تم إنشاء النتيجة بنجاح</response>
        /// <response code="400">خطأ في البيانات المدخلة</response>
        [HttpPost]
        public async Task<IActionResult> AddResult(ResultCreateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ApiResponse<string>(400, "Invalid input data"));

            await _resultService.AddResultAsync(dto);
            return StatusCode(201, new ApiResponse<string>(201, "Result created successfully"));
        }

        /// <summary>
        /// تحديث نتيجة موجودة
        /// </summary>
        /// <response code="200">تم تحديث النتيجة بنجاح</response>
        /// <response code="404">النتيجة غير موجودة</response>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateResult(int id, ResultUpdateDto dto)
        {
            try
            {
                await _resultService.UpdateResultAsync(id, dto);
                return Ok(new ApiResponse<string>(200, "Result updated successfully"));
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new ApiResponse<string>(404, "Result not found"));
            }
        }

        /// <summary>
        /// حذف نتيجة
        /// </summary>
        /// <response code="200">تم حذف النتيجة بنجاح</response>
        /// <response code="404">النتيجة غير موجودة</response>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteResult(int id)
        {
            try
            {
                await _resultService.DeleteResultAsync(id);
                return Ok(new ApiResponse<string>(200, "Result deleted successfully"));
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new ApiResponse<string>(404, "Result not found"));
            }
        }

        // 🔹 علاقات إضافية

        /// <summary>
        /// يرجع نتائج الطلاب المرتبطة بامتحان معين
        /// </summary>
        [HttpGet("exam/{examId}")]
        public async Task<IActionResult> GetResultsByExamId(int examId)
        {
            var results = await _resultService.GetResultsByExamIdAsync(examId);
            return Ok(new ApiResponse<IEnumerable<ResultReadDto>>(200, "Results retrieved successfully", results));
        }

        /// <summary>
        /// يرجع نتائج طالب معين
        /// </summary>
        [HttpGet("student/{studentId}")]
        public async Task<IActionResult> GetResultsByStudentId(int studentId)
        {
            var results = await _resultService.GetResultsByStudentIdAsync(studentId);
            return Ok(new ApiResponse<IEnumerable<ResultReadDto>>(200, "Results retrieved successfully", results));
        }

        /// <summary>
        /// يرجع نتائج مرتبطة بدرس معين
        /// </summary>
        [HttpGet("lesson/{lessonId}")]
        public async Task<IActionResult> GetResultsByLessonId(int lessonId)
        {
            var results = await _resultService.GetResultsByLessonIdAsync(lessonId);
            return Ok(new ApiResponse<IEnumerable<ResultReadDto>>(200, "Results retrieved successfully", results));
        }

        // 🔹 إحصائيات إضافية

        /// <summary>
        /// يرجع معدل درجات طالب معين
        /// </summary>
        [HttpGet("student/{studentId}/average")]
        public async Task<IActionResult> GetAverageScoreByStudentId(int studentId)
        {
            var avg = await _resultService.GetAverageScoreByStudentIdAsync(studentId);
            return Ok(new ApiResponse<double>(200, "Average score retrieved successfully", avg));
        }

        /// <summary>
        /// يرجع معدل درجات درس معين
        /// </summary>
        [HttpGet("lesson/{lessonId}/average")]
        public async Task<IActionResult> GetAverageScoreByLessonId(int lessonId)
        {
            var avg = await _resultService.GetAverageScoreByLessonIdAsync(lessonId);
            return Ok(new ApiResponse<double>(200, "Average score retrieved successfully", avg));
        }

        /// <summary>
        /// يرجع معدل درجات امتحان معين
        /// </summary>
        [HttpGet("exam/{examId}/average")]
        public async Task<IActionResult> GetAverageScoreByExamId(int examId)
        {
            var avg = await _resultService.GetAverageScoreByExamIdAsync(examId);
            return Ok(new ApiResponse<double>(200, "Average score retrieved successfully", avg));
        }
    }
}
