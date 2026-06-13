using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolLearningSystem.Applicationf.DTOs.StudentQuestionProgress
{
    public class StudentQuestionProgressCreateDto
    {
        public int StudentId { get; set; }
        public int QuestionId { get; set; }
        // لا نحتاج لـ NextReviewDate هنا، السيرفر سيحسبه تلقائياً كبداية
    }
}
