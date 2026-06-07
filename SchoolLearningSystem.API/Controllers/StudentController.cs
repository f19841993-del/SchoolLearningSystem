using Microsoft.AspNetCore.Mvc;
using SchoolLearningSystem.Applicationf.Interfaces;
using SchoolLearningSystem.Applicationf.DTOs.Student;
using SchoolLearningSystem.API.Responses;
using SchoolLearningSystem.Applicationf.DTOs.CourseDto;
using SchoolLearningSystem.Applicationf.DTOs.Result;
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
        /// يرجع كل الطلاب الموجودين بالنظام
        /// </summary>
        /// <response code="200">تم جلب الطلاب بنجاح</response>
        [HttpGet]
        public async Task<IActionResult> GetAllStudents()
        {
            var students = await _studentService.GetAllStudentsAsync();
            return Ok(new ApiResponse<IEnumerable<StudentReadDto>>(200, "Students retrieved successfully", students));
        }

        /// <summary>
        /// يرجع بيانات طالب محدد حسب الـ Id
        /// </summary>
        /// <response code="200">تم جلب الطالب بنجاح</response>
        /// <response code="404">الطالب غير موجود</response>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetStudentById(int id)
        {
            var student = await _studentService.GetStudentByIdAsync(id);
            if (student == null)
                return NotFound(new ApiResponse<string>(404, "Student not found"));

            return Ok(new ApiResponse<StudentReadDto>(200, "Student retrieved successfully", student));
        }

        /// <summary>
        /// إضافة طالب جديد
        /// </summary>
        /// <response code="201">تم إنشاء الطالب بنجاح</response>
        /// <response code="400">خطأ في البيانات المدخلة</response>
        [HttpPost]
        public async Task<IActionResult> AddStudent(StudentCreateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ApiResponse<string>(400, "Invalid input data"));

            await _studentService.AddStudentAsync(dto);
            return StatusCode(201, new ApiResponse<string>(201, "Student created successfully"));
        }

        /// <summary>
        /// تحديث بيانات طالب موجود
        /// </summary>
        /// <response code="200">تم تحديث الطالب بنجاح</response>
        /// <response code="404">الطالب غير موجود</response>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateStudent(int id, StudentUpdateDto dto)
        {
            try
            {
                await _studentService.UpdateStudentAsync(id, dto);
                return Ok(new ApiResponse<string>(200, "Student updated successfully"));
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new ApiResponse<string>(404, "Student not found"));
            }
        }

        /// <summary>
        /// حذف طالب
        /// </summary>
        /// <response code="200">تم حذف الطالب بنجاح</response>
        /// <response code="404">الطالب غير موجود</response>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStudent(int id)
        {
            try
            {
                await _studentService.DeleteStudentAsync(id);
                return Ok(new ApiResponse<string>(200, "Student deleted successfully"));
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new ApiResponse<string>(404, "Student not found"));
            }
        }

        // 🔹 علاقات إضافية

        /// <summary>
        /// يرجع الكورسات المرتبطة بالطالب
        /// </summary>
        [HttpGet("{studentId}/courses")]
        public async Task<IActionResult> GetCoursesByStudentId(int studentId)
        {
            var courses = await _studentService.GetCoursesByStudentIdAsync(studentId);
            return Ok(new ApiResponse<IEnumerable<CourseDto>>(200, "Courses retrieved successfully", courses));
        }

        /// <summary>
        /// يرجع نتائج الطالب
        /// </summary>
        [HttpGet("{studentId}/results")]
        public async Task<IActionResult> GetResultsByStudentId(int studentId)
        {
            var results = await _studentService.GetResultsByStudentIdAsync(studentId);
            return Ok(new ApiResponse<IEnumerable<ResultReadDto>>(200, "Results retrieved successfully", results));
        }

        /// <summary>
        /// يرجع جلسات المراجعة الخاصة بالطالب
        /// </summary>
        [HttpGet("{studentId}/memorize-sessions")]
        public async Task<IActionResult> GetMemorizeSessionsByStudentId(int studentId)
        {
            var sessions = await _studentService.GetMemorizeSessionsByStudentIdAsync(studentId);
            return Ok(new ApiResponse<IEnumerable<MemorizeSessionReadDto>>(200, "Memorize sessions retrieved successfully", sessions));
        }

        // 🔹 إحصائيات

        /// <summary>
        /// يرجع معدل درجات الطالب
        /// </summary>
        [HttpGet("{studentId}/average-score")]
        public async Task<IActionResult> GetAverageScoreByStudentId(int studentId)
        {
            var avg = await _studentService.GetAverageScoreByStudentIdAsync(studentId);
            return Ok(new ApiResponse<double>(200, "Average score retrieved successfully", avg));
        }

        /// <summary>
        /// يرجع عدد الكورسات المشترك بها الطالب
        /// </summary>
        [HttpGet("{studentId}/total-courses")]
        public async Task<IActionResult> GetTotalCoursesByStudentId(int studentId)
        {
            var total = await _studentService.GetTotalCoursesByStudentIdAsync(studentId);
            return Ok(new ApiResponse<int>(200, "Total courses retrieved successfully", total));
        }
    }
}
