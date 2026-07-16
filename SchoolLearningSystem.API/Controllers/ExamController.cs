using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolLearningSystem.API.Responses;
using SchoolLearningSystem.Applicationf.DTOs.ExamDto;
using SchoolLearningSystem.Applicationf.DTOs.Lesson;
using SchoolLearningSystem.Applicationf.DTOs.Question;
using SchoolLearningSystem.Applicationf.DTOs.Result;
using SchoolLearningSystem.Applicationf.Interfaces;

namespace SchoolLearningSystem.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ExamController : ControllerBase
    {
        private readonly IExamService _examService;

        public ExamController(IExamService examService)
        {
            _examService = examService;
        }

        // 🔹 CRUD الأساسي

        /// <summary>كل الامتحانات (بيانات وصفية بس، بدون الأسئلة)</summary>
        /// <remarks>الصلاحيات: عام (بدون تسجيل دخول).</remarks>
        [Tags("Exam - الأساسيات (CRUD)")]
        [HttpGet]
        [AllowAnonymous]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<ExamReadDto>>), StatusCodes.Status200OK)]
        public async Task<ActionResult<ApiResponse<IEnumerable<ExamReadDto>>>> GetAll()
        {
            var exams = await _examService.GetAllAsync();
            return Ok(new ApiResponse<IEnumerable<ExamReadDto>>(200, "Exams retrieved successfully", exams));
        }

        /// <summary>امتحان واحد بالتفصيل</summary>
        /// <remarks>الصلاحيات: عام (بدون تسجيل دخول).</remarks>
        /// <response code="404">الامتحان غير موجود</response>
        [Tags("Exam - الأساسيات (CRUD)")]
        [HttpGet("{id}")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(ApiResponse<ExamReadDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApiResponse<ExamReadDto>>> GetById(int id)
        {
            var exam = await _examService.GetByIdAsync(id);
            if (exam == null)
                return NotFound(new ApiResponse(404, "Exam not found"));

            return Ok(new ApiResponse<ExamReadDto>(200, "Exam retrieved successfully", exam));
        }

        /// <summary>إنشاء امتحان جديد</summary>
        /// <remarks>
        /// الصلاحيات: Admin, Teacher فقط.
        ///
        /// مثال Request:
        /// {
        ///   "title": "امتحان الفصل الأول",
        ///   "examType": 1,
        ///   "difficulty": 1,
        ///   "courseId": 1,
        ///   "lessonId": null
        /// }
        /// </remarks>
        /// <response code="400">بيانات غير صالحة</response>
        /// <response code="401">لم يتم تسجيل الدخول</response>
        /// <response code="403">الدور الحالي غير مسموح له</response>
        [Tags("Exam - الأساسيات (CRUD)")]
        [HttpPost]
        [Authorize(Roles = "Admin,Teacher")]
        [ProducesResponseType(typeof(ApiResponse<ExamReadDto>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<ApiResponse<ExamReadDto>>> Add([FromBody] ExamCreateDto dto)
        {
            var createdExam = await _examService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = createdExam.Id },
                new ApiResponse<ExamReadDto>(201, "Exam created successfully", createdExam));
        }

        /// <summary>تعديل امتحان موجود</summary>
        /// <remarks>
        /// الصلاحيات: Admin, Teacher فقط. كل الحقول اختيارية.
        ///
        /// مثال Request:
        /// { "title": "امتحان الفصل الأول (محدّث)" }
        /// </remarks>
        /// <response code="404">الامتحان غير موجود</response>
        /// <response code="401">لم يتم تسجيل الدخول</response>
        /// <response code="403">الدور الحالي غير مسموح له</response>
        [Tags("Exam - الأساسيات (CRUD)")]
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin,Teacher")]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<ApiResponse>> Update(int id, [FromBody] ExamUpdateDto dto)
        {
            await _examService.UpdateAsync(id, dto);
            return Ok(new ApiResponse(200, "Exam updated successfully"));
        }

        /// <summary>حذف امتحان</summary>
        /// <remarks>الصلاحيات: Admin, Teacher فقط. ملاحظة: حذف الامتحان لا يحذف أسئلته.</remarks>
        /// <response code="404">الامتحان غير موجود</response>
        /// <response code="401">لم يتم تسجيل الدخول</response>
        /// <response code="403">الدور الحالي غير مسموح له</response>
        [Tags("Exam - الأساسيات (CRUD)")]
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin,Teacher")]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<ApiResponse>> Delete(int id)
        {
            // ملاحظة معمارية موثقة: حذف الامتحان لا يحذف أسئلته (ExamId تصير null لها)
            await _examService.DeleteAsync(id);
            return Ok(new ApiResponse(200, "Exam deleted successfully"));
        }

        // 🔹 علاقات إضافية — مطابقة حرفياً لـ IExamService

        /// <summary>أسئلة امتحان معيّن (بالإجابات الكاملة)</summary>
        /// <remarks>الصلاحيات: Admin, Teacher فقط - يكشف حقل Answer، لا يُفتح للطالب.</remarks>
        [Tags("Exam - الاستعلام والعلاقات (Queries)")]
        [HttpGet("{id}/questions")]
        [Authorize(Roles = "Admin,Teacher")]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<QuestionReadDto>>), StatusCodes.Status200OK)]
        public async Task<ActionResult<ApiResponse<IEnumerable<QuestionReadDto>>>> GetQuestionsByExamId(int id)
        {
            var questions = await _examService.GetQuestionsByExamIdAsync(id);
            return Ok(new ApiResponse<IEnumerable<QuestionReadDto>>(200, "Questions retrieved successfully", questions));
        }

        /// <summary>نتائج كل الطلاب بامتحان معيّن</summary>
        /// <remarks>الصلاحيات: Admin, Teacher فقط - نتائج كل الطلاب مجتمعين.</remarks>
        [Tags("Exam - الاستعلام والعلاقات (Queries)")]
        [HttpGet("{id}/results")]
        [Authorize(Roles = "Admin,Teacher")]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<ResultReadDto>>), StatusCodes.Status200OK)]
        public async Task<ActionResult<ApiResponse<IEnumerable<ResultReadDto>>>> GetResultsByExamId(int id)
        {
            var results = await _examService.GetResultsByExamIdAsync(id);
            return Ok(new ApiResponse<IEnumerable<ResultReadDto>>(200, "Results retrieved successfully", results));
        }

        /// <summary>الدرس المرتبط بامتحان معيّن (Nullable — الامتحان قد يكون عاماً/شاملاً للكورس)</summary>
        /// <remarks>الصلاحيات: عام (بدون تسجيل دخول).</remarks>
        /// <response code="404">هذا الامتحان غير مرتبط بدرس محدد</response>
        [Tags("Exam - الاستعلام والعلاقات (Queries)")]
        [HttpGet("{id}/lesson")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(ApiResponse<LessonReadDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApiResponse<LessonReadDto>>> GetLessonByExamId(int id)
        {
            var lesson = await _examService.GetLessonByExamIdAsync(id);
            if (lesson == null)
                return NotFound(new ApiResponse(404, "This exam is not linked to a specific lesson"));

            return Ok(new ApiResponse<LessonReadDto>(200, "Lesson retrieved successfully", lesson));
        }

        /// <summary>كل امتحانات درس معيّن</summary>
        /// <remarks>الصلاحيات: عام (بدون تسجيل دخول).</remarks>
        [Tags("Exam - الاستعلام والعلاقات (Queries)")]
        [HttpGet("lesson/{lessonId}")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<ExamReadDto>>), StatusCodes.Status200OK)]
        public async Task<ActionResult<ApiResponse<IEnumerable<ExamReadDto>>>> GetExamsByLessonId(int lessonId)
        {
            var exams = await _examService.GetExamsByLessonIdAsync(lessonId);
            return Ok(new ApiResponse<IEnumerable<ExamReadDto>>(200, "Exams retrieved successfully", exams));
        }

        /// <summary>عدد امتحانات درس معيّن</summary>
        /// <remarks>الصلاحيات: عام (بدون تسجيل دخول).</remarks>
        [Tags("Exam - الإحصائيات (Statistics)")]
        [HttpGet("lesson/{lessonId}/count")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(ApiResponse<int>), StatusCodes.Status200OK)]
        public async Task<ActionResult<ApiResponse<int>>> GetTotalExamsByLessonId(int lessonId)
        {
            var count = await _examService.GetTotalExamsByLessonIdAsync(lessonId);
            return Ok(new ApiResponse<int>(200, "Total exams count retrieved successfully", count));
        }
    }
}
