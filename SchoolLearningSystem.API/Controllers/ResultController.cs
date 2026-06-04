using Microsoft.AspNetCore.Mvc;
using SchoolLearningSystem.Applicationf.Interfaces;
using SchoolLearningSystem.Applicationf.DTOs;

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
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ResultDto>>> GetAllResults()
        {
            var results = await _resultService.GetAllResultsAsync();
            return Ok(results);
        }

        /// <summary>
        /// يرجع بيانات نتيجة محددة حسب الـ Id
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<ResultDto>> GetResultById(int id)
        {
            var result = await _resultService.GetResultByIdAsync(id);
            if (result == null) return NotFound();
            return Ok(result);
        }

        /// <summary>
        /// إضافة نتيجة جديدة
        /// </summary>
        [HttpPost]
        public async Task<ActionResult> AddResult(ResultDto dto)
        {
            await _resultService.AddResultAsync(dto);
            return CreatedAtAction(nameof(GetResultById), new { id = dto.Id }, dto);
        }

        /// <summary>
        /// تحديث نتيجة موجودة
        /// </summary>
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateResult(int id, ResultDto dto)
        {
            if (id != dto.Id) return BadRequest();
            await _resultService.UpdateResultAsync(dto);
            return NoContent();
        }

        /// <summary>
        /// حذف نتيجة
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteResult(int id)
        {
            await _resultService.DeleteResultAsync(id);
            return NoContent();
        }

        // 🔹 علاقات إضافية
        /// <summary>
        /// يرجع نتائج الطلاب المرتبطة بامتحان معين
        /// </summary>
        [HttpGet("exam/{examId}")]
        public async Task<ActionResult<IEnumerable<ResultDto>>> GetResultsByExamId(int examId)
        {
            var results = await _resultService.GetResultsByExamIdAsync(examId);
            return Ok(results);
        }

        /// <summary>
        /// يرجع نتائج طالب معين
        /// </summary>
        [HttpGet("student/{studentId}")]
        public async Task<ActionResult<IEnumerable<ResultDto>>> GetResultsByStudentId(int studentId)
        {
            var results = await _resultService.GetResultsByStudentIdAsync(studentId);
            return Ok(results);
        }
    }
}
