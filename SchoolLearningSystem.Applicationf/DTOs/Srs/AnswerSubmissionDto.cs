namespace SchoolLearningSystem.Applicationf.DTOs.Srs
{
    public class AnswerSubmissionDto
    {
        public int StudentId { get; set; }
        public int QuestionId { get; set; }

        // 🌟 الحقل المفقود الأهم للذكاء الاصطناعي (من 0 إلى 5)
        public int Quality { get; set; }

        // الإجابة التي اختارها الطالب نصياً (لتوثيقها في سجل الذكاء الاصطناعي)
        public string? SelectedAnswer { get; set; }


        public int TimeTakenInSeconds { get; set; }
    }
}