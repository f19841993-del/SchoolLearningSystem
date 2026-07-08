using System;

namespace SchoolLearningSystem.Applicationf.Exceptions
{
    // داخل مشروع Application
    public class NotFoundException : Exception
    {
        public NotFoundException(string message) : base(message)
        {
        }

        // Overload إضافي شائع بمشاريع Clean Architecture: يبني الرسالة تلقائياً
        // من اسم الكيان ومعرّفه بدل تكرار صياغة الرسالة يدوياً بكل Service.
        // مثال استخدام: throw new NotFoundException("Course", id);
        public NotFoundException(string entityName, object key)
            : base($"لم يتم العثور على \"{entityName}\" بالمعرّف ({key}).")
        {
        }
    }
}