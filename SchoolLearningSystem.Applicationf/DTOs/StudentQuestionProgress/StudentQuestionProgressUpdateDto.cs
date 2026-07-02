using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolLearningSystem.Applicationf.DTOs.StudentQuestionProgress
{
    public class StudentQuestionProgressUpdateDto
    {
        public int StudentId { get; set; }
        public int QuestionId { get; set; }
        // 🧠 بيانات خوارزمية الذكاء (SRS)
        public DateTime NextReviewDate { get; set; }
        public int RepetitionLevel { get; set; }
        public double EaseFactor { get; set; }
        public int Interval { get; set; } // أضفناه حديثاً ليتوافق مع SM-2

        // 📊 بيانات التحليل والإحصاء
        public int TotalAttempts { get; set; }
        public int CorrectAttempts { get; set; }
        public DateTime LastReviewedAt { get; set; }
    }
}
