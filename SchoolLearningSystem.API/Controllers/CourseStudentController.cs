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

        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<CourseStudentReadDto>>), StatusCodes.Status200OK)]
        public async Task<ActionResult<ApiResponse<IEnumerable<CourseStudentReadDto>>>> GetAll()
        {
            var relations = await _courseStudentService.GetAllCourseStudentsAsync();
            return Ok(new ApiResponse<IEnumerable<CourseStudentReadDto>>(200, "Relations retrieved successfully", relations));
        }

        [HttpGet("{courseId}/{studentId}")]
        [ProducesResponseType(typeof(ApiResponse<CourseStudentReadDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApiResponse<CourseStudentReadDto>>> GetById(int courseId, int studentId)
        {
            var relation = await _courseStudentService.GetCourseStudentByIdAsync(courseId, studentId);
            if (relation == null)
                return NotFound(new ApiResponse<string>(404, "Relation not found"));

            return Ok(new ApiResponse<CourseStudentReadDto>(200, "Relation retrieved successfully", relation));
        }

        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ApiResponse<string>>> Add(CourseStudentCreateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ApiResponse<string>(400, "Invalid input data"));

            await _courseStudentService.AddCourseStudentAsync(dto);
            return StatusCode(201, new ApiResponse<string>(201, "Student enrolled successfully"));
        }

        [HttpPut("{courseId}/{studentId}")]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApiResponse<string>>> Update(int courseId, int studentId, CourseStudentUpdateDto dto)
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

        [HttpDelete("{courseId}/{studentId}")]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApiResponse<string>>> Delete(int courseId, int studentId)
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

        [HttpGet("{courseId}/students")]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<StudentReadDto>>), StatusCodes.Status200OK)]
        public async Task<ActionResult<ApiResponse<IEnumerable<StudentReadDto>>>> GetStudentsByCourseId(int courseId)
        {
            var students = await _courseStudentService.GetStudentsByCourseIdAsync(courseId);
            return Ok(new ApiResponse<IEnumerable<StudentReadDto>>(200, "Students retrieved successfully", students));
        }

        [HttpGet("student/{studentId}/courses")]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<CourseReadDto>>), StatusCodes.Status200OK)]
        public async Task<ActionResult<ApiResponse<IEnumerable<CourseReadDto>>>> GetCoursesByStudentId(int studentId)
        {
            var courses = await _courseStudentService.GetCoursesByStudentIdAsync(studentId);
            return Ok(new ApiResponse<IEnumerable<CourseReadDto>>(200, "Courses retrieved successfully", courses));
        }

        // 🔹 إحصائيات

        [HttpGet("{courseId}/total-students")]
        [ProducesResponseType(typeof(ApiResponse<int>), StatusCodes.Status200OK)]
        public async Task<ActionResult<ApiResponse<int>>> GetTotalStudentsByCourseId(int courseId)
        {
            var count = await _courseStudentService.GetTotalStudentsByCourseIdAsync(courseId);
            return Ok(new ApiResponse<int>(200, "Total students retrieved successfully", count));
        }

        [HttpGet("student/{studentId}/total-courses")]
        [ProducesResponseType(typeof(ApiResponse<int>), StatusCodes.Status200OK)]
        public async Task<ActionResult<ApiResponse<int>>> GetTotalCoursesByStudentId(int studentId)
        {
            var count = await _courseStudentService.GetTotalCoursesByStudentIdAsync(studentId);
            return Ok(new ApiResponse<int>(200, "Total courses retrieved successfully", count));
        }
    }
}