using Microsoft.AspNetCore.Mvc;
using SchoolLearningSystem.Applicationf.Interfaces;
using SchoolLearningSystem.Applicationf.DTOs;

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
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<LessonDto>>> GetAllLessons()
        {
            var lessons = await _lessonService.GetAllLessonsAsync();
            return Ok(lessons);
        }

        /// <summary>
        /// يرجع بيانات درس محدد حسب الـ Id
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<LessonDto>> GetLessonById(int id)
        {
            var lesson = await _lessonService.GetLessonByIdAsync(id);
            if (lesson == null) return NotFound();
            return Ok(lesson);
        }

        /// <summary>
        /// إضافة درس جديد للنظام
        /// </summary>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult> AddLesson(LessonDto dto)
        {
            await _lessonService.AddLessonAsync(dto);
            return CreatedAtAction(nameof(GetLessonById), new { id = dto.Id }, dto);
        }

        /// <summary>
        /// تحديث بيانات درس موجود
        /// </summary>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> UpdateLesson(int id, LessonDto dto)
        {
            if (id != dto.Id) return BadRequest();
            await _lessonService.UpdateLessonAsync(dto);
            return NoContent();
        }

        /// <summary>
        /// حذف درس من النظام
        /// </summary>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult> DeleteLesson(int id)
        {
            await _lessonService.DeleteLessonAsync(id);
            return NoContent();
        }

        // 🔹 علاقات إضافية
        /// <summary>
        /// يرجع الأسئلة المرتبطة بالدرس
        /// </summary>
        [HttpGet("{id}/questions")]
        public async Task<ActionResult<IEnumerable<QuestionDto>>> GetQuestionsByLessonId(int id)
        {
            var questions = await _lessonService.GetQuestionsByLessonIdAsync(id);
            return Ok(questions);
        }

        /// <summary>
        /// يرجع الامتحانات المرتبطة بالدرس
        /// </summary>
        [HttpGet("{id}/exams")]
        public async Task<ActionResult<IEnumerable<ExamDto>>> GetExamsByLessonId(int id)
        {
            var exams = await _lessonService.GetExamsByLessonIdAsync(id);
            return Ok(exams);
        }

        // 🔹 إحصائيات
        /// <summary>
        /// يرجع عدد الأسئلة المرتبطة بالدرس
        /// </summary>
        [HttpGet("{id}/total-questions")]
        public async Task<ActionResult<int>> GetTotalQuestionsByLessonId(int id)
        {
            var total = await _lessonService.GetTotalQuestionsByLessonIdAsync(id);
            return Ok(total);
        }

        /// <summary>
        /// يرجع عدد الامتحانات المرتبطة بالدرس
        /// </summary>
        [HttpGet("{id}/total-exams")]
        public async Task<ActionResult<int>> GetTotalExamsByLessonId(int id)
        {
            var total = await _lessonService.GetTotalExamsByLessonIdAsync(id);
            return Ok(total);
        }
    }
}
