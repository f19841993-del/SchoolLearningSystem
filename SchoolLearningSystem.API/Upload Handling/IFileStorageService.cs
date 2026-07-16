namespace SchoolLearningSystem.API.UploadHandling
{
    /// <summary>
    /// مسؤول عن حفظ/حذف ملفات الصور المرفوعة (بروفايل الطالب/المعلم، صورة الكورس) على القرص
    /// وإرجاع مسار نسبي (Relative URL) يُخزَّن بحقول مثل Student.ProfileImage / Course.Image.
    /// </summary>
    public interface IFileStorageService
    {
        /// <summary>
        /// يتحقق من الملف (الصيغة/الحجم) ثم يحفظه تحت wwwroot/uploads/{subFolder}
        /// باسم فريد (Guid)، ويرجع رابطاً كاملاً (مثال: https://api.example.com/uploads/profiles/xxx.jpg)
        /// جاهزاً للاستخدام المباشر بالفرونت (Origin مختلف عن الـ API).
        /// </summary>
        Task<string> SaveImageAsync(IFormFile file, string subFolder);

        /// <summary>
        /// يحذف ملف صورة محلي سابق (يُستخدم عند استبدال صورة قديمة). يتجاهل بصمت
        /// أي رابط غير محلي (مثل رابط خارجي) أو غير موجود على القرص.
        /// </summary>
        void DeleteImage(string? url);
    }
}
