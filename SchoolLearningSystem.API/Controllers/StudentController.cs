using Microsoft.AspNetCore.Mvc;
using SchoolLearningSystem.Applicationf.Interfaces;
using SchoolLearningSystem.Applicationf.DTOs;
using SchoolLearningSystem.Applicationf.DTOs.CourseDto;
using SchoolLearningSystem.Applicationf.DTOs.MemorizeSession;

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
        /// <summary>
        /// يرجع كل الطلاب المسجلين بالنظام
        /// </summary>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<StudentDto>>> GetAllStudents()
        {
            var students = await _studentService.GetAllStudentsAsync();
            return Ok(students);
        }

        /// <summary>
        /// يرجع بيانات طالب محدد حسب الـ Id
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<StudentDto>> GetStudentById(int id)
        {
            var student = await _studentService.GetStudentByIdAsync(id);
            if (student == null) return NotFound();
            return Ok(student);
        }

        /// <summary>
        /// إضافة طالب جديد للنظام
        /// </summary>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult> AddStudent(StudentDto dto)
        {
            await _studentService.AddStudentAsync(dto);
            return CreatedAtAction(nameof(GetStudentById), new { id = dto.Id }, dto);
        }

        /// <summary>
        /// تحديث بيانات طالب موجود
        /// </summary>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> UpdateStudent(int id, StudentDto dto)
        {
            if (id != dto.Id) return BadRequest();
            await _studentService.UpdateStudentAsync(dto);
            return NoContent();
        }

        /// <summary>
        /// حذف طالب من النظام
        /// </summary>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult> DeleteStudent(int id)
        {
            await _studentService.DeleteStudentAsync(id);
            return NoContent();
        }

        // 🔹 علاقات إضافية
        /// <summary>
        /// يرجع الكورسات المرتبطة بالطالب
        /// </summary>
        [HttpGet("{id}/courses")]
        public async Task<ActionResult<IEnumerable<CourseDto>>> GetCoursesByStudentId(int id)
        {
            var courses = await _studentService.GetCoursesByStudentIdAsync(id);
            return Ok(courses);
        }

        /// <summary>
        /// يرجع نتائج الطالب
        /// </summary>
        [HttpGet("{id}/results")]
        public async Task<ActionResult<IEnumerable<ResultDto>>> GetResultsByStudentId(int id)
        {
            var results = await _studentService.GetResultsByStudentIdAsync(id);
            return Ok(results);
        }

        /// <summary>
        /// يرجع جلسات الحفظ الخاصة بالطالب
        /// </summary>
        [HttpGet("{id}/memorize-sessions")]
        public async Task<ActionResult<IEnumerable<MemorizeSessionDto>>> GetMemorizeSessionsByStudentId(int id)
        {
            var sessions = await _studentService.GetMemorizeSessionsByStudentIdAsync(id);
            return Ok(sessions);
        }

        // 🔹 إحصائيات
        /// <summary>
        /// يرجع معدل درجات الطالب
        /// </summary>
        [HttpGet("{id}/average-score")]
        public async Task<ActionResult<double>> GetAverageScoreByStudentId(int id)
        {
            var avg = await _studentService.GetAverageScoreByStudentIdAsync(id);
            return Ok(avg);
        }

        /// <summary>
        /// يرجع عدد الكورسات اللي مسجل بيها الطالب
        /// </summary>
        [HttpGet("{id}/total-courses")]
        public async Task<ActionResult<int>> GetTotalCoursesByStudentId(int id)
        {
            var total = await _studentService.GetTotalCoursesByStudentIdAsync(id);
            return Ok(total);
        }
    }
}
