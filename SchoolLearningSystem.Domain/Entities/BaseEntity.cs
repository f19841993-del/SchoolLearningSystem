namespace SchoolLearningSystem.Domain.Entities
{
    public abstract class BaseEntity
    {
        // معرف فريد لكل سجل في قاعدة البيانات
        public int Id { get; set; }

        // تاريخ إنشاء السجل (مفيد جداً للـ AI لتحليل تطور مستوى الطالب عبر الزمن)
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // تاريخ آخر تحديث للسجل
        public DateTime? LastModifiedAt { get; set; }

        // خاصية الحذف المنطقي (Soft Delete) - لمنع مسح البيانات نهائياً
        public bool IsDeleted { get; set; } = false;
    }
}