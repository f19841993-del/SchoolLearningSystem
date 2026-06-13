using Microsoft.AspNetCore.Mvc;
using SchoolLearningSystem.API.Responses;
using SchoolLearningSystem.Applicationf.DTOs.CourseDto;
using SchoolLearningSystem.Applicationf.DTOs.Lesson;
using SchoolLearningSystem.Applicationf.DTOs.Teacher;
using SchoolLearningSystem.Applicationf.Interfaces;

namespace SchoolLearningSystem.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TeacherController : ControllerBase
    {
        private readonly ITeacherService _teacherService;

        public TeacherController(ITeacherService teacherService)
        {
            _teacherService = teacherService;
        }

        // 🔹 CRUD الأساسي (موروث من BaseService)

        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<TeacherReadDto>>), StatusCodes.Status200OK)]
        public async Task<ActionResult<ApiResponse<IEnumerable<TeacherReadDto>>>> GetAll()
        {
            var data = await _teacherService.GetAllAsync();
            return Ok(new ApiResponse<IEnumerable<TeacherReadDto>>(200, "Teachers retrieved successfully", data));
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ApiResponse<TeacherReadDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApiResponse<TeacherReadDto>>> GetById(int id)
        {
            var data = await _teacherService.GetByIdAsync(id);
            if (data == null)
                return NotFound(new ApiResponse<string>(404, "Teacher not found"));

            return Ok(new ApiResponse<TeacherReadDto>(200, "Teacher retrieved successfully", data));
        }

        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<TeacherReadDto>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ApiResponse<TeacherReadDto>>> Add(TeacherCreateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ApiResponse<string>(400, "Invalid input data"));

            var createdTeacher = await _teacherService.CreateAsync(dto);
            return StatusCode(201, new ApiResponse<TeacherReadDto>(201, "Teacher created successfully", createdTeacher));
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApiResponse<string>>> Update(int id, TeacherUpdateDto dto)
        {
            try
            {
                await _teacherService.UpdateAsync(id, dto);
                return Ok(new ApiResponse<string>(200, "Teacher updated successfully"));
            }
            catch (Exception ex)
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
                await _teacherService.DeleteAsync(id);
                return Ok(new ApiResponse<string>(200, "Teacher deleted successfully"));
            }
            catch (Exception ex)
            {
                return NotFound(new ApiResponse<string>(404, ex.Message));
            }
        }

        // 🔹 علاقات إضافية (Custom Business Logic)

        [HttpGet("{id}/courses")]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<CourseReadDto>>), StatusCodes.Status200OK)]
        public async Task<ActionResult<ApiResponse<IEnumerable<CourseReadDto>>>> GetCoursesByTeacherId(int id)
        {
            var courses = await _teacherService.GetCoursesByTeacherIdAsync(id);
            return Ok(new ApiResponse<IEnumerable<CourseReadDto>>(200, "Courses retrieved successfully", courses));
        }

        [HttpGet("{id}/lessons")]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<LessonReadDto>>), StatusCodes.Status200OK)]
        public async Task<ActionResult<ApiResponse<IEnumerable<LessonReadDto>>>> GetLessonsByTeacherId(int id)
        {
            var lessons = await _teacherService.GetLessonsByTeacherIdAsync(id);
            return Ok(new ApiResponse<IEnumerable<LessonReadDto>>(200, "Lessons retrieved successfully", lessons));
        }

        // 🔹 إحصائيات

        [HttpGet("{id}/total-courses")]
        [ProducesResponseType(typeof(ApiResponse<int>), StatusCodes.Status200OK)]
        public async Task<ActionResult<ApiResponse<int>>> GetTotalCourses(int id)
        {
            var count = await _teacherService.GetTotalCoursesByTeacherIdAsync(id);
            return Ok(new ApiResponse<int>(200, "Total courses count retrieved", count));
        }

        [HttpGet("{id}/total-lessons")]
        [ProducesResponseType(typeof(ApiResponse<int>), StatusCodes.Status200OK)]
        public async Task<ActionResult<ApiResponse<int>>> GetTotalLessons(int id)
        {
            var count = await _teacherService.GetTotalLessonsByTeacherIdAsync(id);
            return Ok(new ApiResponse<int>(200, "Total lessons count retrieved", count));
        }
    }
}