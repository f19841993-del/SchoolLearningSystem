

using System;

namespace SchoolLearningSystem.Domain.Services
{
    public static class SrsAlgorithm
    {
        /// <summary>
        /// خوارزمية التكرار المتباعد (مبنية على SM-2) لمحاكاة منحنى النسيان لإبنجهاوس
        /// </summary>
        /// <param name="currentRepetitions">عدد الإجابات الصحيحة المتتالية (يبدأ بـ 0)</param>
        /// <param name="currentEaseFactor">معامل السهولة الحالي (الافتراضي للسؤال الجديد هو 2.5)</param>
        /// <param name="currentInterval">الفاصل الزمني الحالي بالأيام (يبدأ بـ 0)</param>
        /// <param name="quality">جودة الإجابة من 0 إلى 5</param>
        public static (int Repetitions, double EaseFactor, int Interval, DateTime NextReviewDate) CalculateNextState(
            int currentRepetitions,
            double currentEaseFactor,
            int currentInterval,
            int quality)
        {
            int nextRepetitions;
            int nextInterval;
            double nextEaseFactor;

            // 1. حساب معامل السهولة الجديد (الذكاء الاصطناعي في تقييم صعوبة السؤال)
            // المعادلة الرسمية لـ SM-2
            nextEaseFactor = currentEaseFactor + (0.1 - (5 - quality) * (0.08 + (5 - quality) * 0.02));

            // معامل السهولة لا يجب أن يقل عن 1.3 أبداً (حتى لا يصبح التكرار مزعجاً جداً)
            if (nextEaseFactor < 1.3) nextEaseFactor = 1.3;

            // 2. تحديث التكرارات والفترة الزمنية (تطبيق منحنى النسيان)
            if (quality >= 3) // إجابة صحيحة (مقبولة إلى مثالية)
            {
                nextRepetitions = currentRepetitions + 1;

                if (currentRepetitions == 0)
                    nextInterval = 1; // المراجعة الأولى بعد يوم
                else if (currentRepetitions == 1)
                    nextInterval = 6; // المراجعة الثانية بعد 6 أيام
                else
                    nextInterval = (int)Math.Round(currentInterval * currentEaseFactor); // المراجعات القادمة تتوسع بناءً على معامل السهولة
            }
            else // إجابة خاطئة (نسيان تام أو خطأ فادح)
            {
                // إعادة ضبط التقدم لأن الطالب نسي المعلومة (يعود لبداية المنحنى)
                nextRepetitions = 0;
                nextInterval = 1; // يجب مراجعتها غداً فوراً
            }

            // 3. حساب موعد المراجعة القادم
            var nextReviewDate = DateTime.UtcNow.AddDays(nextInterval);

            return (nextRepetitions, nextEaseFactor, nextInterval, nextReviewDate);
        }
    }
}






