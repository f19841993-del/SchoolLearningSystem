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
        public bool IsCorrect { get; set; } // ضروري جداً لتحديث EaseFactor
        public int TimeTakenInSeconds { get; set; } // ضروري جداً لحساب الأداء
    }
}
