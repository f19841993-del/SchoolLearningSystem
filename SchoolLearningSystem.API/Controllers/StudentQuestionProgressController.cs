using Microsoft.AspNetCore.Mvc;
using SchoolLearningSystem.API.Responses;
using SchoolLearningSystem.Applicationf.DTOs.StudentQuestionProgress;
using SchoolLearningSystem.Applicationf.Interfaces;

namespace SchoolLearningSystem.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StudentQuestionProgressController : ControllerBase
    {
        private readonly IStudentQuestionProgressService _service;

        public StudentQuestionProgressController(IStudentQuestionProgressService service)
        {
            _service = service;
        }

        // 🔹 CRUD الأساسي

        [HttpGet("{studentId}/{questionId}")]
        [ProducesResponseType(typeof(ApiResponse<StudentQuestionProgressReadDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApiResponse<StudentQuestionProgressReadDto>>> GetByStudentAndQuestion(int studentId, int questionId)
        {
            var data = await _service.GetByStudentAndQuestionAsync(studentId, questionId);
            if (data == null)
                return NotFound(new ApiResponse<string>(404, "Progress record not found"));

            return Ok(new ApiResponse<StudentQuestionProgressReadDto>(200, "Progress record retrieved successfully", data));
        }

        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ApiResponse<string>>> Add(StudentQuestionProgressCreateDto dto)
        {
           
            await _service.AddProgressAsync(dto);
            return StatusCode(201, new ApiResponse<string>(201, "Progress record created successfully"));
        }

        [HttpPut]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApiResponse<string>>> Update(StudentQuestionProgressUpdateDto dto)
        {
            // تم الحذف: لا يوجد if (!ModelState.IsValid) هنا!

            // الـ Service ستقوم بالبحث والتحديث، وإذا لم تجد السجل سترمي NotFoundException
            // وإذا كان هناك خطأ في البيانات (كأن يكون Interval سالباً)، سترمي CustomValidationException
            await _service.UpdateProgressAsync(dto);

            return Ok(new ApiResponse<string>(200, "Progress record updated successfully"));
        }

        // 🔹 علاقات إضافية (Custom Queries)

        [HttpGet("due/{studentId}")]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<StudentQuestionProgressReadDto>>), StatusCodes.Status200OK)]
        public async Task<ActionResult<ApiResponse<IEnumerable<StudentQuestionProgressReadDto>>>> GetDueQuestions(int studentId)
        {
            var data = await _service.GetDueQuestionsAsync(studentId);
            return Ok(new ApiResponse<IEnumerable<StudentQuestionProgressReadDto>>(200, "Due questions retrieved successfully", data));
        }

        [HttpGet("student/{studentId}")]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<StudentQuestionProgressReadDto>>), StatusCodes.Status200OK)]
        public async Task<ActionResult<ApiResponse<IEnumerable<StudentQuestionProgressReadDto>>>> GetByStudent(int studentId)
        {
            var data = await _service.GetByStudentIdAsync(studentId);
            return Ok(new ApiResponse<IEnumerable<StudentQuestionProgressReadDto>>(200, "Progress records retrieved successfully", data));
        }
    }
}