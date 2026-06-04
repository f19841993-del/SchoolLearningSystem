using Microsoft.AspNetCore.Mvc;
using SchoolLearningSystem.Applicationf.Interfaces;
using SchoolLearningSystem.Applicationf.DTOs;

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
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CourseDto>>> GetAllCourses()
        {
            var courses = await _courseService.GetAllCoursesAsync();
            return Ok(courses);
        }

        /// <summary>
        /// يرجع بيانات كورس محدد حسب الـ Id
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<CourseDto>> GetCourseById(int id)
        {
            var course = await _courseService.GetCourseByIdAsync(id);
            if (course == null) return NotFound();
            return Ok(course);
        }

        /// <summary>
        /// إضافة كورس جديد للنظام
        /// </summary>
        [HttpPost]
        [HttpPost("{teacherId}")]
        public async Task<ActionResult> AddCourse(int teacherId, CourseDto dto)
        {
            await _courseService.AddCourseAsync(dto, teacherId);
            return CreatedAtAction(nameof(GetCourseById), new { id = dto.Id }, dto);
        }


        /// <summary>
        /// تحديث بيانات كورس موجود
        /// </summary>
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateCourse(int id, CourseDto dto)
        {
            if (id != dto.Id) return BadRequest();
            await _courseService.UpdateCourseAsync(dto);
            return NoContent();
        }

        /// <summary>
        /// حذف كورس من النظام
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteCourse(int id)
        {
            await _courseService.DeleteCourseAsync(id);
            return NoContent();
        }

        // 🔹 علاقات إضافية
        /// <summary>
        /// يرجع الدروس المرتبطة بالكورس
        /// </summary>
        [HttpGet("{id}/lessons")]
        public async Task<ActionResult<IEnumerable<LessonDto>>> GetLessonsByCourseId(int id)
        {
            var lessons = await _courseService.GetLessonsByCourseIdAsync(id);
            return Ok(lessons);
        }

        /// <summary>
        /// يرجع الطلاب المرتبطين بالكورس
        /// </summary>
        [HttpGet("{id}/students")]
        public async Task<ActionResult<IEnumerable<StudentDto>>> GetStudentsByCourseId(int id)
        {
            var students = await _courseService.GetStudentsByCourseIdAsync(id);
            return Ok(students);
        }

        /// <summary>
        /// يرجع المدرس المسؤول عن الكورس
        /// </summary>
        [HttpGet("{id}/teacher")]
        public async Task<ActionResult<string>> GetTeacherByCourseId(int id)
        {
            var course = await _courseService.GetCourseByIdAsync(id);
            if (course == null) return NotFound();

            // ✅ لأن CourseDto يحتوي TeacherName
            return Ok(course.TeacherName);
        }



        [HttpGet("{id}/curriculum")]
        public async Task<ActionResult<string>> GetCurriculumByCourseId(int id)
        {
            var course = await _courseService.GetCourseByIdAsync(id);
            if (course == null) return NotFound();

            // ✅ ترجع عنوان المنهج مباشرة من CourseDto
            return Ok(course.CurriculumTitle);
        }

    }
}
