using System.ComponentModel.DataAnnotations;

namespace SchoolLearningSystem.Applicationf.DTOs.CourseStudent
{
    // يمثّل عملية "تسجيل طالب بكورس" (Enrollment) - نفس فلسفة
    // StudentQuestionProgressCreateDto: أقل حقول ممكنة، والباقي افتراضي بالـ Entity.
    public class CourseStudentCreateDto
    {
        public int CourseId { get; set; }

        public int StudentId { get; set; }

        // EnrolledAt, IsActive, ProgressPercentage, LastAccessedAt غير موجودة هنا
        // عمداً - كلها لها قيم افتراضية بالـ Entity أو تُحسب لاحقاً بالاستخدام الفعلي.
    }
}