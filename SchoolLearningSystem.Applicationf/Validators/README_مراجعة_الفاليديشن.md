# مراجعة طبقة الـ Validators — SchoolLearningSystem

## الحكم على استخدام FluentValidation
نعم، ضروري وواقعي 100% بمشاريع Clean Architecture الحقيقية. يفصل التحقق عن الـ DTO
(POCO نظيف)، يدعم شروط عابرة للحقول (زي "أحد الحقلين مطلوب") و DI، ويُسجَّل تلقائياً
بسطر واحد. هذا مطابق تماماً للقرار الموثق أصلاً بـ Enterprise_API_Blueprint.pdf.

## ⚠️ قرار لازم يُحسم الآن (كان مفتوحاً بـ dtos_review_report.md §5)
رفع هذي الملفات يعني إنك تميل عملياً لخيار "FluentValidation + DTO نقي". لازم:
- تشيل كل `[Required]`/`[Range]`/`[StringLength]` (Data Annotations) من كل الـ DTOs
- وإلا رح يصير فحص مزدوج ومتضارب أحياناً (رسالتين خطأ مختلفتين لنفس الحقل)

## الأخطاء الحرجة اللي تم تصحيحها
1. **Namespace**: كل ملفاتك كانت `Applicationf` (بحرف f زائد) — بينما الـ DTOs
   المصححة سابقاً بقيت `Application` (بدون f). هذا كان Compile Error فوري.
2. **DependencyInjection.cs**: كان يسجّل `IStudentQuestionProgressService` المحذوف
   نهائياً (مدموج بـ `SrsService`) — Compile Error. كما كان **ناقص بالكامل**
   تسجيل `ITeacherService` و`IStudentService` (خدمتان من أصل 13 موثقة).
3. **StudentQuestionProgressUpdateDtoValidator**: كان يفحص `StudentId`/`QuestionId`
   بالـ Body رغم إنهما حُذفا منه (صارا Route Parameters) — وكان يفحص الحقول
   كإجبارية رغم إنها صارت nullable لدعم Partial Update.
4. **AnswerSubmissionDtoValidator**: كان ناقص فحص `MemorizeSessionId` رغم إنه FK
   إجباري مؤكد حديثاً.

## ملفات جديدة (لم تكن موجودة، تغطي باقي الـ 13 كيان)
Course✅(مصحح فقط) · CourseStudent · Curriculum · Teacher · Lesson · Exam ·
Question · Exercise · Result (فيها فرض قاعدة "لازم Lesson أو Exam" اللي كانت
مفتوحة بالتوثيق) · Student (Update فقط — Create معلّق على حسم Auth) ·
StudentAnswerDetail (احتياطي، لا Endpoint مباشر له) · MemorizeSession ·
StudentQuestionProgress✅(مصحح فقط) · Srs✅(مصحح فقط)

## نقطة مهمة: أسماء الحقول تقريبية
هذي الملفات كتبتها بالاعتماد على الوصف الموجود بملفات التوثيق (dtos_review_report.md)
لأن ملفات الـ DTO الفعلية نفسها ما كانت مرفوعة بهذي الجلسة. راجع أسماء الخصائص
(Property Names) مقابل الكود الفعلي قبل الإضافة للمشروع — الاحتمال الأكبر مطابقة
لكن التأكيد النهائي يحتاج مقارنة مباشرة.

## لسه مفتوح (غير متعلق بالفاليديشن نفسه)
- تصميم `StudentProgressSummaryDto` (ملخص تقدّم الطالب)
- `ReorderLessonsDto`
- حسم DTOs الخاصة بالـ Auth (Password/PasswordHash)
- شكل استجابة `POST .../memorize-sessions/start`
