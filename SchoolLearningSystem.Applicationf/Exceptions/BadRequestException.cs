using System;

namespace SchoolLearningSystem.Applicationf.Exceptions
{
    public class BadRequestException : Exception
    {
        // المشيد (Constructor) يستقبل الرسالة ويمررها للكلاس الأب (Exception)
        public BadRequestException(string message) : base(message)
        {
        }
    }
}