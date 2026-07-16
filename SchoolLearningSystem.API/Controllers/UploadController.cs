using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolLearningSystem.API.Responses;
using SchoolLearningSystem.API.UploadHandling;
using SchoolLearningSystem.Applicationf.DTOs.CourseDto;
using SchoolLearningSystem.Applicationf.DTOs.Student;
using SchoolLearningSystem.Applicationf.DTOs.Teacher;
using SchoolLearningSystem.Applicationf.Interfaces;

namespace SchoolLearningSystem.API.Controllers
{
    /// <summary>
    /// رفع صور البروفايل (طالب/معلم) وصورة الكورس فعلياً كملفات (multipart/form-data)،
    /// بدل الاكتفاء برابط نصي جاهز عبر PUT العادي. يحفظ الملف على القرص (wwwroot/uploads)
    /// ثم يحدّث حقل الصورة بالكيان المعني تلقائياً برابطه الجديد.
    /// </summary>
    [ApiController]
    [Route("api/upload")]
    [Authorize]
    public class UploadController : ControllerBase
    {
        private readonly IFileStorageService _fileStorageService;
        private readonly IStudentService _studentService;
        private readonly ITeacherService _teacherService;
        private readonly ICourseService _courseService;

        public UploadController(
            IFileStorageService fileStorageService,
            IStudentService studentService,
            ITeacherService teacherService,
            ICourseService courseService)
        {
            _fileStorageService = fileStorageService;
            _studentService = studentService;
            _teacherService = teacherService;
            _courseService = courseService;
        }

        /// <summary>رفع صورة بروفايل الطالب</summary>
        /// <remarks>
        /// الصلاحيات: Admin, Teacher, Student. الطالب يرفع بس صورته هو.
        /// Body: multipart/form-data بحقل اسمه "file" (jpg/jpeg/png/webp/gif، حتى 5 ميجابايت).
        /// </remarks>
        /// <response code="200">تم الرفع، ويرجع الرابط الجديد</response>
        /// <response code="400">ملف غير صالح (صيغة/حجم/مفقود)</response>
        /// <response code="404">الطالب غير موجود</response>
        /// <response code="403">طالب يحاول يرفع صورة لطالب ثاني</response>
        [HttpPost("students/{id}/profile-image")]
        [Authorize(Roles = "Admin,Teacher,Student")]
        [RequestSizeLimit(5 * 1024 * 1024)]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApiResponse<string>>> UploadStudentProfileImage(int id, IFormFile file)
        {
            if (User.IsInRole("Student") && User.FindFirstValue("studentId") != id.ToString())
                return Forbid();

            var student = await _studentService.GetByIdAsync(id);
            if (student == null)
                return NotFound(new ApiResponse(404, "Student not found"));

            var url = await _fileStorageService.SaveImageAsync(file, "profiles");
            try
            {
                await _studentService.UpdateAsync(id, new StudentUpdateDto { ProfileImage = url });
            }
            catch
            {
                // فشل تحديث قاعدة البيانات بعد حفظ الملف فعلياً على القرص - ننظّف الملف
                // اليتيم بدل تركه بدون أي مرجع له (Orphaned File)
                _fileStorageService.DeleteImage(url);
                throw;
            }
            _fileStorageService.DeleteImage(student.ProfileImage);

            return Ok(new ApiResponse<string>(200, "تم رفع صورة البروفايل بنجاح", url));
        }

        /// <summary>رفع صورة بروفايل المعلم</summary>
        /// <remarks>
        /// الصلاحيات: Admin, Teacher. المعلم يرفع بس صورته هو.
        /// Body: multipart/form-data بحقل اسمه "file" (jpg/jpeg/png/webp/gif، حتى 5 ميجابايت).
        /// </remarks>
        /// <response code="200">تم الرفع، ويرجع الرابط الجديد</response>
        /// <response code="400">ملف غير صالح (صيغة/حجم/مفقود)</response>
        /// <response code="404">المعلم غير موجود</response>
        /// <response code="403">معلم يحاول يرفع صورة لمعلم ثاني</response>
        [HttpPost("teachers/{id}/profile-image")]
        [Authorize(Roles = "Admin,Teacher")]
        [RequestSizeLimit(5 * 1024 * 1024)]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApiResponse<string>>> UploadTeacherProfileImage(int id, IFormFile file)
        {
            if (User.IsInRole("Teacher") && User.FindFirstValue("teacherId") != id.ToString())
                return Forbid();

            var teacher = await _teacherService.GetByIdAsync(id);
            if (teacher == null)
                return NotFound(new ApiResponse(404, "Teacher not found"));

            var url = await _fileStorageService.SaveImageAsync(file, "profiles");
            try
            {
                await _teacherService.UpdateAsync(id, new TeacherUpdateDto { ProfileImage = url });
            }
            catch
            {
                _fileStorageService.DeleteImage(url);
                throw;
            }
            _fileStorageService.DeleteImage(teacher.ProfileImage);

            return Ok(new ApiResponse<string>(200, "تم رفع صورة البروفايل بنجاح", url));
        }

        /// <summary>رفع صورة كورس</summary>
        /// <remarks>
        /// الصلاحيات: Admin, Teacher.
        /// Body: multipart/form-data بحقل اسمه "file" (jpg/jpeg/png/webp/gif، حتى 5 ميجابايت).
        /// </remarks>
        /// <response code="200">تم الرفع، ويرجع الرابط الجديد</response>
        /// <response code="400">ملف غير صالح (صيغة/حجم/مفقود)</response>
        /// <response code="404">الكورس غير موجود</response>
        [HttpPost("courses/{id}/image")]
        [Authorize(Roles = "Admin,Teacher")]
        [RequestSizeLimit(5 * 1024 * 1024)]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApiResponse<string>>> UploadCourseImage(int id, IFormFile file)
        {
            var course = await _courseService.GetByIdAsync(id);
            if (course == null)
                return NotFound(new ApiResponse(404, "Course not found"));

            var url = await _fileStorageService.SaveImageAsync(file, "courses");
            try
            {
                await _courseService.UpdateAsync(id, new CourseUpdateDto { Image = url });
            }
            catch
            {
                _fileStorageService.DeleteImage(url);
                throw;
            }
            _fileStorageService.DeleteImage(course.Image);

            return Ok(new ApiResponse<string>(200, "تم رفع صورة الكورس بنجاح", url));
        }
    }
}
