using AutoMapper;
using SchoolLearningSystem.Applicationf.DTOs.MemorizeSession;
using SchoolLearningSystem.Applicationf.DTOs.StudentQuestionProgress;
using SchoolLearningSystem.Applicationf.Exceptions;
using SchoolLearningSystem.Applicationf.Interfaces;
using SchoolLearningSystem.Applicationf.Services.Base;
using SchoolLearningSystem.Domain.Entities;
using SchoolLearningSystem.Domain.Interfaces;

namespace SchoolLearningSystem.Applicationf.Services
{
    // ==================================================================================
    // 📌 دور هذا الـ Service:
    // يدير "جلسات المراجعة" (MemorizeSession) - قلب محرك التكرار المتباعد (SRS).
    // كل جلسة تمثل جولة مراجعة واحدة للطالب تحتوي على مجموعة أسئلة (AnswerDetails)
    // مستحقة المراجعة بناءً على خوارزمية SRS، بغض النظر عن الدرس الذي تنتمي إليه.
    // ==================================================================================
    public class MemorizeService
        : BaseService<MemorizeSession, MemorizeSessionReadDto, MemorizeSessionCreateDto, MemorizeSessionUpdateDto>,
          IMemorizeService
    {
        private readonly IMemorizeRepository _memorizeRepository;
        private readonly IStudentRepository _studentRepository;
        private readonly ISrsService _srsService;   // 👈 الإضافة الجديدة


        public MemorizeService(
            IMemorizeRepository memorizeRepository,
            IStudentRepository studentRepository,
            ISrsService srsService ,
            IMapper mapper)
            : base(memorizeRepository, mapper)
        {
            _memorizeRepository = memorizeRepository;
            _studentRepository = studentRepository;
            _srsService = srsService;
        }

        // 🔹 CRUD الأساسي موروث من BaseService

        // ============================================================================
        // 🎯 Use Case: "الطالب يفتح شاشة (المراجعة اليومية)، والنظام يحتاج يعرف:
        //              هل عنده جلسة بدأها اليوم ولم ينهيها، ليكمّلها من حيث توقف،
        //              أو يبدأ جلسة جديدة من الصفر؟"
        //
        // 💡 نرجع null (وليس استثناء) إذا ما فيه جلسة نشطة - هذي حالة طبيعية جداً
        //    (أغلب الأوقات الطالب ما يكون عنده جلسة مفتوحة)، والـ Controller يقرر
        //    بناءً عليها: يعرض زر (بدء جلسة جديدة) أو (إكمال الجلسة الحالية).
        // ============================================================================
        public async Task<MemorizeSessionReadDto?> GetActiveSessionByStudentIdAsync(int studentId)
        {
            var studentExists = await _studentRepository.GetByIdAsync(studentId)
                ?? throw new NotFoundException($"الطالب برقم {studentId} غير موجود.");

            var session = await _memorizeRepository.GetActiveSessionByStudentIdAsync(studentId);
            return _mapper.Map<MemorizeSessionReadDto?>(session);
        }

        // ============================================================================
        // 🎯 Use Case: "الطالب أو ولي أمره يفتح (سجل المراجعات) ليشوف تطور
        //              الالتزام بالمراجعة اليومية عبر الأسابيع/الأشهر"
        // ============================================================================
        public async Task<IEnumerable<MemorizeSessionReadDto>> GetSessionHistoryByStudentIdAsync(int studentId)
        {
            var studentExists = await _studentRepository.GetByIdAsync(studentId)
                ?? throw new NotFoundException($"الطالب برقم {studentId} غير موجود.");

            var sessions = await _memorizeRepository.GetSessionHistoryByStudentIdAsync(studentId);
            return _mapper.Map<IEnumerable<MemorizeSessionReadDto>>(sessions);
        }

        // ============================================================================
        // 🎯 Use Case: "بعد انتهاء الجلسة، الطالب يفتح شاشة (ملخص الجلسة) ليشوف
        //              كل الـ 20 سؤال اللي جاوب عليها وأيها صحيح وأيها خاطئ"
        //
        // نمط Fail Fast: نرمي استثناء إذا الجلسة نفسها غير موجودة (خطأ فعلي، مو حالة عمل).
        // ============================================================================
        public async Task<MemorizeSessionReadDto> GetSessionWithAnswersAsync(int sessionId)
        {
            var session = await _memorizeRepository.GetSessionWithAnswersAsync(sessionId)
                ?? throw new NotFoundException($"جلسة المراجعة برقم {sessionId} غير موجودة.");

            return _mapper.Map<MemorizeSessionReadDto>(session);
        }

        // ============================================================================
        // 🎯 Use Case: "الطالب يجاوب على آخر سؤال بالجلسة، فيضغط النظام تلقائياً
        //              (إنهاء الجلسة) ويسجّل نسبة نجاحه ووقت الانتهاء"
        //
        // مين يستدعيها: النظام تلقائياً (Backend) بعد آخر إجابة، مو الطالب يدوياً.
        // ============================================================================
        public async Task CompleteSessionAsync(int sessionId)
        {
            var session = await _memorizeRepository.GetByIdAsync(sessionId)
                ?? throw new NotFoundException($"جلسة المراجعة برقم {sessionId} غير موجودة.");

            session.IsCompleted = true;
            // SuccessRate لا تُلمس هنا - محسوبة أصلاً وبدقة تراكمياً داخل
           // SrsService.ProcessAnswerAsync مع كل إجابة، القيمة الحالية هي الصحيحة.
            session.CompletedAt = DateTime.UtcNow;

            await _memorizeRepository.UpdateAsync(session);
            await _memorizeRepository.SaveChangesAsync();
        }

        // ============================================================================
        // 🎯 Use Case الكامل اللي تخدمه هذي الدالة:
        // "الطالب يضغط زر (ابدأ المراجعة) من شاشة الموبايل/الفرونت"
        //
        // النظام لازم يقرر واحد من ثلاث حالات:
        //
        //   الحالة 1: عنده جلسة مفتوحة من قبل (بدأها ولم يُنهها) 
        //             → نكمّله بنفس الجلسة، ما ننشئ وحدة جديدة فوقها
        //             (مثال واقعي: سكّر المتصفح بمنتصف المراجعة، رجع بعد نص ساعة)
        //
        //   الحالة 2: ما عنده جلسة نشطة، وفيه أسئلة مستحقة عليه اليوم حسب SM-2
        //             → ننشئ جلسة جديدة فاضية، وترجع مع قائمة الأسئلة المستحقة
        //
        //   الحالة 3: ما عنده جلسة نشطة، وما فيه ولا سؤال مستحق اليوم
        //             → لا داعي ننشئ جلسة فاضية بالداتابيس أصلاً (توفير مساحة/موارد)
        //             → نرجّع Session = null ليعرف الفرونت يعرض رسالة
        //               ("خلصت كل مراجعاتك اليوم! 🎉") بدل شاشة فاضية
        // ============================================================================
        public async Task<MemorizeSessionStartResultDto> StartNewSessionAsync(int studentId)
        {
            // ✅ خطوة 0: تأكيد وجود الطالب فعلياً (نفس المنطق السابق، بدون تغيير)
            var studentExists = await _studentRepository.GetByIdAsync(studentId)
                ?? throw new NotFoundException($"الطالب برقم {studentId} غير موجود.");

            // 🆕 خطوة 1 (الإضافة الجديدة): تحقق أول شي — هل عنده جلسة نشطة أصلاً؟
            //
            // نستخدم نفس الـ Repository method اللي أصلاً مبنية ومستخدمة بـ
            // GetActiveSessionByStudentIdAsync (القسم فوق بنفس الملف) — ما نكرر منطق جديد،
            // بس نستدعيها هنا بالمكان الصح قبل ما نقرر ننشئ جلسة جديدة.
            var activeSession = await _memorizeRepository.GetActiveSessionByStudentIdAsync(studentId);

            if (activeSession != null)
            {
                // 🔁 الحالة 1: عنده جلسة مفتوحة من قبل — نكمّله بيها، ما ننشئ جديدة
                //
                // بس لازم نجيبله الأسئلة المستحقة "الحالية" (مو القديمة وقت ما بدأ الجلسة) —
                // لأنه ممكن يكون مرّ وقت من وقت ما بدأ الجلسة، وتصير أسئلة زيادة مستحقة الحين.
                var dueQuestionsForActiveSession = (await _srsService
                    .GetDueQuestionsForSessionAsync(studentId, DateTime.UtcNow))
                    .ToList();

                return new MemorizeSessionStartResultDto
                {
                    Session = _mapper.Map<MemorizeSessionReadDto>(activeSession),
                    DueQuestions = dueQuestionsForActiveSession
                };
            }

            // 📌 من هنا وطالع: نفس منطقك الأصلي بدون أي تغيير —
            // وصلنا هنا فقط لو تأكدنا إنه ما عنده جلسة نشطة مسبقاً

            // 1️⃣ نجيب الأسئلة المستحقة من محرك SRS نفسه
            var dueQuestions = (await _srsService
                .GetDueQuestionsForSessionAsync(studentId, DateTime.UtcNow))
                .ToList();

            // 2️⃣ الحالة 3: ما فيه أسئلة مستحقة → لا داعي ننشئ جلسة فاضية بالداتابيس
            if (!dueQuestions.Any())
            {
                return new MemorizeSessionStartResultDto
                {
                    Session = null,
                    DueQuestions = new List<StudentQuestionProgressReadDto>()
                };
            }

            // 3️⃣ الحالة 2: ننشئ جلسة جديدة فارغة (تُملأ لاحقاً مع كل إجابة يرسلها الطالب
            //    عبر AnswerSubmissionDto.MemorizeSessionId)
            var sessionDto = new MemorizeSessionCreateDto
            {
                StudentId = studentId,
                ExerciseId = null,
                DurationInSeconds = 0
            };
            var createdSession = await CreateAsync(sessionDto);

            // 4️⃣ نرجع الجلسة الجديدة + الأسئلة المستحقة سوا
            return new MemorizeSessionStartResultDto
            {
                Session = createdSession,
                DueQuestions = dueQuestions
            };
        }
    }
}