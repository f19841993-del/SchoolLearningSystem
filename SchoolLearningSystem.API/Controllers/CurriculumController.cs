using Microsoft.AspNetCore.Mvc;
using SchoolLearningSystem.API.Responses;
using SchoolLearningSystem.Applicationf.DTOs.CourseDto;
using SchoolLearningSystem.Applicationf.DTOs.Curriculum;
using SchoolLearningSystem.Applicationf.Interfaces;

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
        /// يرجع كل المناهج الموجودة بالنظام
        /// </summary>
        /// <response code="200">تم جلب المناهج بنجاح</response>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var curriculums = await _curriculumService.GetAllCurriculumsAsync();
            return Ok(new ApiResponse<IEnumerable<CurriculumReadDto>>(200, "Curriculums retrieved successfully", curriculums));
        }

        /// <summary>
        /// يرجع منهج محدد حسب الـ Id
        /// </summary>
        /// <response code="200">تم جلب المنهج بنجاح</response>
        /// <response code="404">المنهج غير موجود</response>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var curriculum = await _curriculumService.GetCurriculumByIdAsync(id);
            if (curriculum == null)
                return NotFound(new ApiResponse<string>(404, "Curriculum not found"));

            return Ok(new ApiResponse<CurriculumReadDto>(200, "Curriculum retrieved successfully", curriculum));
        }

        /// <summary>
        /// إضافة منهج جديد للنظام
        /// </summary>
        /// <response code="201">تم إنشاء المنهج بنجاح</response>
        /// <response code="400">خطأ في البيانات المدخلة</response>
        [HttpPost]
        public async Task<IActionResult> Add(CurriculumCreateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ApiResponse<string>(400, "Invalid input data"));

            await _curriculumService.AddCurriculumAsync(dto);
            return StatusCode(201, new ApiResponse<string>(201, "Curriculum created successfully"));
        }

        /// <summary>
        /// تحديث بيانات منهج موجود
        /// </summary>
        /// <response code="200">تم تحديث المنهج بنجاح</response>
        /// <response code="404">المنهج غير موجود</response>
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, CurriculumUpdateDto dto)
        {
            try
            {
                await _curriculumService.UpdateCurriculumAsync(id, dto);
                return Ok(new ApiResponse<string>(200, "Curriculum updated successfully"));
            }
            catch (Exception ex)
            {
                return NotFound(new ApiResponse<string>(404, ex.Message));
            }
        }

        /// <summary>
        /// حذف منهج من النظام
        /// </summary>
        /// <response code="200">تم حذف المنهج بنجاح</response>
        /// <response code="404">المنهج غير موجود</response>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _curriculumService.DeleteCurriculumAsync(id);
                return Ok(new ApiResponse<string>(200, "Curriculum deleted successfully"));
            }
            catch (Exception ex)
            {
                return NotFound(new ApiResponse<string>(404, ex.Message));
            }
        }

        // 🔹 علاقات إضافية

        /// <summary>
        /// يرجع الكورسات المرتبطة بمنهج معين
        /// </summary>
        /// <response code="200">تم جلب الكورسات بنجاح</response>
        [HttpGet("{id}/courses")]
        public async Task<IActionResult> GetCoursesByCurriculumId(int id)
        {
            var courses = await _curriculumService.GetCoursesByCurriculumIdAsync(id);
            return Ok(new ApiResponse<IEnumerable<CourseReadDto>>(200, "Courses retrieved successfully", courses));
        }

        // 🔹 البحث حسب المرحلة الدراسية

        /// <summary>
        /// يرجع المنهج حسب المرحلة الدراسية
        /// </summary>
        /// <response code="200">تم جلب المنهج بنجاح</response>
        /// <response code="404">المنهج غير موجود</response>
        [HttpGet("grade/{gradeLevel}")]
        public async Task<IActionResult> GetByGradeLevel(string gradeLevel)
        {
            var curriculum = await _curriculumService.GetCurriculumByGradeLevelAsync(gradeLevel);
            if (curriculum == null)
                return NotFound(new ApiResponse<string>(404, "Curriculum not found"));

            return Ok(new ApiResponse<CurriculumReadDto>(200, "Curriculum retrieved successfully", curriculum));
        }

        // 🔹 إحصائيات

        /// <summary>
        /// يرجع عدد الكورسات المرتبطة بمنهج معين
        /// </summary>
        /// <response code="200">تم جلب العدد بنجاح</response>
        [HttpGet("{id}/total-courses")]
        public async Task<IActionResult> GetTotalCoursesByCurriculumId(int id)
        {
            var count = await _curriculumService.GetTotalCoursesByCurriculumIdAsync(id);
            return Ok(new ApiResponse<int>(200, "Total courses retrieved successfully", count));
        }
    }
}
