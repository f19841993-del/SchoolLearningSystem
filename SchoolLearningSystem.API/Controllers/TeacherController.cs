using Microsoft.AspNetCore.Mvc;
using SchoolLearningSystem.Applicationf.Interfaces;
using SchoolLearningSystem.Applicationf.DTOs.Teacher;
using SchoolLearningSystem.Applicationf.DTOs.CourseDto;
using SchoolLearningSystem.Applicationf.DTOs.Lesson;
using SchoolLearningSystem.API.Responses;

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
        /// يرجع كل المدرسين
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAllTeachers()
        {
            var teachers = await _teacherService.GetAllTeachersAsync();
            return Ok(new ApiResponse<IEnumerable<TeacherReadDto>>(200, "Teachers retrieved successfully", teachers));
        }

        /// <summary>
        /// يرجع بيانات مدرس محدد حسب الـ Id
        /// </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetTeacherById(int id)
        {
            var teacher = await _teacherService.GetTeacherByIdAsync(id);
            if (teacher == null)
                return NotFound(new ApiResponse<string>(404, "Teacher not found"));

            return Ok(new ApiResponse<TeacherReadDto>(200, "Teacher retrieved successfully", teacher));
        }

        /// <summary>
        /// إضافة مدرس جديد
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> AddTeacher(TeacherCreateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ApiResponse<string>(400, "Invalid input data"));

            await _teacherService.AddTeacherAsync(dto);
            return StatusCode(201, new ApiResponse<string>(201, "Teacher created successfully"));
        }

        /// <summary>
        /// تحديث بيانات مدرس موجود
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTeacher(int id, TeacherUpdateDto dto)
        {
            try
            {
                await _teacherService.UpdateTeacherAsync(id, dto);
                return Ok(new ApiResponse<string>(200, "Teacher updated successfully"));
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new ApiResponse<string>(404, "Teacher not found"));
            }
        }

        /// <summary>
        /// حذف مدرس
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTeacher(int id)
        {
            try
            {
                await _teacherService.DeleteTeacherAsync(id);
                return Ok(new ApiResponse<string>(200, "Teacher deleted successfully"));
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new ApiResponse<string>(404, "Teacher not found"));
            }
        }

        // 🔹 علاقات إضافية

        /// <summary>
        /// يرجع الكورسات المرتبطة بالمدرس
        /// </summary>
        [HttpGet("{teacherId}/courses")]
        public async Task<IActionResult> GetCoursesByTeacherId(int teacherId)
        {
            var courses = await _teacherService.GetCoursesByTeacherIdAsync(teacherId);
            return Ok(new ApiResponse<IEnumerable<CourseReadDto>>(200, "Courses retrieved successfully", courses));
        }

        /// <summary>
        /// يرجع الدروس المرتبطة بالمدرس
        /// </summary>
        [HttpGet("{teacherId}/lessons")]
        public async Task<IActionResult> GetLessonsByTeacherId(int teacherId)
        {
            var lessons = await _teacherService.GetLessonsByTeacherIdAsync(teacherId);
            return Ok(new ApiResponse<IEnumerable<LessonReadDto>>(200, "Lessons retrieved successfully", lessons));
        }

        // 🔹 إحصائيات

        /// <summary>
        /// يرجع عدد الكورسات اللي يدرّسها المدرس
        /// </summary>
        [HttpGet("{teacherId}/total-courses")]
        public async Task<IActionResult> GetTotalCoursesByTeacherId(int teacherId)
        {
            var total = await _teacherService.GetTotalCoursesByTeacherIdAsync(teacherId);
            return Ok(new ApiResponse<int>(200, "Total courses retrieved successfully", total));
        }

        /// <summary>
        /// يرجع عدد الدروس اللي يدرّسها المدرس
        /// </summary>
        [HttpGet("{teacherId}/total-lessons")]
        public async Task<IActionResult> GetTotalLessonsByTeacherId(int teacherId)
        {
            var total = await _teacherService.GetTotalLessonsByTeacherIdAsync(teacherId);
            return Ok(new ApiResponse<int>(200, "Total lessons retrieved successfully", total));
        }
    }
}
