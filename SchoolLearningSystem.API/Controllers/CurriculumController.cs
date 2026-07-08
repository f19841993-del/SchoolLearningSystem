using Microsoft.AspNetCore.Mvc;
using SchoolLearningSystem.API.Responses;
using SchoolLearningSystem.Applicationf.DTOs.CourseDto;
using SchoolLearningSystem.Applicationf.DTOs.CurriculumDto;
using SchoolLearningSystem.Applicationf.Interfaces;
using SchoolLearningSystem.Domain.Enums;

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

        // 🔹 CRUD الأساسي (يستخدم دوال BaseService الموحدة)

        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<CurriculumReadDto>>), StatusCodes.Status200OK)]
        public async Task<ActionResult<ApiResponse<IEnumerable<CurriculumReadDto>>>> GetAll()
        {
            var data = await _curriculumService.GetAllAsync();
            return Ok(new ApiResponse<IEnumerable<CurriculumReadDto>>(200, "Curriculums retrieved successfully", data));
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ApiResponse<CurriculumReadDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApiResponse<CurriculumReadDto>>> GetById(int id)
        {
            var data = await _curriculumService.GetByIdAsync(id);
            if (data == null)
                return NotFound(new ApiResponse(404, "Curriculum not found"));

            return Ok(new ApiResponse<CurriculumReadDto>(200, "Curriculum retrieved successfully", data));
        }

        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<CurriculumReadDto>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ApiResponse<CurriculumReadDto>>> Add([FromBody] CurriculumCreateDto dto)
        {
            // ⚠️ ملاحظة: عدّل الـ Service ليرجع الكيان المُنشأ (CurriculumReadDto) بدل void
            // حتى نقدر نستخدم CreatedAtAction بشكل صحيح مع رابط الموقع (Location header)
            var created = await _curriculumService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = created.Id },
                new ApiResponse<CurriculumReadDto>(201, "Curriculum created successfully", created));
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApiResponse>> Update(int id, [FromBody] CurriculumUpdateDto dto)
        {
            // 🗑️ حُذف try-catch: NotFoundException تُعالَج مركزياً بالـ ExceptionMiddleware
            await _curriculumService.UpdateAsync(id, dto);
            return Ok(new ApiResponse(200, "Curriculum updated successfully"));
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApiResponse>> Delete(int id)
        {
            await _curriculumService.DeleteAsync(id);
            return Ok(new ApiResponse(200, "Curriculum deleted successfully"));
        }

        // 🔹 علاقات إضافية (Business Logic)

        [HttpGet("{id}/courses")]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<CourseReadDto>>), StatusCodes.Status200OK)]
        public async Task<ActionResult<ApiResponse<IEnumerable<CourseReadDto>>>> GetCoursesByCurriculumId(int id)
        {
            var courses = await _curriculumService.GetCoursesByCurriculumIdAsync(id);
            return Ok(new ApiResponse<IEnumerable<CourseReadDto>>(200, "Courses retrieved successfully", courses));
        }

        [HttpGet("grade/{gradeLevel}")]
        [ProducesResponseType(typeof(ApiResponse<CurriculumReadDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApiResponse<CurriculumReadDto>>> GetByGradeLevel(GradeLevel gradeLevel)
        {
            var curriculum = await _curriculumService.GetCurriculumByGradeLevelAsync(gradeLevel);
            if (curriculum == null)
                return NotFound(new ApiResponse(404, "Curriculum not found"));

            return Ok(new ApiResponse<CurriculumReadDto>(200, "Curriculum retrieved successfully", curriculum));
        }

        [HttpGet("{id}/total-courses")]
        [ProducesResponseType(typeof(ApiResponse<int>), StatusCodes.Status200OK)]
        public async Task<ActionResult<ApiResponse<int>>> GetTotalCoursesByCurriculumId(int id)
        {
            var count = await _curriculumService.GetTotalCoursesByCurriculumIdAsync(id);
            return Ok(new ApiResponse<int>(200, "Total courses retrieved successfully", count));
        }
    }
}
