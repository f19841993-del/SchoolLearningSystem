using Microsoft.AspNetCore.Mvc;
using SchoolLearningSystem.API.Responses;
using SchoolLearningSystem.Applicationf.DTOs.StudentAnswer;
using SchoolLearningSystem.Applicationf.Interfaces;

namespace SchoolLearningSystem.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StudentAnswerDetailController : ControllerBase
    {
        private readonly IStudentAnswerDetailService _service;

        public StudentAnswerDetailController(IStudentAnswerDetailService service)
        {
            _service = service;
        }

        // 🔹 CRUD الأساسي

        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<StudentAnswerDetailReadDto>>), StatusCodes.Status200OK)]
        public async Task<ActionResult<ApiResponse<IEnumerable<StudentAnswerDetailReadDto>>>> GetAll()
        {
            var data = await _service.GetAllAsync();
            return Ok(new ApiResponse<IEnumerable<StudentAnswerDetailReadDto>>(200, "Answers retrieved successfully", data));
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ApiResponse<StudentAnswerDetailReadDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApiResponse<StudentAnswerDetailReadDto>>> GetById(int id)
        {
            var data = await _service.GetByIdAsync(id);
            if (data == null)
                return NotFound(new ApiResponse(404, "Answer record not found"));

            return Ok(new ApiResponse<StudentAnswerDetailReadDto>(200, "Answer retrieved successfully", data));
        }

        /// <summary>
        /// ✅ استعادة: هذا الـ Endpoint اتحذف بمحادثة سابقة اعتماداً على ملاحظة قديمة
        /// بـ api_contract.md تقول "لا POST مباشر هنا". لكن IStudentAnswerDetailService
        /// الفعلية تنص صراحة: "تسجيل إجابة جديدة يتم عبر CreateAsync الموروثة مباشرة -
        /// لا حاجة لدالة مخصصة" — يعني الـ POST مقصود ومسموح به، والملاحظة القديمة
        /// كانت من مسودة توثيق أقدم تجاوزها القرار الفعلي بالكود. رجّعته.
        /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<StudentAnswerDetailReadDto>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ApiResponse<StudentAnswerDetailReadDto>>> Add([FromBody] StudentAnswerDetailCreateDto dto)
        {
            var created = await _service.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = created.Id },
                new ApiResponse<StudentAnswerDetailReadDto>(201, "Answer recorded successfully", created));
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApiResponse>> Update(int id, [FromBody] StudentAnswerDetailUpdateDto dto)
        {
            await _service.UpdateAsync(id, dto);
            return Ok(new ApiResponse(200, "Answer updated successfully"));
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApiResponse>> Delete(int id)
        {
            await _service.DeleteAsync(id);
            return Ok(new ApiResponse(200, "Answer deleted successfully"));
        }

        // 🔹 علاقات إضافية — مطابقة حرفياً لـ IStudentAnswerDetailService
        // ⚠️ تصحيح أسماء: GetByStudentIdAsync/GetByQuestionIdAsync/GetRecentAnswersAsync
        // غير موجودة — الأسماء الصحيحة أدناه.

        [HttpGet("student/{studentId}")]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<StudentAnswerDetailReadDto>>), StatusCodes.Status200OK)]
        public async Task<ActionResult<ApiResponse<IEnumerable<StudentAnswerDetailReadDto>>>> GetByStudent(int studentId)
        {
            var data = await _service.GetAnswersByStudentIdAsync(studentId);
            return Ok(new ApiResponse<IEnumerable<StudentAnswerDetailReadDto>>(200, "Answers by student retrieved successfully", data));
        }

        [HttpGet("question/{questionId}")]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<StudentAnswerDetailReadDto>>), StatusCodes.Status200OK)]
        public async Task<ActionResult<ApiResponse<IEnumerable<StudentAnswerDetailReadDto>>>> GetByQuestion(int questionId)
        {
            var data = await _service.GetAnswersByQuestionIdAsync(questionId);
            return Ok(new ApiResponse<IEnumerable<StudentAnswerDetailReadDto>>(200, "Answers by question retrieved successfully", data));
        }

        [HttpGet("recent/{studentId}/{count}")]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<StudentAnswerDetailReadDto>>), StatusCodes.Status200OK)]
        public async Task<ActionResult<ApiResponse<IEnumerable<StudentAnswerDetailReadDto>>>> GetRecent(int studentId, int count)
        {
            var data = await _service.GetRecentAnswersByStudentIdAsync(studentId, count);
            return Ok(new ApiResponse<IEnumerable<StudentAnswerDetailReadDto>>(200, "Recent answers retrieved successfully", data));
        }

        [HttpGet("incorrect/{studentId}/{lessonId}")]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<StudentAnswerDetailReadDto>>), StatusCodes.Status200OK)]
        public async Task<ActionResult<ApiResponse<IEnumerable<StudentAnswerDetailReadDto>>>> GetIncorrectAnswers(int studentId, int lessonId)
        {
            var data = await _service.GetIncorrectAnswersByStudentIdAsync(studentId, lessonId);
            return Ok(new ApiResponse<IEnumerable<StudentAnswerDetailReadDto>>(200, "Incorrect answers retrieved successfully", data));
        }
    }
}
