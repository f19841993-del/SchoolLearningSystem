# رحلة بناء Auth/JWT الحقيقي — SchoolLearningSystem

> هذا الملف توثيق كامل ودقيق لكل خطوة تمت أثناء بناء نظام المصادقة والصلاحيات
> (Authentication & Authorization) بمشروع SchoolLearningSystem، من نقطة الصفر
> إلى نظام كامل يحمي كل الـ Endpoints حسب الدور (Admin/Teacher/Student).
> الهدف منه: يصير مرجع تشرحه لأي أداة ذكاء اصطناعي ثانية (أو تراجعه بنفسك لاحقاً)
> لفهم **كل قرار اتخذ وليش**، **كل خطأ صار وكيف انصلح**، وشكل النظام النهائي بالضبط.

---

## 1. نقطة البداية (الفجوة الأصلية)

قبل ما نبدأ، وضع المشروع كان:

- 3 ملفات فاضية بالكامل: `SchoolLearningSystem.API/Authentication/AuthController.cs`,
  `JwtMiddleware.cs`, `JwtSettings.cs` (كل واحد فيها كلاس بدون أي عضو).
- لا توجد أي حزمة JWT أو Identity مثبّتة بالمشروع.
- `Program.cs` فيه `app.UseAuthorization()` **بدون** `app.UseAuthentication()` قبلها —
  يعني حتى لو حطينا `[Authorize]` ما رح يشتغل صح لأنه ما في نظام مصادقة مسجّل أصلاً.
- كيانا `Student` و`Teacher` منفصلين تماماً، بدون أي حقل Password.
- **لا يوجد كيان Admin إطلاقاً** بأي مكان بالمشروع.
- الفجوة كانت موثّقة فعلياً بكومنت داخل `StudentController.cs`:
  > "⚠️ فجوة موثقة (Auth غير مبنية): StudentCreateDto بدون Password حالياً.
  > مؤقت لحين نقله لـ POST /api/auth/register/student"

---

## 2. القرارات المعمارية الأساسية (ولماذا)

### 2.1 جدول `Users` مركزي منفصل عن Student/Teacher
بدل ما نحط Password جوه جدول Student وجدول Teacher كل وحدة لحالها، بنينا كيان
`User` مستقل بعلاقة **1-to-1 اختيارية** مع كل منهم:

```
User
 ├─ Username, Email, PasswordHash, Role (enum), IsActive
 ├─ StudentId (int?) ──► Student   (nullable FK, nav واحد الاتجاه فقط)
 └─ TeacherId (int?) ──► Teacher   (nullable FK, نفس الشي)
```

**السبب:**
- Admin ماله جدول أصلاً بالمشروع — ما نريد نصنع صف Student وهمي حتى نسوي حساب أدمن.
- يفصل "هوية الدخول" (Auth) عن "بيانات البروفايل" (Domain) — تعديل لاحق بمنطق تسجيل
  الدخول ما يأثر على جداول الطلاب/المعلمين.
- Unique index **مشروط** (`WHERE StudentId IS NOT NULL`) يسمح بعدة صفوف NULL
  بنفس الوقت (لحسابات الأدمن) بينما يمنع تكرار نفس StudentId لأكثر من حساب دخول.

### 2.2 استخدام `JwtBearer` الجاهز من ASP.NET Core بدل Middleware يدوي
الملف `JwtMiddleware.cs` الأصلي بقي **بدون استخدام إطلاقاً** — بدله استخدمنا الحزمة
الرسمية `Microsoft.AspNetCore.Authentication.JwtBearer` مسجّلة بـ `Program.cs`
عبر `AddAuthentication().AddJwtBearer(...)`. السبب:
- تحقق التوقيع/الصلاحية/Issuer/Audience جاهز ومُختبر أمنياً، مو كود يدوي عرضة للثغرات.
- يتكامل تلقائياً مع `[Authorize]`, `[Authorize(Roles="...")]`, و`User.Claims`.

### 2.3 تشفير الباسورد: `BCrypt.Net-Next`
أبسط من `PasswordHasher<T>` الخاص بـ ASP.NET Identity (ما يحتاج EF Identity Stores).
دالتين بس: `BCrypt.HashPassword(...)` و`BCrypt.Verify(...)`.

### 2.4 سياسة التسجيل
- **Student / Teacher**: تسجيل ذاتي عام (Self-service)، الحساب يصير فعّال فوراً
  (`IsActive = true` افتراضياً).
- **Admin**: **لا يوجد Endpoint تسجيل إطلاقاً**. حساب واحد يُزرع تلقائياً بأول تشغيل
  للمشروع عبر `DbSeeder.SeedAdminUserAsync()` — قرار أمني مقصود (منع أي حد يسجّل
  حساب أدمن لنفسه عبر API عام).

---

## 3. الخطوات بالتفصيل (بالترتيب الزمني الفعلي)

### الخطوة 0 — الحزم و appsettings.json

| أضيف | بأي مشروع | لماذا |
|---|---|---|
| `Microsoft.AspNetCore.Authentication.JwtBearer` 8.0.0 | API | التحقق من التوكن + الـ Middleware |
| `System.IdentityModel.Tokens.Jwt` 8.0.1 | API | توليد التوكن (`JwtSecurityTokenHandler`) |
| `BCrypt.Net-Next` 4.2.0 | Applicationf **و** Infrastructure | تشفير الباسورد؛ احتجناها بمكانين لأن `AuthService` (Applicationf) و`DbSeeder` (Infrastructure) كلاهما يشفّران باسوردات |

`appsettings.json` أضيف له قسم:
```json
"Jwt": {
  "Key": "<مفتاح عشوائي آمن 64 بايت base64>",
  "Issuer": "SchoolLearningSystem",
  "Audience": "SchoolLearningSystemClient",
  "ExpiryMinutes": 120
}
```

**🐛 خطأ صار وانصلح:** أول مرة انكتب `Key` كنص placeholder حرفي
(`"REPLACE_WITH_A_LONG_RANDOM_SECRET..."`)، وبمحاولة تانية انكتب نص عربي تالف
بالخطأ. الحل: ولّدنا مفتاح فعلي عشوائي بـ `openssl rand -base64 64` وحطيناه مكانه.

**🐛 خطأ صار وانصلح:** حزمة `BCrypt.Net-Next` انسيت إضافتها لمشروع **Infrastructure**
تحديداً (كانت مضافة بـ Applicationf بس) — ظهر خطأ Build:
`error CS0103: The name 'BCrypt' does not exist in the current context` بملف
`DbSeeder.cs`. انصلح بإضافة نفس الحزمة لمشروع Infrastructure كمان.

---

### الخطوة 1 — Domain

ملفات جديدة:
- `Domain/Enums/UserRole.cs` → `enum UserRole { Admin = 1, Teacher = 2, Student = 3 }`
- `Domain/Entities/User.cs` → يرث `BaseEntity`، فيه:
  `Username, Email, PasswordHash, Role, IsActive, StudentId?, TeacherId?, Student?, Teacher?`
- `Domain/Interfaces/IUserRepository.cs` → يرث `IGenericRepository<User>` + يضيف:
  `GetByUsernameOrEmailAsync`, `ExistsByUsernameAsync`, `ExistsByEmailAsync`

**🐛 خطأ صار وانصلح (Build Error حقيقي):** `User.cs` انكتب أول مرة
`internal class User` بدل `public`. بما إنه `IUserRepository` (واجهة عامة) تستخدم
`User` بتوقيعها العام، صار خطأ:
```
error CS0061: Inconsistent accessibility: base interface 'IGenericRepository<User>'
              is less accessible than interface 'IUserRepository'
error CS0050: Inconsistent accessibility: return type 'Task<User?>' is less
              accessible than method 'IUserRepository.GetByUsernameOrEmailAsync'
```
انصلح بتغيير `internal` إلى `public`.

**🐛 مشكلة تصميمية صارت وانصلحت:** أول مسودة لـ `User.cs` فيها حقل زايد
`public int RoleId { get; set; }` بموازاة `Role` (enum). هذا مصدر حقيقة مضاعف
للدور بدون داعي (الـ enum نفسه مبني على int). انحذف الحقل، `Role` وحده كافي.

---

### الخطوة 2 — Infrastructure

- `Configurations/UserConfiguration.cs`:
  - `HasIndex(Username).IsUnique()`, `HasIndex(Email).IsUnique()`
  - `HasIndex(StudentId).IsUnique()` و`HasIndex(TeacherId).IsUnique()` — **الاثنين
    فهارس مشروطة** (SQL Server: `WHERE [StudentId] IS NOT NULL`) حتى يسمح بعدة
    NULL بنفس الوقت (Admin بدون Student ولا Teacher).
  - `HasOne(u => u.Student).WithMany().HasForeignKey(...).OnDelete(DeleteBehavior.SetNull)`
    (نفس الشي لـ Teacher) — حذف Student/Teacher فعلياً ما يكسر حساب الدخول، يخلي
    الـ FK يصير null بس.
  - `HasQueryFilter(u => !u.IsDeleted)` — نفس نمط الـ Soft Delete بكل كيان بالمشروع.
- `AppDbContext.cs`: أضيف `DbSet<User> Users`.
- `Repositories/UserRepository.cs`: يرث `GenericRepository<User>` (اللي جايبله CRUD
  كامل تلقائياً) + تنفيذ الثلاث دوال الإضافية.
- `DependencyInjection.cs`: `services.AddScoped<IUserRepository, UserRepository>();`
- Migration: `20260715191037_AddUsersTable` — راجعتها سطر سطر قبل التطبيق، طابقت
  الإعدادات فوق تماماً، وطُبّقت فعلياً على قاعدة البيانات الحقيقية `SchoolMathSrsDb`
  عبر `dotnet ef database update`.

**⚠️ ملاحظة تنظيف بسيطة (لسا موجودة، مو خطأ):** `UserRepository.cs` فيها حقل
`private readonly AppDbContext _context;` زايد يخفي حقل الأب `protected _context`
الموروث من `GenericRepository<T>` — تحذير Build بسيط (`CS0108`)، وظيفياً ما يأثر
لأن الاثنين يشيرون لنفس الـ instance.

---

### الخطوة 3 — Application

ملفات جديدة:
- `DTOs/Auth/`: `RegisterStudentDto`, `RegisterTeacherDto`, `LoginDto`, `AuthResponseDto`
- `Interfaces/IAuthService.cs`, `Interfaces/IJwtTokenGenerator.cs`
  (الواجهة بس هنا — التنفيذ الفعلي بطبقة الـ API لأنه يحتاج حزم/إعدادات خاصة بـ ASP.NET Core)
- `Services/AuthService.cs` — المنطق الفعلي:
  - `RegisterStudentAsync`: يتحقق يوزرنيم/إيميل غير مكرر → ينشئ `Student` عبر
    `IStudentService.CreateAsync` → ينشئ `User` مربوط بـ `StudentId` → يشفّر
    الباسورد → يولّد توكن.
  - `RegisterTeacherAsync`: نفس الفكرة، بس `TeacherCreateDto` **ما فيها حقل Subject
    إطلاقاً** (مثبّت على "Math" جوا `TeacherService` نفسه).
  - `LoginAsync`: يدور المستخدم بـ `GetByUsernameOrEmailAsync` → `BCrypt.Verify` →
    يتحقق `IsActive` → يولّد توكن.
- `Validators/AuthValidator/`: `RegisterStudentDtoValidator`, `RegisterTeacherDtoValidator`,
  `LoginDtoValidator` (FluentValidation، نفس نمط باقي الـ Validators بالمشروع).
- `DependencyInjection.cs`: `services.AddScoped<IAuthService, AuthService>();`

**🐛 خطأ صار وانصلح:** `AuthService.cs` بقيت فترة `internal class AuthService {}`
فاضية بالكامل قبل ما يُكتب المنطق الحقيقي فيها.

**🐛 خطأ صار وانصلح (استثناءات غلط):** أول تنفيذ استخدم
`UnauthorizedAccessException` و`InvalidOperationException` لأخطاء "باسورد غلط"
و"يوزرنيم مكرر". فحصنا `ExceptionMiddleware.cs` ولقينا إنه يتعامل بشكل خاص بس مع
3 أنواع: `NotFoundException` (404)، `CustomValidationException` (400)،
`BadRequestException` (400) — أي استثناء ثاني يرجع **500 Internal Server Error**
عام (مضلل، ويتسجل بـ `LogError` كأنه عطل حقيقي). انصلح: استبدال كل الاستثناءات
بـ `BadRequestException` (400 صحيح ومفهوم).

**🐛 باگ صار وانصلح (كوبي-بيست):** بدالة `EnsureUsernameAndEmailAreFreeAsync`،
فحص تكرار الـ **يوزرنيم** كانت رسالته تقول خطأً "البريد الإلكتروني مستخدم مسبقاً"
(كوبي من السطر التاني). انصلح لتقول "اسم المستخدم مستخدم مسبقاً".

**قرار أمني مقصود بـ `LoginAsync`:** رسالة الخطأ لعدم وجود المستخدم ولخطأ الباسورد
**موحّدة** ("اسم المستخدم أو كلمة المرور غير صحيحة") — يمنع اكتشاف يوزرنيمات
موجودة فعلياً بالنظام (User Enumeration Attack).

---

### الخطوة 4 — API: توليد التوكن + Controller

- `Authentication/JwtSettings.cs`: POCO مطابق لقسم `Jwt` بـ appsettings.json.
- `Authentication/JwtTokenGenerator.cs` (تنفيذ `IJwtTokenGenerator`):
  ```csharp
  var claims = new List<Claim>
  {
      new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
      new Claim(ClaimTypes.Name, user.Username),
      new Claim(ClaimTypes.Role, user.Role.ToString())
  };
  if (user.StudentId.HasValue) claims.Add(new Claim("studentId", user.StudentId.Value.ToString()));
  if (user.TeacherId.HasValue) claims.Add(new Claim("teacherId", user.TeacherId.Value.ToString()));
  ```
  الـ Claims المخصصة `studentId`/`teacherId` هي **أساس فحص الـ Ownership** لاحقاً
  (خطوة 6).
- `Authentication/AuthController.cs`: 3 Endpoints —
  `POST /api/auth/register/student`, `POST /api/auth/register/teacher`,
  `POST /api/auth/login`.
- `Program.cs`: تسجيل `Configure<JwtSettings>` و`IJwtTokenGenerator`.

---

### الخطوة 5 — تفعيل الـ Pipeline (Program.cs)

```csharp
var jwtSettings = builder.Configuration.GetSection("Jwt").Get<JwtSettings>()!;

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = jwtSettings.Issuer,
            ValidateAudience = true,
            ValidAudience = jwtSettings.Audience,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Key)),
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero
        };
    });
```

وبالـ Pipeline (**الترتيب حرج جداً**):
```csharp
app.UseHttpsRedirection();
app.UseCors("AllowFrontend");
app.UseAuthentication();   // 🆕 هذا بالضبط السطر الناقص من نقطة البداية
app.UseAuthorization();
app.MapControllers();
```
`UseAuthentication` (يبني هوية المستخدم من التوكن) لازم قبل `UseAuthorization`
(يفحص هل مسموحله)، غير هيك كل شي يترفض دايماً.

---

### الخطوة 6 — زرع Admin + حماية كل الكونترولرز

**زرع الأدمن** (`Infrastructure/Seeding/DbSeeder.cs`):
```csharp
public static async Task SeedAdminUserAsync(AppDbContext context)
{
    if (await context.Users.AnyAsync(u => u.Role == UserRole.Admin)) return; // مرة وحدة بس

    var admin = new User
    {
        Username = "admin",
        Email = "admin@schoollearning.local",
        PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin@12345"),
        Role = UserRole.Admin,
        IsActive = true
    };
    context.Users.Add(admin);
    await context.SaveChangesAsync();
}
```
يُستدعى من `Program.cs` بجنب `SeedTestDataAsync` الموجودة أصلاً.

**حماية الكونترولرز — القاعدة العامة المطبّقة على كل الـ 13 كونترولر + AuthController:**

| الحالة | القرار |
|---|---|
| كونترولر بشكل عام | `[Authorize]` افتراضي فوق الكلاس |
| تصفح محتوى عام (Course/Lesson/Curriculum/Teacher GET) | `[AllowAnonymous]` |
| Add/Update/Delete وعمليات الإدارة | `[Authorize(Roles = "Admin,Teacher")]` |
| بيانات طالب شخصية (نتائج، جلسات، تقدّم) | `[Authorize(Roles="Admin,Teacher,Student")]` **+ فحص Ownership يدوي** |

**نمط فحص الـ Ownership** (مكرر بأكثر من 20 مكان بالمشروع):
```csharp
if (User.IsInRole("Student") && User.FindFirstValue("studentId") != studentId.ToString())
    return Forbid();
```

**🔍 اكتشاف أمني مهم أثناء الحماية:** `QuestionReadDto` و`ExerciseReadDto` يرجّعون
حقل **`Answer`** (الإجابة الصحيحة) **مباشرة بكل استجابة**. لو فتحناهم للطالب أو
حتى Anonymous، أي حد يشوف إجابة أي سؤال قبل ما يحاول يحلّه. **القرار:**
`QuestionController` و`ExerciseController` بالكامل مقيّدين لـ `Admin,Teacher` بس —
ولا حتى الطالب المسجّل يوصلهم. (تحسين مستقبلي مقترح: فصل DTO خاص بالطالب بدون
حقل Answer، بدل التقييد الكامل — لسا ما انسوّى).

**مسارات إدارية قديمة صارت مكرّرة:** `StudentController.Add` و`TeacherController.Add`
كانا يسمحان بإنشاء حساب طالب/معلم بدون باسورد (من قبل بناء الـ Auth). بعد ما صار
التسجيل الحقيقي عبر `/api/auth/register/*`، هذول الاثنين قُيّدا لـ `Admin` بس
(مسار إداري احتياطي، مو للتسجيل الذاتي).

**🐛 فجوة صارت وانصلحت (`MemorizeSessionController`):** `CompleteSession` و
`GetWithAnswers` يأخذان `id` تبع الجلسة نفسها، مو `studentId` — فما قدرنا نتحقق
"هل الجلسة تخص هذا الطالب" لأن `MemorizeSessionReadDto` ما كان فيها `StudentId`
(بس `StudentName`). **الحل المطبّق:**
1. أضيف `public int StudentId { get; set; }` لـ `MemorizeSessionReadDto`
   (AutoMapper طابقها تلقائياً بالاسم مع `MemorizeSession.StudentId` بدون أي
   `ForMember` إضافي).
2. `GetWithAnswers`: نجيب بيانات الجلسة أول، نتحقق `data.StudentId` يطابق الـ
   claim، بعدين نرجعها.
3. `CompleteSession`: نجيب الجلسة عبر `GetByIdAsync(id)` أول (404 لو مو موجودة)،
   نتحقق الملكية، وبعدين نستدعي `CompleteSessionAsync(id)` الفعلية.

---

## 4. التحقق الفعلي (End-to-End Testing)

شغّلنا التطبيق فعلياً (مو بس Build) واختبرنا بـ `curl` أكثر من مرة عبر رحلة البناء
كاملة. أهم النتائج المؤكدة فعلياً:

| الاختبار | النتيجة |
|---|---|
| `GET /api/student` بدون توكن | `401` |
| `GET /api/course` بدون توكن (AllowAnonymous) | `200` |
| Login بحساب Admin المزروع | توكن صحيح، `role: Admin` |
| تسجيل طالب/معلم جديد عبر `/api/auth/register/*` | `201` + توكن فوري |
| طالب يشوف بروفايله (`/api/student/{ownId}`) | `200` |
| نفس الطالب يحاول يشوف طالب ثاني | `403 Forbidden` |
| طالب يحاول `/api/exercise` أو `/api/question` | `403` (حتى بحساب مسجّل) |
| طالب يحاول ينشئ Course أو معلم جديد | `403` |
| تسجيل يوزرنيم مكرر | `400` + رسالة صحيحة ("اسم المستخدم مستخدم مسبقاً") |
| معلم يصل لـ QuestionController | `200` (Admin,Teacher مسموح) |

---

## 5. توثيق OpenAPI/Swagger (المرحلة الأخيرة)

بعد ما خلص نظام الحماية، أضفنا طبقة توثيق كاملة حتى الفرونت (وأي أداة ذكاء اصطناعي
تتعامل مع الـ API) تفهم كل Endpoint بدون ما تحتاج تسأل:

1. **زر Authorize بـ Swagger UI** (`Program.cs`):
   ```csharp
   c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme { ... });
   c.AddSecurityRequirement(new OpenApiSecurityRequirement { ... });
   ```
2. **`[Tags("...")]`** على الكونترولرز اللي فيها 8+ Endpoints (تجميع منطقي بواجهة
   Swagger: "0. الأساسيات (CRUD)"، "1. الاستعلام والعلاقات"، "2. الإحصائيات"، إلخ).
3. **XML Doc Comments كاملة على كل Endpoint بكل الـ 14 كونترولر:**
   - `<summary>`: وصف مختصر لغرض الـ Endpoint.
   - `<remarks>`: الصلاحيات المطلوبة بالضبط + **مثال JSON فعلي** للـ Request
     (بدل ما يخمّن المبرمج/الأداة شكل البيانات الصحيح).
   - `<response code="XXX">`: وصف يوضح **ليش** يصير هذا الكود بالتحديد (401 لعدم
     تسجيل الدخول، 403 لدور غير مسموح، 404 لعدم وجود المورد...).
   - `[ProducesResponseType(StatusCodes.Status401Unauthorized)]` و`403Forbidden`
     أضيفت لكل Endpoint محمي (كانت ناقصة تماماً من قبل).

تحقّقنا فعلياً إنه هذا التوثيق يطلع صحيح بملف `/swagger/v1/swagger.json` الناتج
(فحصناه مباشرة — الـ summaries، الـ remarks، الأمثلة، والـ Tags كلها موجودة).

---

## 6. مصفوفة الصلاحيات النهائية (كل Endpoint × الدور المطلوب)

### `AuthController` (`/api/auth`)
| Endpoint | Method | الصلاحيات |
|---|---|---|
| `register/student` | POST | عام (بدون تسجيل دخول) |
| `register/teacher` | POST | عام (بدون تسجيل دخول) |
| `login` | POST | عام (بدون تسجيل دخول) |

### `CourseController`, `LessonController`, `CurriculumController`, `TeacherController`
- **GET** (تصفح المحتوى العام): `AllowAnonymous`
- **Add/Update/Delete**: `Admin,Teacher` (`TeacherController.Add/Delete`: `Admin` بس)

### `ExamController`
- GET (بيانات وصفية): `AllowAnonymous`
- `{id}/questions`, `{id}/results`, Add/Update/Delete: `Admin,Teacher`

### `ExerciseController`, `QuestionController` (ملف `QuestionsController.cs`)
- **الكونترولر كامل**: `Admin,Teacher` بس (يكشف حقل `Answer`، ولا حتى Student)

### `ResultController`, `StudentAnswerDetailController`, `SrsController`, `MemorizeSessionController`, `CourseStudentController`
- عمليات "كل الطلاب مجتمعين" أو الإدارة: `Admin,Teacher`
- عمليات خاصة بطالب معيّن (`.../student/{studentId}/...`): `Admin,Teacher,Student`
  **+ فحص Ownership** (الطالب يشوف بس بياناته)

### `StudentController`
- `GetAll`, `GetByGradeLevel`, `Delete`: `Admin,Teacher`
- `Add`: `Admin` بس (مسار إداري قديم)
- `GetById`, `Update`, `GetWithProgress`: `Admin,Teacher,Student` + Ownership

---

## 7. حسابات الاختبار الفعلية (موجودة بقاعدة البيانات الآن)

| الدور | Username | Password |
|---|---|---|
| Admin | `admin` | `Admin@12345` |
| Teacher | `teach1` | `Pass123!` |
| Student | `stud1` | `Pass123!` |

⚠️ `test_student` (من `DbSeeder.SeedTestDataAsync` القديمة، قبل بناء الـ Auth) **ما
يقدر يسجّل دخول** — صف Student بدون حساب User/Password مرتبط فيه.

---

## 8. فجوات/تحسينات مستقبلية معروفة (لسا ما انسوّت)

| الشي | الحالة | لو تحتاجها لاحقاً |
|---|---|---|
| `UserRepository._context` حقل زايد يخفي حقل الأب | تحذير Build بسيط، غير مؤثر | احذف الحقل المكرر، استخدم الموروث من `GenericRepository<T>` |
| فصل DTO خاص بالطالب بدون `Answer` لـ Question/Exercise | الحل الحالي: تقييد كامل لـ Admin/Teacher | أنشئ `QuestionForStudentDto` بدون Answer، افتح Endpoint مخصص للطالب |
| Endpoint لإنشاء Admin إضافي | غير موجود عمداً | أضف `POST /api/auth/register/admin` بـ `[Authorize(Roles="Admin")]` |
| Refresh Token / تغيير الباسورد | غير موجود | يحتاج تصميم منفصل (Refresh Token storage, endpoint جديد) |
| قفل Swagger العام يظهر على كل Endpoint حتى AllowAnonymous | تجميلي بس، ما يأثر وظيفياً | تحسين اختياري لعرض القفل بس على المحمي فعلياً |

---

## 9. خلاصة الملفات المتأثرة (للمرجعية السريعة)

**جديدة بالكامل:**
```
Domain/Enums/UserRole.cs
Domain/Entities/User.cs
Domain/Interfaces/IUserRepository.cs
Infrastructure/Configurations/UserConfiguration.cs
Infrastructure/Repositories/UserRepository.cs
Infrastructure/Migrations/20260715191037_AddUsersTable.cs (+ .Designer.cs)
Applicationf/DTOs/Auth/{RegisterStudentDto,RegisterTeacherDto,LoginDto,AuthResponseDto}.cs
Applicationf/Interfaces/{IAuthService,IJwtTokenGenerator}.cs
Applicationf/Services/AuthService.cs
Applicationf/Validators/AuthValidator/{RegisterStudentDtoValidator,RegisterTeacherDtoValidator,LoginDtoValidator}.cs
API/Authentication/{AuthController,JwtSettings,JwtTokenGenerator}.cs   (JwtMiddleware.cs بقيت فاضية وغير مستخدمة)
```

**معدّلة:**
```
API.csproj, Applicationf.csproj, Infrastructure.csproj  (حزم NuGet)
appsettings.json                                        (قسم Jwt)
Infrastructure/Data/AppDbContext.cs                      (DbSet<User>)
Infrastructure/DependencyInjection.cs                    (تسجيل IUserRepository)
Infrastructure/Seeding/DbSeeder.cs                        (SeedAdminUserAsync)
Applicationf/DependencyInjection.cs                       (تسجيل IAuthService)
API/Program.cs                                           (JwtBearer + Pipeline + Swagger Security)
API/Controllers/*.cs                                      (كل الـ 13 كونترولر: [Authorize] + Ownership + توثيق كامل)
```

---

*هذا الملف يوثّق حالة النظام لحظة إنشائه. لو تمت تعديلات لاحقة على الصلاحيات أو
البنية، حدّث الجدول بقسم 6 والفجوات بقسم 8 حتى يضل الملف دقيق.*
