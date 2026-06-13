using Microsoft.AspNetCore.Mvc;
using SchoolLearningSystem.API.Responses;
using SchoolLearningSystem.Applicationf.DTOs.CourseDto;
using SchoolLearningSystem.Applicationf.DTOs.MemorizeSession;
using SchoolLearningSystem.Applicationf.DTOs.Result;
using SchoolLearningSystem.Applicationf.DTOs.Student;
using SchoolLearningSystem.Applicationf.Interfaces;

namespace SchoolLearningSystem.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StudentController : ControllerBase
    {
        private readonly IStudentService _studentService;

        public StudentController(IStudentService studentService)
        {
            _studentService = studentService;
        }

        // 🔹 CRUD الأساسي

        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<StudentReadDto>>), StatusCodes.Status200OK)]
        public async Task<ActionResult<ApiResponse<IEnumerable<StudentReadDto>>>> GetAll()
        {
            var students = await _studentService.GetAllAsync();
            return Ok(new ApiResponse<IEnumerable<StudentReadDto>>(200, "Students retrieved successfully", students));
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ApiResponse<StudentReadDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApiResponse<StudentReadDto>>> GetById(int id)
        {
            var student = await _studentService.GetByIdAsync(id);
            if (student == null)
                return NotFound(new ApiResponse<string>(404, "Student not found"));

            return Ok(new ApiResponse<StudentReadDto>(200, "Student retrieved successfully", student));
        }

        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ApiResponse<string>>> Add(StudentCreateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ApiResponse<string>(400, "Invalid input data"));

            await _studentService.CreateAsync(dto);
            return StatusCode(201, new ApiResponse<string>(201, "Student created successfully"));
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApiResponse<string>>> Update(int id, StudentUpdateDto dto)
        {
            try
            {
                await _studentService.UpdateAsync(id, dto);
                return Ok(new ApiResponse<string>(200, "Student updated successfully"));
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new ApiResponse<string>(404, ex.Message));
            }
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApiResponse<string>>> Delete(int id)
        {
            try
            {
                await _studentService.DeleteAsync(id);
                return Ok(new ApiResponse<string>(200, "Student deleted successfully"));
            }
            catch (Exception ex)
            {
                return NotFound(new ApiResponse<string>(404, ex.Message));
            }
        }

        // 🔹 علاقات إضافية (Custom Business Logic)

        [HttpGet("{id}/courses")]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<CourseReadDto>>), StatusCodes.Status200OK)]
        public async Task<ActionResult<ApiResponse<IEnumerable<CourseReadDto>>>> GetCourses(int id)
        {
            var courses = await _studentService.GetCoursesByStudentIdAsync(id);
            return Ok(new ApiResponse<IEnumerable<CourseReadDto>>(200, "Courses retrieved successfully", courses));
        }

        [HttpGet("{id}/results")]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<ResultReadDto>>), StatusCodes.Status200OK)]
        public async Task<ActionResult<ApiResponse<IEnumerable<ResultReadDto>>>> GetResults(int id)
        {
            var results = await _studentService.GetResultsByStudentIdAsync(id);
            return Ok(new ApiResponse<IEnumerable<ResultReadDto>>(200, "Results retrieved successfully", results));
        }

        [HttpGet("{id}/sessions")]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<MemorizeSessionReadDto>>), StatusCodes.Status200OK)]
        public async Task<ActionResult<ApiResponse<IEnumerable<MemorizeSessionReadDto>>>> GetSessions(int id)
        {
            var sessions = await _studentService.GetMemorizeSessionsByStudentIdAsync(id);
            return Ok(new ApiResponse<IEnumerable<MemorizeSessionReadDto>>(200, "Sessions retrieved successfully", sessions));
        }

        // 🔹 إحصائيات

        [HttpGet("{id}/average-score")]
        [ProducesResponseType(typeof(ApiResponse<double>), StatusCodes.Status200OK)]
        public async Task<ActionResult<ApiResponse<double>>> GetAverageScore(int id)
        {
            var avg = await _studentService.GetAverageScoreByStudentIdAsync(id);
            return Ok(new ApiResponse<double>(200, "Average score retrieved successfully", avg));
        }

        [HttpGet("{id}/total-courses")]
        [ProducesResponseType(typeof(ApiResponse<int>), StatusCodes.Status200OK)]
        public async Task<ActionResult<ApiResponse<int>>> GetTotalCourses(int id)
        {
            var count = await _studentService.GetTotalCoursesByStudentIdAsync(id);
            return Ok(new ApiResponse<int>(200, "Total courses count retrieved", count));
        }
    }
}