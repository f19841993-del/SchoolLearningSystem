using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolLearningSystem.Domain.Models
{
    public record QuestionDifficultyStats(
          int QuestionId, string QuestionText,
          int LessonId, string LessonTitle,
          int TotalAttempts, int IncorrectCount)
    {
        public double ErrorRatePercent => TotalAttempts == 0 ? 0 : (double)IncorrectCount / TotalAttempts * 100;
    }
}
