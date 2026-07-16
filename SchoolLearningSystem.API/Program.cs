using SchoolLearningSystem.API.Authentication;
using SchoolLearningSystem.API.Extensions;
using SchoolLearningSystem.API.Middleware;
using SchoolLearningSystem.API.UploadHandling;
using SchoolLearningSystem.Applicationf;
using SchoolLearningSystem.Applicationf.Interfaces;
using SchoolLearningSystem.Infrastructure;
using SchoolLearningSystem.Infrastructure.Data;
using SchoolLearningSystem.Infrastructure.Seeding;
using Serilog;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
namespace SchoolLearningSystem.API
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            // 1. إعداد وتكوين Serilog
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information() // تحديد المستوى الافتراضي (Information فما فوق)
                .WriteTo.Console() // طباعة السجلات في شاشة الأوامر
                .WriteTo.File("Logs/log.txt", rollingInterval: RollingInterval.Day) // الأهم: حفظ السجلات في ملف، وإنشاء ملف جديد كل يوم
                .CreateLogger();
            // 3. إخبار النظام باستخدام Serilog
            builder.Host.UseSerilog();
            // 1. تسجيل الخدمات
            builder.Services.AddControllers();
            builder.Services.AddApplicationServices();
            builder.Services.AddInfrastructureServices(builder.Configuration);
            builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("Jwt"));
            builder.Services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();
            builder.Services.AddHttpContextAccessor();
            builder.Services.AddScoped<IFileStorageService, FileStorageService>();
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

            // 🆕 تسجيل سياسة CORS: تسمح للفرونت (Origin مختلف) يستدعي الـ API من المتصفح.
            // بدونها، المتصفح يمنع كل الطلبات تلقائياً (CORS Policy Error) حتى لو الـ API شغال صح.
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowFrontend", policy =>
                {
                    policy.WithOrigins(
                            "http://localhost:3000",   // 👈 عدّل هذا لبورت الفرونت الفعلي عندك
                            "http://localhost:5173")    // (مثال: Vite افتراضياً يشتغل على 5173)
                          .AllowAnyHeader()
                          .AllowAnyMethod();
                });
            });

            // 2. تسجيل Swagger
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);

                // 🆕 زر "Authorize" بواجهة Swagger: يخلي تختبر الـ Endpoints المحمية
                // مباشرة من المتصفح بلصق التوكن الراجع من /api/auth/login
                c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = Microsoft.OpenApi.Models.ParameterLocation.Header,
                    Description = "ألصق التوكن هنا بدون كلمة \"Bearer\" - سويجر يضيفها تلقائياً."
                });

                c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
                {
                    {
                        new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                        {
                            Reference = new Microsoft.OpenApi.Models.OpenApiReference
                            {
                                Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
                });
            });

            var app = builder.Build();
            // 🌱 Seed Data تجريبية — يشتغل مرة وحدة بس (محمي بالتحقق داخل الدالة نفسها)
            using (var scope = app.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                await DbSeeder.SeedTestDataAsync(context);
                await DbSeeder.SeedAdminUserAsync(context);   // 🆕
            }
            // 3. تهيئة الـ Pipeline (الترتيب هنا حيوي جداً)

            // يجب أن يكون الـ ExceptionMiddleware في أول السلسلة
            // ليلتقط أي خطأ يحدث في أي مرحلة لاحقة
            app.UseCustomExceptionMiddleware();  // الـ Extension Method اللي بنيتها

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles(); // 🆕 يخدم wwwroot/uploads (صور البروفايل/الكورس) كملفات ثابتة
            app.UseCors("AllowFrontend");
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers();

            app.Run();
        }
    }
}