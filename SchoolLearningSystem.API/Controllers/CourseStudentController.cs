using Microsoft.AspNetCore.Mvc;
using SchoolLearningSystem.API.Responses;
using SchoolLearningSystem.Applicationf.DTOs.CourseDto;
using SchoolLearningSystem.Applicationf.DTOs.CourseStudent;
using SchoolLearningSystem.Applicationf.DTOs.Student;
using SchoolLearningSystem.Applicationf.Interfaces;

namespace SchoolLearningSystem.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CourseStudentController : ControllerBase
    {
        private readonly ICourseStudentService _courseStudentService;

        public CourseStudentController(ICourseStudentService courseStudentService)
        {
            _courseStudentService = courseStudentService;
        }

        // 🔹 CRUD الأساسي

        /// <summary>
        /// يرجع كل العلاقات بين الكورسات والطلاب
        /// </summary>
        /// <response code="200">تم جلب العلاقات بنجاح</response>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var relations = await _courseStudentService.GetAllCourseStudentsAsync();
            return Ok(new ApiResponse<IEnumerable<CourseStudentReadDto>>(200, "Relations retrieved successfully", relations));
        }

        /// <summary>
        /// يرجع علاقة محددة بين كورس وطالب
        /// </summary>
        /// <response code="200">تم جلب العلاقة بنجاح</response>
        /// <response code="404">العلاقة غير موجودة</response>
        [HttpGet("{courseId}/{studentId}")]
        public async Task<IActionResult> GetById(int courseId, int studentId)
        {
            var relation = await _courseStudentService.GetCourseStudentByIdAsync(courseId, studentId);
            if (relation == null)
                return NotFound(new ApiResponse<string>(404, "Relation not found"));

            return Ok(new ApiResponse<CourseStudentReadDto>(200, "Relation retrieved successfully", relation));
        }

        /// <summary>
        /// إضافة طالب إلى كورس
        /// </summary>
        /// <response code="201">تم تسجيل الطالب بالكورس بنجاح</response>
        /// <response code="400">خطأ في البيانات المدخلة</response>
        [HttpPost]
        public async Task<IActionResult> Add(CourseStudentCreateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ApiResponse<string>(400, "Invalid input data"));

            await _courseStudentService.AddCourseStudentAsync(dto);
            return StatusCode(201, new ApiResponse<string>(201, "Student enrolled successfully"));
        }

        /// <summary>
        /// تحديث بيانات علاقة طالب بكورس
        /// </summary>
        /// <response code="200">تم تحديث العلاقة بنجاح</response>
        /// <response code="404">العلاقة غير موجودة</response>
        [HttpPut("{courseId}/{studentId}")]
        public async Task<IActionResult> Update(int courseId, int studentId, CourseStudentUpdateDto dto)
        {
            try
            {
                await _courseStudentService.UpdateCourseStudentAsync(courseId, studentId, dto);
                return Ok(new ApiResponse<string>(200, "Relation updated successfully"));
            }
            catch (Exception ex)
            {
                return NotFound(new ApiResponse<string>(404, ex.Message));
            }
        }

        /// <summary>
        /// حذف طالب من كورس
        /// </summary>
        /// <response code="200">تم حذف الطالب من الكورس</response>
        /// <response code="404">العلاقة غير موجودة</response>
        [HttpDelete("{courseId}/{studentId}")]
        public async Task<IActionResult> Delete(int courseId, int studentId)
        {
            try
            {
                await _courseStudentService.DeleteCourseStudentAsync(courseId, studentId);
                return Ok(new ApiResponse<string>(200, "Relation deleted successfully"));
            }
            catch (Exception ex)
            {
                return NotFound(new ApiResponse<string>(404, ex.Message));
            }
        }

        // 🔹 علاقات إضافية

        /// <summary>
        /// يرجع الطلاب المرتبطين بكورس معين
        /// </summary>
        /// <response code="200">تم جلب الطلاب بنجاح</response>
        [HttpGet("{courseId}/students")]
        public async Task<IActionResult> GetStudentsByCourseId(int courseId)
        {
            var students = await _courseStudentService.GetStudentsByCourseIdAsync(courseId);
            return Ok(new ApiResponse<IEnumerable<StudentReadDto>>(200, "Students retrieved successfully", students));
        }

        /// <summary>
        /// يرجع الكورسات المرتبطة بطالب معين
        /// </summary>
        /// <response code="200">تم جلب الكورسات بنجاح</response>
        [HttpGet("student/{studentId}/courses")]
        public async Task<IActionResult> GetCoursesByStudentId(int studentId)
        {
            var courses = await _courseStudentService.GetCoursesByStudentIdAsync(studentId);
            return Ok(new ApiResponse<IEnumerable<CourseReadDto>>(200, "Courses retrieved successfully", courses));
        }

        // 🔹 إحصائيات

        /// <summary>
        /// يرجع عدد الطلاب المسجلين بكورس معين
        /// </summary>
        /// <response code="200">تم جلب العدد بنجاح</response>
        [HttpGet("{courseId}/total-students")]
        public async Task<IActionResult> GetTotalStudentsByCourseId(int courseId)
        {
            var count = await _courseStudentService.GetTotalStudentsByCourseIdAsync(courseId);
            return Ok(new ApiResponse<int>(200, "Total students retrieved successfully", count));
        }

        /// <summary>
        /// يرجع عدد الكورسات اللي مسجل بيها طالب معين
        /// </summary>
        /// <response code="200">تم جلب العدد بنجاح</response>
        [HttpGet("student/{studentId}/total-courses")]
        public async Task<IActionResult> GetTotalCoursesByStudentId(int studentId)
        {
            var count = await _courseStudentService.GetTotalCoursesByStudentIdAsync(studentId);
            return Ok(new ApiResponse<int>(200, "Total courses retrieved successfully", count));
        }
    }
}
