using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolLearningSystem.Applicationf.DTOs.Analytics
{
    public class QuestionDifficultyStatsDto
    {
        public int QuestionId { get; set; }
        public string QuestionText { get; set; } = string.Empty;
        public int LessonId { get; set; }
        public string LessonTitle { get; set; } = string.Empty;
        public int TotalAttempts { get; set; }
        public int IncorrectCount { get; set; }
        public double ErrorRatePercent { get; set; }
    }
}
