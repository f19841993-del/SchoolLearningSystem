using Microsoft.AspNetCore.Mvc;
using SchoolLearningSystem.API.Responses;
using SchoolLearningSystem.Applicationf.DTOs.ExamDto;
using SchoolLearningSystem.Applicationf.Interfaces;

namespace SchoolLearningSystem.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ExamController : ControllerBase
    {
        private readonly IExamService _examService;

        public ExamController(IExamService examService)
        {
            _examService = examService;
        }

        // 🔹 CRUD الأساسي

        /// <summary>
        /// يرجع كل الامتحانات الموجودة بالنظام
        /// </summary>
        /// <response code="200">تم جلب الامتحانات بنجاح</response>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var exams = await _examService.GetAllExamsAsync();
            return Ok(new ApiResponse<IEnumerable<ExamReadDto>>(200, "Exams retrieved successfully", exams));
        }

        /// <summary>
        /// يرجع امتحان محدد حسب الـ Id
        /// </summary>
        /// <response code="200">تم جلب الامتحان بنجاح</response>
        /// <response code="404">الامتحان غير موجود</response>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var exam = await _examService.GetExamByIdAsync(id);
            if (exam == null)
                return NotFound(new ApiResponse<string>(404, "Exam not found"));

            return Ok(new ApiResponse<ExamReadDto>(200, "Exam retrieved successfully", exam));
        }

        /// <summary>
        /// إضافة امتحان جديد
        /// </summary>
        /// <response code="201">تم إنشاء الامتحان بنجاح</response>
        /// <response code="400">خطأ في البيانات المدخلة</response>
        [HttpPost]
        public async Task<IActionResult> Add(ExamCreateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ApiResponse<string>(400, "Invalid input data"));

            await _examService.AddExamAsync(dto);
            return StatusCode(201, new ApiResponse<string>(201, "Exam created successfully"));
        }

        /// <summary>
        /// تحديث بيانات امتحان موجود
        /// </summary>
        /// <response code="200">تم تحديث الامتحان بنجاح</response>
        /// <response code="404">الامتحان غير موجود</response>
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, ExamUpdateDto dto)
        {
            try
            {
                await _examService.UpdateExamAsync(id, dto);
                return Ok(new ApiResponse<string>(200, "Exam updated successfully"));
            }
            catch (Exception ex)
            {
                return NotFound(new ApiResponse<string>(404, ex.Message));
            }
        }

        /// <summary>
        /// حذف امتحان
        /// </summary>
        /// <response code="200">تم حذف الامتحان بنجاح</response>
        /// <response code="404">الامتحان غير موجود</response>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _examService.DeleteExamAsync(id);
                return Ok(new ApiResponse<string>(200, "Exam deleted successfully"));
            }
            catch (Exception ex)
            {
                return NotFound(new ApiResponse<string>(404, ex.Message));
            }
        }

        // 🔹 علاقات إضافية

        /// <summary>
        /// يرجع الأسئلة المرتبطة بالامتحان
        /// </summary>
        /// <response code="200">تم جلب الأسئلة بنجاح</response>
        [HttpGet("{id}/questions")]
        public async Task<IActionResult> GetQuestionsByExamId(int id)
        {
            var questions = await _examService.GetQuestionsByExamIdAsync(id);
            return Ok(new ApiResponse<IEnumerable<QuestionReadDto>>(200, "Questions retrieved successfully", questions));
        }

        /// <summary>
        /// يرجع نتائج الطلاب المرتبطة بالامتحان
        /// </summary>
        /// <response code="200">تم جلب النتائج بنجاح</response>
        [HttpGet("{id}/results")]
        public async Task<IActionResult> GetResultsByExamId(int id)
        {
            var results = await _examService.GetResultsByExamIdAsync(id);
            return Ok(new ApiResponse<IEnumerable<ResultReadDto>>(200, "Results retrieved successfully", results));
        }

        /// <summary>
        /// يرجع الدروس المرتبطة بالامتحان
        /// </summary>
        /// <response code="200">تم جلب الدروس بنجاح</response>
        [HttpGet("{id}/lessons")]
        public async Task<IActionResult> GetLessonsByExamId(int id)
        {
            var lessons = await _examService.GetLessonsByExamIdAsync(id);
            return Ok(new ApiResponse<IEnumerable<LessonReadDto>>(200, "Lessons retrieved successfully", lessons));
        }
    }
}
