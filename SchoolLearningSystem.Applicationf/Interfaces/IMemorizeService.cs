using SchoolLearningSystem.Applicationf.DTOs.MemorizeSession;
using SchoolLearningSystem.Applicationf.Interfaces.Base;

namespace SchoolLearningSystem.Applicationf.Interfaces
{
    public interface IMemorizeService
        : IBaseService<MemorizeSessionReadDto, MemorizeSessionCreateDto, MemorizeSessionUpdateDto>
    {
        // 🔹 العمليات الأساسية (GetAll, GetById, Create, Update, Delete, GetPaged)
        // موجودة مسبقاً بفضل الوراثة من IBaseService

        // ==========================================
        // 🔹 عمليات مخصصة لجلسات المراجعة (SRS Business Logic)
        // ==========================================

        // جلب الجلسة النشطة الحالية للطالب (إن وُجدت) - نقطة البداية عند فتح شاشة المراجعة
        Task<MemorizeSessionReadDto?> GetActiveSessionByStudentIdAsync(int studentId);

        // جلب سجل كل جلسات الطالب (تاريخ المراجعات) لعرضه بلوحة تحكمه
        Task<IEnumerable<MemorizeSessionReadDto>> GetSessionHistoryByStudentIdAsync(int studentId);

        // جلب جلسة معيّنة مع كل تفاصيل إجاباتها (مراجعة كاملة بعد الانتهاء)
        Task<MemorizeSessionReadDto> GetSessionWithAnswersAsync(int sessionId);

        // إنهاء الجلسة الحالية: تحديد IsCompleted=true وتسجيل CompletedAt ونسبة النجاح
        Task CompleteSessionAsync(int sessionId, double successRate);

        // IMemorizeService.cs
        Task<MemorizeSessionStartResultDto> StartNewSessionAsync(int studentId);
    }
}