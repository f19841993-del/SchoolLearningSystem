using Microsoft.AspNetCore.Mvc;
using SchoolLearningSystem.Applicationf.Interfaces;
using SchoolLearningSystem.Applicationf.DTOs;

namespace SchoolLearningSystem.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ExamController : ControllerBase
    {
        private readonly IExamService _examService;

        public ExamController(IExamService examService)
        {
            _examService = examService;
        }

        // 🔹 CRUD الأساسي
        /// <summary>
        /// يرجع كل الامتحانات الموجودة بالنظام
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ExamDto>>> GetAllExams()
        {
            var exams = await _examService.GetAllExamsAsync();
            return Ok(exams);
        }

        /// <summary>
        /// يرجع بيانات امتحان محدد حسب الـ Id
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<ExamDto>> GetExamById(int id)
        {
            var exam = await _examService.GetExamByIdAsync(id);
            if (exam == null) return NotFound();
            return Ok(exam);
        }

        /// <summary>
        /// إضافة امتحان جديد
        /// </summary>
        [HttpPost]
        public async Task<ActionResult> AddExam(ExamDto dto)
        {
            await _examService.AddExamAsync(dto);
            return CreatedAtAction(nameof(GetExamById), new { id = dto.Id }, dto);
        }

        /// <summary>
        /// تحديث امتحان موجود
        /// </summary>
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateExam(int id, ExamDto dto)
        {
            if (id != dto.Id) return BadRequest();
            await _examService.UpdateExamAsync(dto);
            return NoContent();
        }

        /// <summary>
        /// حذف امتحان
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteExam(int id)
        {
            await _examService.DeleteExamAsync(id);
            return NoContent();
        }

        // 🔹 علاقات إضافية
        /// <summary>
        /// يرجع الأسئلة المرتبطة بالامتحان
        /// </summary>
        [HttpGet("{id}/questions")]
        public async Task<ActionResult<IEnumerable<QuestionDto>>> GetQuestionsByExamId(int id)
        {
            var questions = await _examService.GetQuestionsByExamIdAsync(id);
            return Ok(questions);
        }

        /// <summary>
        /// يرجع نتائج الطلاب المرتبطة بالامتحان
        /// </summary>
        [HttpGet("{id}/results")]
        public async Task<ActionResult<IEnumerable<ResultDto>>> GetResultsByExamId(int id)
        {
            var results = await _examService.GetResultsByExamIdAsync(id);
            return Ok(results);
        }
    }
}
