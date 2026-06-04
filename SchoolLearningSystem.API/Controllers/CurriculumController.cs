using Microsoft.AspNetCore.Mvc;
using SchoolLearningSystem.Applicationf.Interfaces;
using SchoolLearningSystem.Applicationf.DTOs;

namespace SchoolLearningSystem.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CurriculumController : ControllerBase
    {
        private readonly ICurriculumService _curriculumService;

        public CurriculumController(ICurriculumService curriculumService)
        {
            _curriculumService = curriculumService;
        }

        // 🔹 CRUD الأساسي
        /// <summary>
        /// يرجع كل المناهج الدراسية الموجودة بالنظام
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CurriculumDto>>> GetAllCurriculums()
        {
            var curriculums = await _curriculumService.GetAllCurriculumsAsync();
            return Ok(curriculums);
        }

        /// <summary>
        /// يرجع بيانات منهج محدد حسب الـ Id
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<CurriculumDto>> GetCurriculumById(int id)
        {
            var curriculum = await _curriculumService.GetCurriculumByIdAsync(id);
            if (curriculum == null) return NotFound();
            return Ok(curriculum);
        }

        /// <summary>
        /// إضافة منهج جديد
        /// </summary>
        [HttpPost]
        public async Task<ActionResult> AddCurriculum(CurriculumDto dto)
        {
            await _curriculumService.AddCurriculumAsync(dto);
            return CreatedAtAction(nameof(GetCurriculumById), new { id = dto.Id }, dto);
        }

        /// <summary>
        /// تحديث بيانات منهج موجود
        /// </summary>
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateCurriculum(int id, CurriculumDto dto)
        {
            if (id != dto.Id) return BadRequest();
            await _curriculumService.UpdateCurriculumAsync(dto);
            return NoContent();
        }

        /// <summary>
        /// حذف منهج من النظام
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteCurriculum(int id)
        {
            await _curriculumService.DeleteCurriculumAsync(id);
            return NoContent();
        }

        // 🔹 علاقات إضافية
        /// <summary>
        /// يرجع الكورسات المرتبطة بالمنهج
        /// </summary>
        [HttpGet("{id}/courses")]
        public async Task<ActionResult<IEnumerable<CourseDto>>> GetCoursesByCurriculumId(int id)
        {
            var courses = await _curriculumService.GetCoursesByCurriculumIdAsync(id);
            return Ok(courses);
        }
    }
}
