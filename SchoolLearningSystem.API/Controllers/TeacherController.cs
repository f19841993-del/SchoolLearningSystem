using Microsoft.AspNetCore.Mvc;
using SchoolLearningSystem.API.Responses;
using SchoolLearningSystem.Applicationf.DTOs.CourseDto;
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

        // 🔹 CRUD الأساسي

        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<TeacherReadDto>>), StatusCodes.Status200OK)]
        public async Task<ActionResult<ApiResponse<IEnumerable<TeacherReadDto>>>> GetAll()
        {
            var data = await _teacherService.GetAllAsync();
            return Ok(new ApiResponse<IEnumerable<TeacherReadDto>>(200, "Teachers retrieved successfully", data));
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ApiResponse<TeacherReadDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApiResponse<TeacherReadDto>>> GetById(int id)
        {
            var data = await _teacherService.GetByIdAsync(id);
            if (data == null)
                return NotFound(new ApiResponse(404, "Teacher not found"));

            return Ok(new ApiResponse<TeacherReadDto>(200, "Teacher retrieved successfully", data));
        }

        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<TeacherReadDto>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ApiResponse<TeacherReadDto>>> Add([FromBody] TeacherCreateDto dto)
        {
            var createdTeacher = await _teacherService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = createdTeacher.Id },
                new ApiResponse<TeacherReadDto>(201, "Teacher created successfully", createdTeacher));
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApiResponse>> Update(int id, [FromBody] TeacherUpdateDto dto)
        {
            await _teacherService.UpdateAsync(id, dto);
            return Ok(new ApiResponse(200, "Teacher updated successfully"));
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApiResponse>> Delete(int id)
        {
            await _teacherService.DeleteAsync(id);
            return Ok(new ApiResponse(200, "Teacher deleted successfully"));
        }

        // 🔹 علاقات إضافية — مطابقة حرفياً لـ ITeacherService
        // ⚠️ حُذفت: GetLessonsByTeacherId / GetTotalCourses / GetTotalLessons — كانت
        // تنادي دوال (GetLessonsByTeacherIdAsync, GetTotalCoursesByTeacherIdAsync,
        // GetTotalLessonsByTeacherIdAsync) غير موجودة إطلاقاً بـ ITeacherService.
        // إذا احتجتها فعلياً، أضفها أولاً بالـ Interface والـ Service.

        [HttpGet("{id}/courses")]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<CourseReadDto>>), StatusCodes.Status200OK)]
        public async Task<ActionResult<ApiResponse<IEnumerable<CourseReadDto>>>> GetCoursesByTeacherId(int id)
        {
            var courses = await _teacherService.GetCoursesByTeacherIdAsync(id);
            return Ok(new ApiResponse<IEnumerable<CourseReadDto>>(200, "Courses retrieved successfully", courses));
        }
    }
}
