using SchoolLearningSystem.API.Extensions;
using SchoolLearningSystem.API.Middleware;
using SchoolLearningSystem.Applicationf;
using SchoolLearningSystem.Infrastructure;
using SchoolLearningSystem.Infrastructure.Data;
using SchoolLearningSystem.Infrastructure.Seeding;
using Serilog;

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

            // 2. تسجيل Swagger
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });

            var app = builder.Build();
            // 🌱 Seed Data تجريبية — يشتغل مرة وحدة بس (محمي بالتحقق داخل الدالة نفسها)
            using (var scope = app.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                await DbSeeder.SeedTestDataAsync(context);
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
            app.UseAuthorization();
            app.MapControllers();

            app.Run();
        }
    }
}