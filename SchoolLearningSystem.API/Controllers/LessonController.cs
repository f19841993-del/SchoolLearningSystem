using Microsoft.AspNetCore.Mvc;
using SchoolLearningSystem.API.Responses;
using SchoolLearningSystem.Applicationf.DTOs.ExamDto;
using SchoolLearningSystem.Applicationf.DTOs.Exercise;
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

        // 🔹 CRUD الأساسي

        /// <summary>
        /// يرجع كل الدروس الموجودة بالنظام
        /// </summary>
        /// <response code="200">تم جلب الدروس بنجاح</response>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var lessons = await _lessonService.GetAllLessonsAsync();
            return Ok(new ApiResponse<IEnumerable<LessonReadDto>>(200, "Lessons retrieved successfully", lessons));
        }

        /// <summary>
        /// يرجع درس محدد حسب الـ Id
        /// </summary>
        /// <response code="200">تم جلب الدرس بنجاح</response>
        /// <response code="404">الدرس غير موجود</response>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var lesson = await _lessonService.GetLessonByIdAsync(id);
            if (lesson == null)
                return NotFound(new ApiResponse<string>(404, "Lesson not found"));

            return Ok(new ApiResponse<LessonReadDto>(200, "Lesson retrieved successfully", lesson));
        }

        /// <summary>
        /// إضافة درس جديد
        /// </summary>
        /// <response code="201">تم إنشاء الدرس بنجاح</response>
        /// <response code="400">خطأ في البيانات المدخلة</response>
        [HttpPost]
        public async Task<IActionResult> Add(LessonCreateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ApiResponse<string>(400, "Invalid input data"));

            await _lessonService.AddLessonAsync(dto);
            return StatusCode(201, new ApiResponse<string>(201, "Lesson created successfully"));
        }

        /// <summary>
        /// تحديث بيانات درس موجود
        /// </summary>
        /// <response code="200">تم تحديث الدرس بنجاح</response>
        /// <response code="404">الدرس غير موجود</response>
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, LessonUpdateDto dto)
        {
            try
            {
                await _lessonService.UpdateLessonAsync(id, dto);
                return Ok(new ApiResponse<string>(200, "Lesson updated successfully"));
            }
            catch (Exception ex)
            {
                return NotFound(new ApiResponse<string>(404, ex.Message));
            }
        }

        /// <summary>
        /// حذف درس
        /// </summary>
        /// <response code="200">تم حذف الدرس بنجاح</response>
        /// <response code="404">الدرس غير موجود</response>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _lessonService.DeleteLessonAsync(id);
                return Ok(new ApiResponse<string>(200, "Lesson deleted successfully"));
            }
            catch (Exception ex)
            {
                return NotFound(new ApiResponse<string>(404, ex.Message));
            }
        }

        // 🔹 علاقات إضافية

        /// <summary>
        /// يرجع الامتحانات المرتبطة بالدرس
        /// </summary>
        [HttpGet("{id}/exams")]
        public async Task<IActionResult> GetExamsByLessonId(int id)
        {
            var exams = await _lessonService.GetExamsByLessonIdAsync(id);
            return Ok(new ApiResponse<IEnumerable<ExamReadDto>>(200, "Exams retrieved successfully", exams));
        }

        /// <summary>
        /// يرجع التدريبات المرتبطة بالدرس
        /// </summary>
        [HttpGet("{id}/exercises")]
        public async Task<IActionResult> GetExercisesByLessonId(int id)
        {
            var exercises = await _lessonService.GetExercisesByLessonIdAsync(id);
            return Ok(new ApiResponse<IEnumerable<ExerciseReadDto>>(200, "Exercises retrieved successfully", exercises));
        }

        /// <summary>
        /// يرجع النتائج المرتبطة بالدرس
        /// </summary>
        [HttpGet("{id}/results")]
        public async Task<IActionResult> GetResultsByLessonId(int id)
        {
            var results = await _lessonService.GetResultsByLessonIdAsync(id);
            return Ok(new ApiResponse<IEnumerable<ResultReadDto>>(200, "Results retrieved successfully", results));
        }

        /// <summary>
        /// يرجع جلسات المراجعة المرتبطة بالدرس
        /// </summary>
        [HttpGet("{id}/memorize-sessions")]
        public async Task<IActionResult> GetMemorizeSessionsByLessonId(int id)
        {
            var sessions = await _lessonService.GetMemorizeSessionsByLessonIdAsync(id);
            return Ok(new ApiResponse<IEnumerable<MemorizeSessionReadDto>>(200, "Memorize sessions retrieved successfully", sessions));
        }

        /// <summary>
        /// يرجع الأسئلة المرتبطة بالدرس
        /// </summary>
        [HttpGet("{id}/questions")]
        public async Task<IActionResult> GetQuestionsByLessonId(int id)
        {
            var questions = await _lessonService.GetQuestionsByLessonIdAsync(id);
            return Ok(new ApiResponse<IEnumerable<QuestionReadDto>>(200, "Questions retrieved successfully", questions));
        }

        // 🔹 إحصائيات

        /// <summary>
        /// يرجع عدد الأسئلة المرتبطة بالدرس
        /// </summary>
        [HttpGet("{id}/total-questions")]
        public async Task<IActionResult> GetTotalQuestionsByLessonId(int id)
        {
            var total = await _lessonService.GetTotalQuestionsByLessonIdAsync(id);
            return Ok(new ApiResponse<int>(200, "Total questions retrieved successfully", total));
        }

        /// <summary>
        /// يرجع عدد الامتحانات المرتبطة بالدرس
        /// </summary>
        [HttpGet("{id}/total-exams")]
        public async Task<IActionResult> GetTotalExamsByLessonId(int id)
        {
            var total = await _lessonService.GetTotalExamsByLessonIdAsync(id);
            return Ok(new ApiResponse<int>(200, "Total exams retrieved successfully", total));
        }
    }
}
