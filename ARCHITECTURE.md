# توثيق معماري موحّد — SchoolLearningSystem

> هذا الملف يجمع القرارات المعمارية والأنماط المتكررة اللي كانت متفرقة بتعليقات
> داخل عشرات ملفات الكود (`📌 دور هذا الـ Service`, `💡`, `⚠️`) بمكان واحد فعلي
> بالمستودع. الهدف: أي مطوّر أو أداة ذكاء اصطناعي تفهم "ليش الكود مبني هيك" من
> قراءة ملف واحد، بدل ما تدور بين 15 ملف لتجمع نفس القرار.
>
> لتفاصيل ميزة معينة ببناء خطوة-بخطوة (مثل رحلة بناء الـ Auth كاملة)، راجع
> الملفات المتخصصة المرتبطة أدناه — هذا الملف بس يلخّص **القرار والسبب**، مو
> "اليوميات" الكاملة لكل خطوة.

---

## 1. البنية الطبقية (Layers)

```
SchoolLearningSystem.Domain          → Entities, Interfaces (عقود Repository), Enums, Models القراءة فقط
SchoolLearningSystem.Applicationf    → Services (منطق العمل), DTOs, Validators (FluentValidation), Mappings (AutoMapper), Exceptions
SchoolLearningSystem.Infrastructure  → EF Core: DbContext, Configurations, Repositories (تنفيذ فعلي), Migrations, Seeding
SchoolLearningSystem.API             → Controllers, Middleware, Authentication (JWT), Upload Handling
```

القاعدة العامة: **Controller** يتحقق من الصلاحيات (Ownership) وينادي **Service**؛
**Service** يحتوي كل منطق العمل (Business Rules) ويقرر متى يرمي استثناء؛
**Repository** ينفّذ الاستعلامات فقط، بدون أي قرار عمل بداخله.

---

## 2. نمط الـ Repository + Unit of Work

- كل Repository يرث `GenericRepository<T> where T : BaseEntity` ([`GenericRepository.cs`](SchoolLearningSystem.Infrastructure/Repositories/Base/GenericRepository.cs)) اللي يوفّر CRUD جاهز.
- **`AddAsync`/`UpdateAsync`/`DeleteAsync` لا تستدعي `SaveChangesAsync` بداخلها أبداً.**
  الحفظ الفعلي بقاعدة البيانات مسؤولية الـ **Service حصراً**، ويصير مرة وحدة
  بنهاية كل Use Case (نمط Unit of Work).
- الفائدة: عملية منطقية وحدة ممكن تلمس أكثر من Repository (مثلاً تحديث تقدّم
  الطالب + إنشاء سجل إجابة + تحديث إحصائيات جلسة) وتنحفظ كلها بمعاملة قاعدة
  بيانات **واحدة ذرية** — لأن كل الـ Repositories تشترك بنفس `AppDbContext`
  الـ Scoped بنفس الـ Request.
- مثال فعلي وواضح: [`SrsService.ProcessAnswerAsync`](SchoolLearningSystem.Applicationf/Services/SrsService.cs:42)
  يعدّل `StudentQuestionProgress` + `StudentAnswerDetail` + `MemorizeSession` بالذاكرة،
  وبنهاية الدالة استدعاء وحيد لـ `_progressRepository.SaveChangesAsync()` يغطي الثلاثة سوا.
- كيانات بدون `BaseEntity` (مثل `StudentQuestionProgress` اللي مفتاحه مركّب
  `StudentId+QuestionId`) لها Repository مخصص لا يرث `GenericRepository<T>`،
  لكنه يتبع **نفس الاتفاقية بالضبط**: `SaveChangesAsync()` ميثود منفصلة، تُستدعى
  من الـ Service بس.

---

## 3. الحذف المنطقي (Soft Delete)

- كل كيان يرث `BaseEntity` فيه `IsDeleted` (bool).
- `AppDbContext` يطبّق **Global Query Filter** تلقائياً (`!IsDeleted`) على كل
  استعلام عادي لأي كيان يرث `BaseEntity` — الـ Repositories ما تحتاج تكتب
  `.Where(x => !x.IsDeleted)` يدوياً بأي استعلام عادي.
- `RestoreAsync`/`HardDeleteAsync` (لو موجودة) تستخدم `IgnoreQueryFilters()`
  عمداً لأنها الوحيدة اللي تحتاج توصل لسجلات `IsDeleted = true`.

---

## 4. التعامل مع الأخطاء (Fail-Fast + ExceptionMiddleware)

الـ Services تتبع نمط **Fail-Fast**: ترمي استثناء فوراً بدل ما ترجع `null` أو
`false` وتخلي الـ Controller يخمّن السبب. [`ExceptionMiddleware`](SchoolLearningSystem.API/Middleware/ExceptionMiddleware.cs)
يترجم 3 أنواع استثناءات فقط لأكواد HTTP محددة — أي استثناء آخر يرجع **500 عام**
(ويتسجل بـ `LogError` كأنه عطل حقيقي بالسيرفر، فانتبه تستخدم النوع الصحيح):

| نوع الاستثناء | HTTP Code | يُستخدم لـ |
|---|---|---|
| `NotFoundException` | 404 | المورد المطلوب (Id) غير موجود |
| `CustomValidationException` | 400 | فشل تحقق تفصيلي (قائمة Field+Message) |
| `BadRequestException` | 400 | خطأ عمل عام برسالة واحدة (مثال: ملف رفع غير صالح) |
| أي استثناء آخر | 500 | خطأ سيرفر حقيقي غير متوقع فقط |

`BaseService<T>.UpdateAsync/DeleteAsync` يرميان `NotFoundException` دائماً لو
الكيان غير موجود — نفس السلوك بالضبط بين العمليتين، بدل تناقض صامت.

---

## 5. المصادقة والصلاحيات (Auth/JWT)

نظام كامل: `User` (كيان مستقل بعلاقة 1-to-1 اختيارية مع `Student`/`Teacher`)،
JWT عبر `JwtBearer` الرسمي، BCrypt للتشفير، وحماية كل الكونترولرز بـ
`[Authorize(Roles = "...")]` + فحص Ownership يدوي للبيانات الشخصية:

```csharp
if (User.IsInRole("Student") && User.FindFirstValue("studentId") != studentId.ToString())
    return Forbid();
```

**للتفاصيل الكاملة خطوة-بخطوة (كل قرار، كل خطأ صار وكيف انصلح، مصفوفة الصلاحيات
الكاملة لكل Endpoint)** → راجع [`AUTH_JWT_IMPLEMENTATION_JOURNEY.md`](AUTH_JWT_IMPLEMENTATION_JOURNEY.md).

**قرار أمني ملخّص يستاهل التذكير هنا:** `QuestionController`/`ExerciseController`
مقيّدين بالكامل لـ `Admin,Teacher` (ولا حتى الطالب) لأن الـ DTO يكشف حقل
`Answer` مباشرة — لا يوجد DTO منفصل للطالب حالياً (فجوة معروفة، راجع قسم 8).

---

## 6. خوارزمية SRS (SM-2) — منطق حسّاس يستاهل انتباه خاص

المكان: [`SrsService.ProcessAnswerAsync`](SchoolLearningSystem.Applicationf/Services/SrsService.cs:42)

| القرار | القيمة/المنطق | السبب |
|---|---|---|
| نطاق الجودة المقبول | `Quality` بين 0 و 5 شامل | خارج هذا النطاق `BadRequestException` فوري |
| عتبة "إجابة صحيحة" | `Quality >= 3` | محسوبة **بالسيرفر** من الـ Quality، مو حقل منفصل يرسله الفرونت (يمنع تلاعب) |
| Quality < 3 (نسيان) | `RepetitionLevel = 0`, `Interval = 1` | يرجّع الجدولة للبداية بالكامل |
| Quality >= 3 | `Interval` حسب `RepetitionLevel`: `0→1`, `1→6`, غير ذلك `Round(Interval * EaseFactor)` | صيغة SM-2 القياسية |
| الحد الأدنى لـ `EaseFactor` | `1.3` (ثابت `MinimumEaseFactor`) | يمنع الفاصل الزمني من التقلّص لدرجة تصير المراجعة عديمة الفائدة |
| صيغة تحديث `EaseFactor` | `EaseFactor + (0.1 - (5-Quality) * (0.08 + (5-Quality) * 0.02))` ثم `Max(., 1.3)` | صيغة SM-2 الأصلية بدون أي تعديل |

**هذي أهم مرشح لاختبارات Unit Tests بالمشروع** (لسا غير مغطاة، راجع قسم 8) —
أي كسر صامت بهذي الصيغة يفسد جدولة المراجعة لكل طالب بصمت، بدون أي خطأ ظاهر.

---

## 7. رفع الملفات (Upload Handling)

المكان: [`FileStorageService`](SchoolLearningSystem.API/Upload%20Handling/FileStorageService.cs) + [`UploadController`](SchoolLearningSystem.API/Controllers/UploadController.cs)

| القرار | التفاصيل |
|---|---|
| القيود المقبولة | `.jpg .jpeg .png .webp .gif` فقط، حد أقصى 5 ميجابايت |
| شكل الرابط المُرجَع | **Absolute URL كامل** (`scheme://host/uploads/...`) وليس نسبي — لأن الفرونت على origin مختلف عن الـ API (CORS)، ولأن `CourseUpdateDtoValidator` يرفض أي رابط غير Absolute |
| تسمية الملفات | `Guid + الامتداد الأصلي` لمنع تصادم الأسماء |
| التخزين | `wwwroot/uploads/{profiles|courses}` + `app.UseStaticFiles()` مفعّل بـ `Program.cs` |
| تنظيف الملفات اليتيمة | لو فشل تحديث قاعدة البيانات **بعد** حفظ الملف على القرص، يُحذف الملف فوراً (try/catch بالـ Controller) بدل ما يضل ملف بلا مرجع له للأبد |
| حذف الصورة القديمة | عند رفع صورة جديدة، القديمة تُحذف تلقائياً — لكن فقط لو رابط محلي (`/uploads/...`)؛ روابط خارجية تُتجاهل بصمت (`DeleteImage`) |

---

## 8. فجوات معروفة (تحديث حي — راجع هذا القسم بدل البحث بالتاريخ)

| الفجوة | الحالة | ملاحظة |
|---|---|---|
| صفر مشاريع Unit Tests بالحل | ❌ لم يُنفَّذ | أهم مرشح: خوارزمية SM-2 (قسم 6) |
| DTO خاص بالطالب بدون حقل `Answer` لـ Question/Exercise | ❌ لم يُنفَّذ | الحل الحالي: تقييد كامل الكونترولر لـ Admin/Teacher (راجع قسم 5) |
| تذكيرات المراجعة اليومية (Notifications) | ❌ لم يُنفَّذ | لا يوجد أي نظام إشعارات حالياً |
| لوحة تحليلات معلم موحّدة | 🟡 جزئي | `GetHardestQuestions` ([`StudentAnswerDetailController.cs:196`](SchoolLearningSystem.API/Controllers/StudentAnswerDetailController.cs:196)) endpoint كامل وشغّال، لكن ما فيه تجميع شامل لعدة مؤشرات بمكان وحد |
| Refresh Token / تغيير الباسورد | ❌ لم يُنفَّذ | يحتاج تصميم منفصل |

**مكتمل فعلاً (خلافاً لتقارير سابقة، تحققنا بالكود مباشرة):**
- Auth/JWT كامل — Level 1 ✅
- رفع الملفات — Level 1 ✅ (قسم 7)
- `CourseStudent.ProgressPercentage`/`LastAccessedAt` تُحسب فعلياً بـ [`CourseStudentService.cs:148`](SchoolLearningSystem.Applicationf/Services/CourseStudentService.cs:148) — Level 3 ✅
- جلسة "تدريب مكثف" حقيقية عبر `MemorizeSessionController.StartRemedialSession` — Level 3 ✅
- Unit of Work — متبع صح فعلاً بكل الـ Services (قسم 2) — Level 2 (بند واحد) ✅

---

*هذا الملف يوثّق قرارات معمارية ثابتة نسبياً (تتغير ببطء). لتفاصيل رحلة بناء ميزة
معينة بالكامل، أنشئ ملف منفصل بنفس أسلوب [`AUTH_JWT_IMPLEMENTATION_JOURNEY.md`](AUTH_JWT_IMPLEMENTATION_JOURNEY.md)
واربطه من قسم مناسب هنا، بدل ما تكرر التفاصيل بهذا الملف.*
