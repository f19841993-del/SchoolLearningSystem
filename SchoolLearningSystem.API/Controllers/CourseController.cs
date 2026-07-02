using Microsoft.AspNetCore.Mvc;
using SchoolLearningSystem.API.Responses;
using SchoolLearningSystem.Application.Common.Models;
using SchoolLearningSystem.Application.Common.Parameters;
using SchoolLearningSystem.Applicationf.DTOs.CourseDto;
using SchoolLearningSystem.Applicationf.DTOs.ExamDto;
using SchoolLearningSystem.Applicationf.DTOs.Lesson;
using SchoolLearningSystem.Applicationf.DTOs.Student;
using SchoolLearningSystem.Applicationf.Interfaces;

namespace SchoolLearningSystem.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CourseController : ControllerBase
    {
        private readonly ICourseService _courseService;

        public CourseController(ICourseService courseService)
        {
            _courseService = courseService;
        }

        // 🔹 CRUD الأساسي

        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<CourseReadDto>>), StatusCodes.Status200OK)]
        public async Task<ActionResult<ApiResponse<IEnumerable<CourseReadDto>>>> GetAllCourses()
        {
            var courses = await _courseService.GetAllAsync();
            return Ok(new ApiResponse<IEnumerable<CourseReadDto>>(200, "Courses retrieved successfully", courses));
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ApiResponse<CourseReadDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)] // 💡 تعديل: استخدمنا الكلاس العادي للخطأ
        public async Task<ActionResult<ApiResponse<CourseReadDto>>> GetCourseById(int id)
        {
            var course = await _courseService.GetByIdAsync(id);
            if (course == null)
                return NotFound(new ApiResponse(404, "Course not found")); // 💡 تعديل: بدون <string>

            return Ok(new ApiResponse<CourseReadDto>(200, "Course retrieved successfully", course));
        }

        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<CourseReadDto>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)] // 💡 تعديل
        public async Task<ActionResult<ApiResponse<CourseReadDto>>> AddCourse([FromBody] CourseCreateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ApiResponse(400, "Invalid input data")); // 💡 تعديل: بدون <string>

            var createdCourse = await _courseService.CreateAsync(dto);

            return CreatedAtAction(nameof(GetCourseById), new { id = createdCourse.Id },
                new ApiResponse<CourseReadDto>(201, "Course created successfully", createdCourse));
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)] // 💡 تعديل: إرجاع الكلاس العادي فقط
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)] // 💡 تعديل
        public async Task<ActionResult<ApiResponse>> UpdateCourse(int id, [FromBody] CourseUpdateDto dto) // 💡 تعديل: Task<ActionResult<ApiResponse>>
        {
            await _courseService.UpdateAsync(id, dto);
            // 💡 تعديل: رسالة نجاح نقية بدون الحاجة لتمرير نوع بيانات وهمي
            return Ok(new ApiResponse(200, "Course updated successfully"));
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)] // 💡 تعديل
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)] // 💡 تعديل
        public async Task<ActionResult<ApiResponse>> DeleteCourse(int id) // 💡 تعديل
        {
            await _courseService.DeleteAsync(id);
            // 💡 تعديل: كود نظيف ومباشر
            return Ok(new ApiResponse(200, "Course deleted successfully"));
        }

        // 🔹 علاقات إضافية

        [HttpGet("{id}/lessons")]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<LessonReadDto>>), StatusCodes.Status200OK)]
        public async Task<ActionResult<ApiResponse<IEnumerable<LessonReadDto>>>> GetLessonsByCourseId(int id)
        {
            var lessons = await _courseService.GetLessonsByCourseIdAsync(id);
            return Ok(new ApiResponse<IEnumerable<LessonReadDto>>(200, "Lessons retrieved successfully", lessons));
        }

        [HttpGet("{id}/students")]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<StudentReadDto>>), StatusCodes.Status200OK)]
        public async Task<ActionResult<ApiResponse<IEnumerable<StudentReadDto>>>> GetStudentsByCourseId(int id)
        {
            var students = await _courseService.GetStudentsByCourseIdAsync(id);
            return Ok(new ApiResponse<IEnumerable<StudentReadDto>>(200, "Students retrieved successfully", students));
        }

        [HttpGet("{id}/exams")]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<ExamReadDto>>), StatusCodes.Status200OK)]
        public async Task<ActionResult<ApiResponse<IEnumerable<ExamReadDto>>>> GetExamsByCourseId(int id)
        {
            var exams = await _courseService.GetExamsByCourseIdAsync(id);
            return Ok(new ApiResponse<IEnumerable<ExamReadDto>>(200, "Exams retrieved successfully", exams));
        }

        [HttpGet("{id}/teacher")]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)] // 💡 تعديل
        public async Task<ActionResult<ApiResponse<string>>> GetTeacherByCourseId(int id)
        {
            var course = await _courseService.GetByIdAsync(id);
            if (course == null)
                return NotFound(new ApiResponse(404, "Course not found")); // 💡 تعديل

            return Ok(new ApiResponse<string>(200, "Teacher retrieved successfully", course.TeacherName));
        }

        [HttpGet("{id}/curriculum")]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)] // 💡 تعديل
        public async Task<ActionResult<ApiResponse<string>>> GetCurriculumByCourseId(int id)
        {
            var course = await _courseService.GetByIdAsync(id);
            if (course == null)
                return NotFound(new ApiResponse(404, "Course not found")); // 💡 تعديل

            return Ok(new ApiResponse<string>(200, "Curriculum retrieved successfully", course.CurriculumTitle));
        }

        // 🔹 دالة الترقيم (Pagination)
        [HttpGet("paged")]
        [ProducesResponseType(typeof(ApiResponse<PagedList<CourseReadDto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)] // 💡 تعديل
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status500InternalServerError)] // 💡 تعديل
        public async Task<ActionResult<ApiResponse<PagedList<CourseReadDto>>>> GetPagedCourses(
            [FromQuery] QueryParameters parameters)
        {
            var pagedCourses = await _courseService.GetPagedAsync(parameters);
            return Ok(new ApiResponse<PagedList<CourseReadDto>>(200, "Courses retrieved successfully", pagedCourses));
        }
    }
}


//using Microsoft.AspNetCore.Mvc;
//using SchoolLearningSystem.API.Responses;
//using SchoolLearningSystem.Applicationf.DTOs.CourseDto;
//using SchoolLearningSystem.Applicationf.DTOs.ExamDto;
//using SchoolLearningSystem.Applicationf.DTOs.Lesson;
//using SchoolLearningSystem.Applicationf.DTOs.Student;
//using SchoolLearningSystem.Applicationf.Interfaces;

//namespace SchoolLearningSystem.API.Controllers
//{
//    [ApiController]
//    [Route("api/[controller]")]
//    public class CourseController : ControllerBase
//    {
//        private readonly ICourseService _courseService;

//        public CourseController(ICourseService courseService)
//        {
//            _courseService = courseService;
//        }

//        // 🔹 CRUD الأساسي

//        [HttpGet]
//        [ProducesResponseType(typeof(ApiResponse<IEnumerable<CourseReadDto>>), StatusCodes.Status200OK)]
//        public async Task<ActionResult<ApiResponse<IEnumerable<CourseReadDto>>>> GetAllCourses()
//        {
//            var courses = await _courseService.GetAllAsync();
//            return Ok(new ApiResponse<IEnumerable<CourseReadDto>>(200, "Courses retrieved successfully", courses));
//        }

//        [HttpGet("{id}")]
//        [ProducesResponseType(typeof(ApiResponse<CourseReadDto>), StatusCodes.Status200OK)]
//        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status404NotFound)]
//        public async Task<ActionResult<ApiResponse<CourseReadDto>>> GetCourseById(int id)
//        {
//            var course = await _courseService.GetByIdAsync(id);
//            if (course == null)
//                return NotFound(new ApiResponse<string>(404, "Course not found"));

//            return Ok(new ApiResponse<CourseReadDto>(200, "Course retrieved successfully", course));
//        }

//        [HttpPost]
//        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status201Created)]
//        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status400BadRequest)]
//        public async Task<ActionResult<ApiResponse<string>>> AddCourse(CourseCreateDto dto)
//        {
//            if (!ModelState.IsValid)
//                return BadRequest(new ApiResponse<string>(400, "Invalid input data"));

//            await _courseService.CreateAsync(dto);
//            return StatusCode(201, new ApiResponse<string>(201, "Course created successfully"));
//        }

//        [HttpPut("{id}")]
//        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status200OK)]
//        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status404NotFound)]
//        public async Task<ActionResult<ApiResponse<string>>> UpdateCourse(int id, CourseUpdateDto dto)
//        {
//            try
//            {
//                await _courseService.UpdateAsync(id, dto);
//                return Ok(new ApiResponse<string>(200, "Course updated successfully"));
//            }
//            catch (Exception ex)
//            {
//                return NotFound(new ApiResponse<string>(404, ex.Message));
//            }
//        }

//        [HttpDelete("{id}")]
//        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status200OK)]
//        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status404NotFound)]
//        public async Task<ActionResult<ApiResponse<string>>> DeleteCourse(int id)
//        {
//            try
//            {
//                await _courseService.DeleteAsync(id);
//                return Ok(new ApiResponse<string>(200, "Course deleted successfully"));
//            }
//            catch (Exception ex)
//            {
//                return NotFound(new ApiResponse<string>(404, ex.Message));
//            }
//        }

//        // 🔹 علاقات إضافية

//        [HttpGet("{id}/lessons")]
//        [ProducesResponseType(typeof(ApiResponse<IEnumerable<LessonReadDto>>), StatusCodes.Status200OK)]
//        public async Task<ActionResult<ApiResponse<IEnumerable<LessonReadDto>>>> GetLessonsByCourseId(int id)
//        {
//            var lessons = await _courseService.GetLessonsByCourseIdAsync(id);
//            return Ok(new ApiResponse<IEnumerable<LessonReadDto>>(200, "Lessons retrieved successfully", lessons));
//        }

//        [HttpGet("{id}/students")]
//        [ProducesResponseType(typeof(ApiResponse<IEnumerable<StudentReadDto>>), StatusCodes.Status200OK)]
//        public async Task<ActionResult<ApiResponse<IEnumerable<StudentReadDto>>>> GetStudentsByCourseId(int id)
//        {
//            var students = await _courseService.GetStudentsByCourseIdAsync(id);
//            return Ok(new ApiResponse<IEnumerable<StudentReadDto>>(200, "Students retrieved successfully", students));
//        }

//        [HttpGet("{id}/exams")]
//        [ProducesResponseType(typeof(ApiResponse<IEnumerable<ExamReadDto>>), StatusCodes.Status200OK)]
//        public async Task<ActionResult<ApiResponse<IEnumerable<ExamReadDto>>>> GetExamsByCourseId(int id)
//        {
//            var exams = await _courseService.GetExamsByCourseIdAsync(id);
//            return Ok(new ApiResponse<IEnumerable<ExamReadDto>>(200, "Exams retrieved successfully", exams));
//        }

//        [HttpGet("{id}/teacher")]
//        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status200OK)]
//        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status404NotFound)]
//        public async Task<ActionResult<ApiResponse<string>>> GetTeacherByCourseId(int id)
//        {
//            var course = await _courseService.GetByIdAsync(id);
//            if (course == null)
//                return NotFound(new ApiResponse<string>(404, "Course not found"));

//            return Ok(new ApiResponse<string>(200, "Teacher retrieved successfully", course.TeacherName));
//        }

//        [HttpGet("{id}/curriculum")]
//        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status200OK)]
//        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status404NotFound)]
//        public async Task<ActionResult<ApiResponse<string>>> GetCurriculumByCourseId(int id)
//        {
//            var course = await _courseService.GetByIdAsync(id);
//            if (course == null)
//                return NotFound(new ApiResponse<string>(404, "Course not found"));

//            return Ok(new ApiResponse<string>(200, "Curriculum retrieved successfully", course.CurriculumTitle));
//        }
//    }
//}