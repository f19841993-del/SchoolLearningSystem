using Microsoft.AspNetCore.Mvc;
using SchoolLearningSystem.API.Responses;
using SchoolLearningSystem.Application.Common.Models;
using SchoolLearningSystem.Application.Common.Parameters;
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

        #region 1. عمليات التسجيل (Enrollment)

        /// <summary>
        /// تسجيل طالب في كورس معين
        /// </summary>
        [Tags("1. إدارة التسجيل (Enrollment)")]
        [HttpPost("enroll")]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<string>>> EnrollStudent([FromBody] CourseStudentCreateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ApiResponse(400, "Invalid input data"));

            await _courseStudentService.EnrollStudentAsync(dto.CourseId, dto.StudentId);
            return StatusCode(201, new ApiResponse<string>(201, "Student enrolled successfully"));
        }

        /// <summary>
        /// إلغاء تسجيل طالب من كورس
        /// </summary>
        [Tags("1. إدارة التسجيل (Enrollment)")]
        [HttpDelete("remove/{courseId}/{studentId}")]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<string>>> RemoveStudent(int courseId, int studentId)
        {
            await _courseStudentService.RemoveStudentAsync(courseId, studentId);
            return Ok(new ApiResponse<string>(200, "Student removed from course successfully"));
        }

        #endregion

        #region 2. الاستعلام عبر العلاقات (Relationships with Pagination)

        /// <summary>
        /// جلب قائمة الطلاب المسجلين في كورس معين (مرقمة)
        /// </summary>
        [Tags("2. الاستعلام والعلاقات (Queries)")]
        [HttpGet("course/{courseId}/students/paged")]
        [ProducesResponseType(typeof(ApiResponse<PagedList<StudentReadDto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<PagedList<StudentReadDto>>>> GetPagedStudentsByCourse(
            int courseId, [FromQuery] QueryParameters parameters)
        {
            var pagedStudents = await _courseStudentService.GetPagedStudentsByCourseIdAsync(courseId, parameters);
            return Ok(new ApiResponse<PagedList<StudentReadDto>>(200, "Students retrieved successfully", pagedStudents));
        }

        /// <summary>
        /// جلب قائمة الكورسات التي سجل فيها طالب معين (مرقمة)
        /// </summary>
        [Tags("2. الاستعلام والعلاقات (Queries)")]
        [HttpGet("student/{studentId}/courses/paged")]
        [ProducesResponseType(typeof(ApiResponse<PagedList<CourseReadDto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<PagedList<CourseReadDto>>>> GetPagedCoursesByStudent(
            int studentId, [FromQuery] QueryParameters parameters)
        {
            var pagedCourses = await _courseStudentService.GetPagedCoursesByStudentIdAsync(studentId, parameters);
            return Ok(new ApiResponse<PagedList<CourseReadDto>>(200, "Courses retrieved successfully", pagedCourses));
        }

        #endregion

        #region 3. الإحصائيات (Statistics)

        /// <summary>
        /// جلب عدد الطلاب الكلي المسجلين في كورس
        /// </summary>
        [Tags("3. الإحصائيات (Statistics)")]
        [HttpGet("course/{courseId}/students/count")]
        [ProducesResponseType(typeof(ApiResponse<int>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<int>>> GetTotalStudentsByCourseId(int courseId)
        {
            var count = await _courseStudentService.GetTotalStudentsByCourseIdAsync(courseId);
            return Ok(new ApiResponse<int>(200, "Total students count retrieved successfully", count));
        }

        /// <summary>
        /// جلب عدد الكورسات التي سجل فيها الطالب
        /// </summary>
        [Tags("3. الإحصائيات (Statistics)")]
        [HttpGet("student/{studentId}/courses/count")]
        [ProducesResponseType(typeof(ApiResponse<int>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<int>>> GetTotalCoursesByStudentId(int studentId)
        {
            var count = await _courseStudentService.GetTotalCoursesByStudentIdAsync(studentId);
            return Ok(new ApiResponse<int>(200, "Total courses count retrieved successfully", count));
        }

        #endregion
    }
}