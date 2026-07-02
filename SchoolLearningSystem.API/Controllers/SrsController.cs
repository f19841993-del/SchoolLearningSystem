using Microsoft.AspNetCore.Mvc;
using SchoolLearningSystem.API.Responses;
using SchoolLearningSystem.Applicationf.DTOs.Srs;
using SchoolLearningSystem.Applicationf.DTOs.StudentQuestionProgress;
using SchoolLearningSystem.Applicationf.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SchoolLearningSystem.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SrsController : ControllerBase
    {
        private readonly ISrsService _srsService;

        // حقن خدمة الذكاء الاصطناعي (Dependency Injection)
        public SrsController(ISrsService srsService)
        {
            _srsService = srsService;
        }

        /// <summary>
        /// يستقبل إجابة الطالب ويقوم بتحديث مساره التعليمي (خوارزمية التكرار المتباعد)
        /// </summary>
        [HttpPost("submit-answer")]
        [ProducesResponseType(typeof(ApiResponse<string>), 200)]
        [ProducesResponseType(typeof(ApiResponse<string>), 400)]
        public async Task<ActionResult<ApiResponse<string>>> SubmitAnswer([FromBody] AnswerSubmissionDto dto)
        {
            // خط الدفاع الأول الأساسي
            if (!ModelState.IsValid)
                return BadRequest(new ApiResponse<string>(400, "بيانات الإجابة غير مكتملة أو غير صحيحة."));

            // 🛡️ ملاحظة: هنا سيتم تفعيل الـ AnswerSubmissionDtoValidator داخل الخدمة.
            // إذا كانت قيمة Quality مثلاً 9 (خارج نطاق 0-5)، ستقوم الخدمة برمي خطأ مخصص
            // وسيلتقطه الـ Global Exception Middleware ليُرجعه للواجهة الأمامية كـ 400 Bad Request.

            // إرسال الإجابة إلى "العقل المدبر" ليقوم بحساب الموعد القادم
            await _srsService.ProcessAnswerAsync(dto);

            return Ok(new ApiResponse<string>(200, "تم تسجيل الإجابة وتحديث مسار الطالب بنجاح."));
        }

        /// <summary>
        /// يجلب الأسئلة التي حان موعد مراجعتها للطالب (جلسة المراجعة اليومية)
        /// يمكن تمرير تاريخ وهمي لاختبار الخوارزمية (Time Travel Simulation)
        /// </summary>
        [HttpGet("due-questions/{studentId}")]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<StudentQuestionProgressReadDto>>), 200)]
        public async Task<ActionResult<ApiResponse<IEnumerable<StudentQuestionProgressReadDto>>>> GetDueQuestions(
            int studentId, 
            [FromQuery] DateTime? simulatedDate = null) // 👈 السر هنا: متغير اختياري للمحاكاة
        {
            // 🌟 التعديل السحري: إذا تم إرسال تاريخ وهمي نستخدمه، وإلا نستخدم وقت السيرفر الحقيقي
            // ملاحظة: ستحتاج لتحديث الواجهة ISrsService و SrsService لتقبل هذا المتغير الاختياري
            // الكود التخيلي في الخدمة سيكون: var targetDate = simulatedDate ?? DateTime.UtcNow;
            
            // مؤقتاً، سنفترض أننا نمررها للخدمة (تتطلب تحديث الخدمة لتستقبله)
             var dueQuestions = await _srsService.GetDueQuestionsForSessionAsync(studentId, simulatedDate);
            
            //var dueQuestions = await _srsService.GetDueQuestionsForSessionAsync(studentId); // الكود الحالي

            return Ok(new ApiResponse<IEnumerable<StudentQuestionProgressReadDto>>(200, "تم جلب أسئلة المراجعة بنجاح.", dueQuestions));
        }
    }
}