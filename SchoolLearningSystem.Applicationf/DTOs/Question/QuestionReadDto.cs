using System;
using System.Collections.Generic;
using System.Linq;

    namespace SchoolLearningSystem.Applicationf.DTOs.Question
    {
        public class QuestionReadDto
        {
            public int Id { get; set; }
            public string Text { get; set; } = string.Empty;
            public string Answer { get; set; } = string.Empty;
            public string DifficultyLevel { get; set; } = string.Empty;
            public int ExamId { get; set; }
            public string ExamTitle { get; set; } = string.Empty;
            public int QuestionNumber { get; set; }
        }
    }


