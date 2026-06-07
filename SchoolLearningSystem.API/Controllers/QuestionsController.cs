using Microsoft.AspNetCore.Mvc;
using SchoolLearningSystem.Applicationf.Interfaces;
using SchoolLearningSystem.Applicationf.DTOs.Question;

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
        public async Task<ActionResult<IEnumerable<QuestionReadDto>>> GetAllQuestions()
        {
            var questions = await _questionService.GetAllQuestionsAsync();
            return Ok(questions);
        }

        /// <summary>
        /// يرجع بيانات سؤال محدد حسب الـ Id
        /// </summary>
        /// <response code="200">تم جلب السؤال بنجاح</response>
        /// <response code="404">السؤال غير موجود</response>
        [HttpGet("{id}")]
        public async Task<ActionResult<QuestionReadDto>> GetQuestionById(int id)
        {
            var question = await _questionService.GetQuestionByIdAsync(id);
            if (question == null) return NotFound("Question not found");
            return Ok(question);
        }

        /// <summary>
        /// إضافة سؤال جديد
        /// </summary>
        /// <response code="201">تم إنشاء السؤال بنجاح</response>
        /// <response code="400">خطأ في البيانات المدخلة</response>
        [HttpPost]
        public async Task<ActionResult> AddQuestion(QuestionCreateDto dto)
        {
            if (!ModelState.IsValid) return BadRequest("Invalid input data");

            await _questionService.AddQuestionAsync(dto);
            return StatusCode(201, "Question created successfully");
        }

        /// <summary>
        /// تحديث سؤال موجود
        /// </summary>
        /// <response code="200">تم تحديث السؤال بنجاح</response>
        /// <response code="404">السؤال غير موجود</response>
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateQuestion(int id, QuestionUpdateDto dto)
        {
            try
            {
                await _questionService.UpdateQuestionAsync(id, dto);
                return Ok("Question updated successfully");
            }
            catch (KeyNotFoundException)
            {
                return NotFound("Question not found");
            }
        }

        /// <summary>
        /// حذف سؤال
        /// </summary>
        /// <response code="200">تم حذف السؤال بنجاح</response>
        /// <response code="404">السؤال غير موجود</response>
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteQuestion(int id)
        {
            try
            {
                await _questionService.DeleteQuestionAsync(id);
                return Ok("Question deleted successfully");
            }
            catch (KeyNotFoundException)
            {
                return NotFound("Question not found");
            }
        }

        // 🔹 علاقات إضافية

        /// <summary>
        /// يرجع كل الأسئلة المرتبطة بامتحان معين
        /// </summary>
        /// <response code="200">تم جلب الأسئلة بنجاح</response>
        [HttpGet("exam/{examId}")]
        public async Task<ActionResult<IEnumerable<QuestionReadDto>>> GetQuestionsByExamId(int examId)
        {
            var questions = await _questionService.GetQuestionsByExamIdAsync(examId);
            return Ok(questions);
        }

        /// <summary>
        /// يرجع كل الأسئلة المرتبطة بدرس معين
        /// </summary>
        /// <response code="200">تم جلب الأسئلة بنجاح</response>
        [HttpGet("lesson/{lessonId}")]
        public async Task<ActionResult<IEnumerable<QuestionReadDto>>> GetQuestionsByLessonId(int lessonId)
        {
            var questions = await _questionService.GetQuestionsByLessonIdAsync(lessonId);
            return Ok(questions);
        }
    }
}
