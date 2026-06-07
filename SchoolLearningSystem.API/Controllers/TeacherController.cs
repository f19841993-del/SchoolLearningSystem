using Microsoft.AspNetCore.Mvc;
using SchoolLearningSystem.Applicationf.Interfaces;
using SchoolLearningSystem.Applicationf.DTOs;
using SchoolLearningSystem.Applicationf.DTOs.CourseDto;
using SchoolLearningSystem.Applicationf.DTOs.Lesson;

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
        /// <summary>
        /// يرجع كل المدرسين المسجلين بالنظام
        /// </summary>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<TeacherDto>>> GetAllTeachers()
        {
            var teachers = await _teacherService.GetAllTeachersAsync();
            return Ok(teachers);
        }

        /// <summary>
        /// يرجع بيانات مدرس محدد حسب الـ Id
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<TeacherDto>> GetTeacherById(int id)
        {
            var teacher = await _teacherService.GetTeacherByIdAsync(id);
            if (teacher == null) return NotFound();
            return Ok(teacher);
        }

        /// <summary>
        /// إضافة مدرس جديد للنظام
        /// </summary>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult> AddTeacher(TeacherDto dto)
        {
            await _teacherService.AddTeacherAsync(dto);
            return CreatedAtAction(nameof(GetTeacherById), new { id = dto.Id }, dto);
        }

        /// <summary>
        /// تحديث بيانات مدرس موجود
        /// </summary>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> UpdateTeacher(int id, TeacherDto dto)
        {
            if (id != dto.Id) return BadRequest();
            await _teacherService.UpdateTeacherAsync(dto);
            return NoContent();
        }

        /// <summary>
        /// حذف مدرس من النظام
        /// </summary>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult> DeleteTeacher(int id)
        {
            await _teacherService.DeleteTeacherAsync(id);
            return NoContent();
        }

        // 🔹 علاقات إضافية
        /// <summary>
        /// يرجع الكورسات المرتبطة بالمدرس
        /// </summary>
        [HttpGet("{id}/courses")]
        public async Task<ActionResult<IEnumerable<CourseDto>>> GetCoursesByTeacherId(int id)
        {
            var courses = await _teacherService.GetCoursesByTeacherIdAsync(id);
            return Ok(courses);
        }

        /// <summary>
        /// يرجع الدروس المرتبطة بالمدرس
        /// </summary>
        [HttpGet("{id}/lessons")]
        public async Task<ActionResult<IEnumerable<LessonDto>>> GetLessonsByTeacherId(int id)
        {
            var lessons = await _teacherService.GetLessonsByTeacherIdAsync(id);
            return Ok(lessons);
        }

        // 🔹 إحصائيات
        /// <summary>
        /// يرجع عدد الكورسات اللي يدرّسها المدرس
        /// </summary>
        [HttpGet("{id}/total-courses")]
        public async Task<ActionResult<int>> GetTotalCoursesByTeacherId(int id)
        {
            var total = await _teacherService.GetTotalCoursesByTeacherIdAsync(id);
            return Ok(total);
        }

        /// <summary>
        /// يرجع عدد الدروس اللي يدرّسها المدرس
        /// </summary>
        [HttpGet("{id}/total-lessons")]
        public async Task<ActionResult<int>> GetTotalLessonsByTeacherId(int id)
        {
            var total = await _teacherService.GetTotalLessonsByTeacherIdAsync(id);
            return Ok(total);
        }
    }
}
