using Microsoft.AspNetCore.Mvc;
using SchoolLearningSystem.API.Responses;
using SchoolLearningSystem.Applicationf.DTOs.MemorizeSession;
using SchoolLearningSystem.Applicationf.Interfaces;

namespace SchoolLearningSystem.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MemorizeSessionController : ControllerBase
    {
        private readonly IMemorizeService _memorizeService;

        public MemorizeSessionController(IMemorizeService memorizeService)
        {
            _memorizeService = memorizeService;
        }

        // 🔹 CRUD الأساسي (موروث من IBaseService)

        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<MemorizeSessionReadDto>>), StatusCodes.Status200OK)]
        public async Task<ActionResult<ApiResponse<IEnumerable<MemorizeSessionReadDto>>>> GetAll()
        {
            var data = await _memorizeService.GetAllAsync();
            return Ok(new ApiResponse<IEnumerable<MemorizeSessionReadDto>>(200, "Sessions retrieved successfully", data));
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ApiResponse<MemorizeSessionReadDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApiResponse<MemorizeSessionReadDto>>> GetById(int id)
        {
            var data = await _memorizeService.GetByIdAsync(id);
            if (data == null)
                return NotFound(new ApiResponse(404, "Session not found"));

            return Ok(new ApiResponse<MemorizeSessionReadDto>(200, "Session retrieved successfully", data));
        }

        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<MemorizeSessionReadDto>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ApiResponse<MemorizeSessionReadDto>>> Add([FromBody] MemorizeSessionCreateDto dto)
        {
            // 🔴 فجوة حرجة موثقة: هذا ما زال إنشاءً "أعمى"، غير مربوط بـ
            // ISrsService.GetDueQuestionsForSessionAsync. الـ Use Case الموثق
            // (StartNewSessionAsync) غير مبني بعد بـ IMemorizeService — أضفه هناك أولاً.
            var created = await _memorizeService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = created.Id },
                new ApiResponse<MemorizeSessionReadDto>(201, "Session created successfully", created));
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApiResponse>> Update(int id, [FromBody] MemorizeSessionUpdateDto dto)
        {
            await _memorizeService.UpdateAsync(id, dto);
            return Ok(new ApiResponse(200, "Session updated successfully"));
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApiResponse>> Delete(int id)
        {
            await _memorizeService.DeleteAsync(id);
            return Ok(new ApiResponse(200, "Session deleted successfully"));
        }

        // 🔹 عمليات مخصصة لجلسات المراجعة (Business Logic) — مطابقة حرفياً لـ IMemorizeService

        /// <summary>
        /// الجلسة النشطة الحالية للطالب (إن وُجدت) — نقطة البداية عند فتح شاشة المراجعة
        /// </summary>
        [HttpGet("student/{studentId}/active")]
        [ProducesResponseType(typeof(ApiResponse<MemorizeSessionReadDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApiResponse<MemorizeSessionReadDto>>> GetActiveSession(int studentId)
        {
            var data = await _memorizeService.GetActiveSessionByStudentIdAsync(studentId);
            if (data == null)
                return NotFound(new ApiResponse(404, "No active session found for this student"));

            return Ok(new ApiResponse<MemorizeSessionReadDto>(200, "Active session retrieved successfully", data));
        }

        /// <summary>
        /// سجل كل جلسات الطالب (تاريخ المراجعات) لعرضه بلوحة تحكمه
        /// </summary>
        [HttpGet("student/{studentId}/history")]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<MemorizeSessionReadDto>>), StatusCodes.Status200OK)]
        public async Task<ActionResult<ApiResponse<IEnumerable<MemorizeSessionReadDto>>>> GetHistory(int studentId)
        {
            var data = await _memorizeService.GetSessionHistoryByStudentIdAsync(studentId);
            return Ok(new ApiResponse<IEnumerable<MemorizeSessionReadDto>>(200, "Session history retrieved successfully", data));
        }

        /// <summary>
        /// جلسة معيّنة مع كل تفاصيل إجاباتها (مراجعة كاملة بعد الانتهاء)
        /// </summary>
        [HttpGet("{id}/with-answers")]
        [ProducesResponseType(typeof(ApiResponse<MemorizeSessionReadDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<ApiResponse<MemorizeSessionReadDto>>> GetWithAnswers(int id)
        {
            var data = await _memorizeService.GetSessionWithAnswersAsync(id);
            return Ok(new ApiResponse<MemorizeSessionReadDto>(200, "Session with answers retrieved successfully", data));
        }

        /// <summary>
        /// إنهاء الجلسة الحالية: IsCompleted=true + تسجيل CompletedAt ونسبة النجاح
        /// </summary>
        [HttpPatch("{id}/complete")]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApiResponse>> CompleteSession(int id, [FromQuery] double successRate)
        {
            await _memorizeService.CompleteSessionAsync(id, successRate);
            return Ok(new ApiResponse(200, "Session completed successfully"));
        }

        // API/Controllers/MemorizeSessionsController.cs

        [HttpPost("/api/students/{studentId}/memorize-sessions/start")]
        public async Task<ActionResult<ApiResponse<MemorizeSessionStartResultDto>>> StartSession(int studentId)
        {
            var result = await _memorizeService.StartNewSessionAsync(studentId);
            return Ok(ApiResponse<MemorizeSessionStartResultDto>.Success(result));
        }
    }
}
