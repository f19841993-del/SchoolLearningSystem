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

        /// <summary>
        /// يرجع كل الكورسات الموجودة بالنظام
        /// </summary>
        /// <response code="200">تم جلب الكورسات بنجاح</response>
        [HttpGet]
        public async Task<IActionResult> GetAllCourses()
        {
            var courses = await _courseService.GetAllCoursesAsync();
            return Ok(new ApiResponse<IEnumerable<CourseReadDto>>(200, "Courses retrieved successfully", courses));
        }

        /// <summary>
        /// يرجع بيانات كورس محدد حسب الـ Id
        /// </summary>
        /// <response code="200">تم جلب الكورس بنجاح</response>
        /// <response code="404">الكورس غير موجود</response>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCourseById(int id)
        {
            var course = await _courseService.GetCourseByIdAsync(id);
            if (course == null)
                return NotFound(new ApiResponse<string>(404, "Course not found"));

            return Ok(new ApiResponse<CourseReadDto>(200, "Course retrieved successfully", course));
        }

        /// <summary>
        /// إضافة كورس جديد للنظام
        /// </summary>
        /// <response code="201">تم إنشاء الكورس بنجاح</response>
        /// <response code="400">خطأ في البيانات المدخلة</response>
        [HttpPost]
        public async Task<IActionResult> AddCourse(CourseCreateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ApiResponse<string>(400, "Invalid input data"));

            await _courseService.AddCourseAsync(dto);
            return StatusCode(201, new ApiResponse<string>(201, "Course created successfully"));
        }

        /// <summary>
        /// تحديث بيانات كورس موجود
        /// </summary>
        /// <response code="200">تم تحديث الكورس بنجاح</response>
        /// <response code="404">الكورس غير موجود</response>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCourse(int id, CourseUpdateDto dto)
        {
            try
            {
                await _courseService.UpdateCourseAsync(id, dto);
                return Ok(new ApiResponse<string>(200, "Course updated successfully"));
            }
            catch (Exception ex)
            {
                return NotFound(new ApiResponse<string>(404, ex.Message));
            }
        }

        /// <summary>
        /// حذف كورس من النظام
        /// </summary>
        /// <response code="200">تم حذف الكورس بنجاح</response>
        /// <response code="404">الكورس غير موجود</response>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCourse(int id)
        {
            try
            {
                await _courseService.DeleteCourseAsync(id);
                return Ok(new ApiResponse<string>(200, "Course deleted successfully"));
            }
            catch (Exception ex)
            {
                return NotFound(new ApiResponse<string>(404, ex.Message));
            }
        }

        // 🔹 علاقات إضافية

        /// <summary>
        /// يرجع الدروس المرتبطة بالكورس
        /// </summary>
        /// <response code="200">تم جلب الدروس بنجاح</response>
        [HttpGet("{id}/lessons")]
        public async Task<IActionResult> GetLessonsByCourseId(int id)
        {
            var lessons = await _courseService.GetLessonsByCourseIdAsync(id);
            return Ok(new ApiResponse<IEnumerable<LessonReadDto>>(200, "Lessons retrieved successfully", lessons));
        }

        /// <summary>
        /// يرجع الطلاب المرتبطين بالكورس
        /// </summary>
        /// <response code="200">تم جلب الطلاب بنجاح</response>
        [HttpGet("{id}/students")]
        public async Task<IActionResult> GetStudentsByCourseId(int id)
        {
            var students = await _courseService.GetStudentsByCourseIdAsync(id);
            return Ok(new ApiResponse<IEnumerable<StudentReadDto>>(200, "Students retrieved successfully", students));
        }

        /// <summary>
        /// يرجع الامتحانات المرتبطة بالكورس
        /// </summary>
        /// <response code="200">تم جلب الامتحانات بنجاح</response>
        [HttpGet("{id}/exams")]
        public async Task<IActionResult> GetExamsByCourseId(int id)
        {
            var exams = await _courseService.GetExamsByCourseIdAsync(id);
            return Ok(new ApiResponse<IEnumerable<ExamReadDto>>(200, "Exams retrieved successfully", exams));
        }

        /// <summary>
        /// يرجع المدرس المسؤول عن الكورس
        /// </summary>
        /// <response code="200">تم جلب المدرس بنجاح</response>
        /// <response code="404">الكورس غير موجود</response>
        [HttpGet("{id}/teacher")]
        public async Task<IActionResult> GetTeacherByCourseId(int id)
        {
            var course = await _courseService.GetCourseByIdAsync(id);
            if (course == null)
                return NotFound(new ApiResponse<string>(404, "Course not found"));

            return Ok(new ApiResponse<string>(200, "Teacher retrieved successfully", course.TeacherName));
        }

        /// <summary>
        /// يرجع عنوان المنهج المرتبط بالكورس
        /// </summary>
        /// <response code="200">تم جلب المنهج بنجاح</response>
        /// <response code="404">الكورس غير موجود</response>
        [HttpGet("{id}/curriculum")]
        public async Task<IActionResult> GetCurriculumByCourseId(int id)
        {
            var course = await _courseService.GetCourseByIdAsync(id);
            if (course == null)
                return NotFound(new ApiResponse<string>(404, "Course not found"));

            return Ok(new ApiResponse<string>(200, "Curriculum retrieved successfully", course.CurriculumTitle));
        }
    }
}
