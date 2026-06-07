using Microsoft.AspNetCore.Mvc;
using SchoolLearningSystem.Applicationf.Interfaces;
using SchoolLearningSystem.Applicationf.DTOs.Question;
using SchoolLearningSystem.API.Responses;

namespace SchoolLearningSystem.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class QuestionController : ControllerBase
    {
        private readonly IQuestionService _questionService;

        public QuestionController(IQuestionService questionService)
        {
            _questionService = questionService;
        }

        // 🔹 CRUD الأساسي

        /// <summary>
        /// يرجع كل الأسئلة الموجودة بالنظام
        /// </summary>
        /// <response code="200">تم جلب الأسئلة بنجاح</response>
        [HttpGet]
        public async Task<IActionResult> GetAllQuestions()
        {
            var questions = await _questionService.GetAllQuestionsAsync();
            return Ok(new ApiResponse<IEnumerable<QuestionReadDto>>(200, "Questions retrieved successfully", questions));
        }

        /// <summary>
        /// يرجع بيانات سؤال محدد حسب الـ Id
        /// </summary>
        /// <response code="200">تم جلب السؤال بنجاح</response>
        /// <response code="404">السؤال غير موجود</response>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetQuestionById(int id)
        {
            var question = await _questionService.GetQuestionByIdAsync(id);
            if (question == null)
                return NotFound(new ApiResponse<string>(404, "Question not found"));

            return Ok(new ApiResponse<QuestionReadDto>(200, "Question retrieved successfully", question));
        }

        /// <summary>
        /// إضافة سؤال جديد
        /// </summary>
        /// <response code="201">تم إنشاء السؤال بنجاح</response>
        /// <response code="400">خطأ في البيانات المدخلة</response>
        [HttpPost]
        public async Task<IActionResult> AddQuestion(QuestionCreateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ApiResponse<string>(400, "Invalid input data"));

            await _questionService.AddQuestionAsync(dto);
            return StatusCode(201, new ApiResponse<string>(201, "Question created successfully"));
        }

        /// <summary>
        /// تحديث سؤال موجود
        /// </summary>
        /// <response code="200">تم تحديث السؤال بنجاح</response>
        /// <response code="404">السؤال غير موجود</response>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateQuestion(int id, QuestionUpdateDto dto)
        {
            try
            {
                await _questionService.UpdateQuestionAsync(id, dto);
                return Ok(new ApiResponse<string>(200, "Question updated successfully"));
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new ApiResponse<string>(404, "Question not found"));
            }
        }

        /// <summary>
        /// حذف سؤال
        /// </summary>
        /// <response code="200">تم حذف السؤال بنجاح</response>
        /// <response code="404">السؤال غير موجود</response>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteQuestion(int id)
        {
            try
            {
                await _questionService.DeleteQuestionAsync(id);
                return Ok(new ApiResponse<string>(200, "Question deleted successfully"));
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new ApiResponse<string>(404, "Question not found"));
            }
        }

        // 🔹 علاقات إضافية

        /// <summary>
        /// يرجع كل الأسئلة المرتبطة بامتحان معين
        /// </summary>
        /// <response code="200">تم جلب الأسئلة بنجاح</response>
        [HttpGet("exam/{examId}")]
        public async Task<IActionResult> GetQuestionsByExamId(int examId)
        {
            var questions = await _questionService.GetQuestionsByExamIdAsync(examId);
            return Ok(new ApiResponse<IEnumerable<QuestionReadDto>>(200, "Questions retrieved successfully", questions));
        }

        /// <summary>
        /// يرجع كل الأسئلة المرتبطة بدرس معين
        /// </summary>
        /// <response code="200">تم جلب الأسئلة بنجاح</response>
        [HttpGet("lesson/{lessonId}")]
        public async Task<IActionResult> GetQuestionsByLessonId(int lessonId)
        {
            var questions = await _questionService.GetQuestionsByLessonIdAsync(lessonId);
            return Ok(new ApiResponse<IEnumerable<QuestionReadDto>>(200, "Questions retrieved successfully", questions));
        }

        // 🔹 إحصائيات إضافية

        /// <summary>
        /// يرجع عدد الأسئلة المرتبطة بامتحان معين
        /// </summary>
        [HttpGet("exam/{examId}/count")]
        public async Task<IActionResult> GetQuestionCountByExamId(int examId)
        {
            var count = await _questionService.GetQuestionCountByExamIdAsync(examId);
            return Ok(new ApiResponse<int>(200, "Question count retrieved successfully", count));
        }

        /// <summary>
        /// يرجع عدد الأسئلة حسب مستوى الصعوبة
        /// </summary>
        [HttpGet("difficulty/{difficultyLevel}/count")]
        public async Task<IActionResult> GetQuestionCountByDifficulty(string difficultyLevel)
        {
            var count = await _questionService.GetQuestionCountByDifficultyAsync(difficultyLevel);
            return Ok(new ApiResponse<int>(200, "Question count retrieved successfully", count));
        }
    }
}
