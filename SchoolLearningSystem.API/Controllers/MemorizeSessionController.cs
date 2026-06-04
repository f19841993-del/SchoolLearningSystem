using Microsoft.AspNetCore.Mvc;
using SchoolLearningSystem.Applicationf.Interfaces;
using SchoolLearningSystem.Applicationf.DTOs;

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

        /// <summary>
        /// يرجع كل جلسات المراجعة
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MemorizeSessionDto>>> GetAllSessions()
        {
            var sessions = await _memorizeService.GetAllSessionsAsync();
            return Ok(sessions);
        }

        /// <summary>
        /// يرجع جلسة مراجعة حسب الـ Id
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<MemorizeSessionDto>> GetSessionById(int id)
        {
            var session = await _memorizeService.GetSessionByIdAsync(id);
            if (session == null) return NotFound();
            return Ok(session);
        }

        /// <summary>
        /// إضافة جلسة مراجعة جديدة
        /// </summary>
        [HttpPost]
        public async Task<ActionResult> AddSession(MemorizeSessionDto dto)
        {
            await _memorizeService.AddSessionAsync(dto);
            return CreatedAtAction(nameof(GetSessionById), new { id = dto.Id }, dto);
        }

        /// <summary>
        /// تحديث جلسة مراجعة موجودة
        /// </summary>
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateSession(int id, MemorizeSessionDto dto)
        {
            if (id != dto.Id) return BadRequest();
            await _memorizeService.UpdateSessionAsync(dto);
            return NoContent();
        }

        /// <summary>
        /// حذف جلسة مراجعة
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteSession(int id)
        {
            await _memorizeService.DeleteSessionAsync(id);
            return NoContent();
        }

        // 🔹 علاقات إضافية
        /// <summary>
        /// يرجع جلسات المراجعة لطالب معين
        /// </summary>
        [HttpGet("student/{studentId}")]
        public async Task<ActionResult<IEnumerable<MemorizeSessionDto>>> GetSessionsByStudentId(int studentId)
        {
            var sessions = await _memorizeService.GetSessionsByStudentIdAsync(studentId);
            return Ok(sessions);
        }

        /// <summary>
        /// يرجع جلسات المراجعة لدرس معين
        /// </summary>
        [HttpGet("lesson/{lessonId}")]
        public async Task<ActionResult<IEnumerable<MemorizeSessionDto>>> GetSessionsByLessonId(int lessonId)
        {
            var sessions = await _memorizeService.GetSessionsByLessonIdAsync(lessonId);
            return Ok(sessions);
        }
    }
}
