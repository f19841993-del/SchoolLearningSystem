using Microsoft.AspNetCore.Mvc;
using SchoolLearningSystem.Applicationf.Interfaces;
using SchoolLearningSystem.Applicationf.DTOs;

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
        [HttpGet]
        public async Task<ActionResult<IEnumerable<QuestionDto>>> GetAllQuestions()
        {
            var questions = await _questionService.GetAllQuestionsAsync();
            return Ok(questions);
        }

        /// <summary>
        /// يرجع بيانات سؤال محدد حسب الـ Id
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<QuestionDto>> GetQuestionById(int id)
        {
            var question = await _questionService.GetQuestionByIdAsync(id);
            if (question == null) return NotFound();
            return Ok(question);
        }

        /// <summary>
        /// إضافة سؤال جديد
        /// </summary>
        [HttpPost]
        public async Task<ActionResult> AddQuestion(QuestionDto dto)
        {
            await _questionService.AddQuestionAsync(dto);
            return CreatedAtAction(nameof(GetQuestionById), new { id = dto.Id }, dto);
        }

        /// <summary>
        /// تحديث سؤال موجود
        /// </summary>
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateQuestion(int id, QuestionDto dto)
        {
            if (id != dto.Id) return BadRequest();
            await _questionService.UpdateQuestionAsync(dto);
            return NoContent();
        }

        /// <summary>
        /// حذف سؤال
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteQuestion(int id)
        {
            await _questionService.DeleteQuestionAsync(id);
            return NoContent();
        }

        // 🔹 علاقات إضافية
        /// <summary>
        /// يرجع كل الأسئلة المرتبطة بدرس معين
        /// </summary>
        [HttpGet("lesson/{lessonId}")]
        public async Task<ActionResult<IEnumerable<QuestionDto>>> GetQuestionsByLessonId(int lessonId)
        {
            var questions = await _questionService.GetQuestionsByLessonIdAsync(lessonId);
            return Ok(questions);
        }
    }
}
