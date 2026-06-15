

using Microsoft.AspNetCore.Mvc;
using SchoolLearningSystem.API.Responses;
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
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApiResponse<CourseReadDto>>> GetCourseById(int id)
        {
            // أبقينا الفحص هنا حالياً (Hybrid Approach)
            var course = await _courseService.GetByIdAsync(id);
            if (course == null)
                return NotFound(new ApiResponse<string>(404, "Course not found"));

            return Ok(new ApiResponse<CourseReadDto>(200, "Course retrieved successfully", course));
        }

        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ApiResponse<string>>> AddCourse(CourseCreateDto dto)
        {
            // التحقق من صحة البيانات يظل في الكنترولر لأنه جزء من الـ API Contract
            if (!ModelState.IsValid)
                return BadRequest(new ApiResponse<string>(400, "Invalid input data"));

            await _courseService.CreateAsync(dto);
            return StatusCode(201, new ApiResponse<string>(201, "Course created successfully"));
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApiResponse<string>>> UpdateCourse(int id, CourseUpdateDto dto)
        {
            // تم حذف الـ try-catch. أي خطأ سيحدث هنا (مثل Course Not Found)
            // سيتم التقاطه بواسطة الـ ExceptionMiddleware.
            await _courseService.UpdateAsync(id, dto);
            return Ok(new ApiResponse<string>(200, "Course updated successfully"));
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApiResponse<string>>> DeleteCourse(int id)
        {
            // تم حذف الـ try-catch.
            await _courseService.DeleteAsync(id);
            return Ok(new ApiResponse<string>(200, "Course deleted successfully"));
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
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApiResponse<string>>> GetTeacherByCourseId(int id)
        {
            var course = await _courseService.GetByIdAsync(id);
            if (course == null)
                return NotFound(new ApiResponse<string>(404, "Course not found"));

            return Ok(new ApiResponse<string>(200, "Teacher retrieved successfully", course.TeacherName));
        }

        [HttpGet("{id}/curriculum")]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApiResponse<string>>> GetCurriculumByCourseId(int id)
        {
            var course = await _courseService.GetByIdAsync(id);
            if (course == null)
                return NotFound(new ApiResponse<string>(404, "Course not found"));

            return Ok(new ApiResponse<string>(200, "Curriculum retrieved successfully", course.CurriculumTitle));
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