using Microsoft.AspNetCore.Mvc;
using SchoolLearningSystem.API.Responses;
using SchoolLearningSystem.Applicationf.DTOs.Student;
using SchoolLearningSystem.Applicationf.Interfaces;
using SchoolLearningSystem.Domain.Enums;

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

        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<StudentReadDto>>), StatusCodes.Status200OK)]
        public async Task<ActionResult<ApiResponse<IEnumerable<StudentReadDto>>>> GetAll()
        {
            var students = await _studentService.GetAllAsync();
            return Ok(new ApiResponse<IEnumerable<StudentReadDto>>(200, "Students retrieved successfully", students));
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ApiResponse<StudentReadDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApiResponse<StudentReadDto>>> GetById(int id)
        {
            var student = await _studentService.GetByIdAsync(id);
            if (student == null)
                return NotFound(new ApiResponse(404, "Student not found"));

            return Ok(new ApiResponse<StudentReadDto>(200, "Student retrieved successfully", student));
        }

        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ApiResponse>> Add([FromBody] StudentCreateDto dto)
        {
            // ⚠️ فجوة موثقة (Auth غير مبنية): StudentCreateDto بدون Password حالياً.
            // مؤقت لحين نقله لـ POST /api/auth/register/student
            await _studentService.CreateAsync(dto);
            return StatusCode(201, new ApiResponse(201, "Student created successfully"));
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApiResponse>> Update(int id, [FromBody] StudentUpdateDto dto)
        {
            await _studentService.UpdateAsync(id, dto);
            return Ok(new ApiResponse(200, "Student updated successfully"));
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApiResponse>> Delete(int id)
        {
            await _studentService.DeleteAsync(id);
            return Ok(new ApiResponse(200, "Student deleted successfully"));
        }

        // 🔹 علاقات إضافية (Business Logic) — مطابقة حرفياً لـ IStudentService

        /// <summary>
        /// طلاب مرحلة دراسية معيّنة
        /// </summary>
        [HttpGet("grade/{gradeLevel}")]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<StudentReadDto>>), StatusCodes.Status200OK)]
        public async Task<ActionResult<ApiResponse<IEnumerable<StudentReadDto>>>> GetByGradeLevel(GradeLevel gradeLevel)
        {
            var data = await _studentService.GetStudentsByGradeLevelAsync(gradeLevel);
            return Ok(new ApiResponse<IEnumerable<StudentReadDto>>(200, "Students retrieved successfully", data));
        }

        /// <summary>
        /// الطالب مع كل بيانات تقدمه (لمحرك SRS / لوحة الأداء الشخصية)
        /// </summary>
        [HttpGet("{id}/progress")]
        [ProducesResponseType(typeof(ApiResponse<StudentReadDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<ApiResponse<StudentReadDto>>> GetWithProgress(int id)
        {
            var data = await _studentService.GetStudentWithProgressAsync(id);
            return Ok(new ApiResponse<StudentReadDto>(200, "Student progress retrieved successfully", data));
        }

        // ⚠️ حُذفت من هنا نهائياً (كانت تنادي دوال غير موجودة إطلاقاً بـ IStudentService،
        // وهي أصلاً "مصدر حقيقة" بخدمات ثانية) — الفرونت يستخدم مباشرة:
        //   كورسات الطالب  → GET /api/coursestudent/student/{studentId}/courses/paged   (ICourseStudentService)
        //   نتائج الطالب   → GET /api/result/student/{studentId}                        (IResultService)
        //   جلسات الطالب   → GET /api/memorizesession/student/{studentId}/history        (IMemorizeService)
        //   متوسط الدرجات  → GET /api/result/student/{studentId}/average                  (IResultService)
        //   عدد الكورسات   → GET /api/coursestudent/student/{studentId}/courses/count     (ICourseStudentService)
    }
}
