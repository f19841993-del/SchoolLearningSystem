using SchoolLearningSystem.API.Middleware;
using SchoolLearningSystem.Applicationf;
using SchoolLearningSystem.Infrastructure;
using Serilog;

namespace SchoolLearningSystem.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            // 1. ≈⁄œ«œ Ê ﬂÊÌ‰ Serilog
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information() //  ÕœÌœ «·„” ÊÏ «·«› —«÷Ì (Information ›„« ›Êﬁ)
                .WriteTo.Console() // ÿ»«⁄… «·”Ã·«  ›Ì ‘«‘… «·√Ê«„—
                .WriteTo.File("Logs/log.txt", rollingInterval: RollingInterval.Day) // «·√Â„: Õ›Ÿ «·”Ã·«  ›Ì „·›° Ê≈‰‘«¡ „·› ÃœÌœ ﬂ· ÌÊ„
                .CreateLogger();
            // 3. ≈Œ»«— «·‰Ÿ«„ »«” Œœ«„ Serilog
            builder.Host.UseSerilog();
            // 1.  ”ÃÌ· «·Œœ„« 
            builder.Services.AddControllers();
            builder.Services.AddApplicationServices();
            builder.Services.AddInfrastructureServices(builder.Configuration);

            // 2.  ”ÃÌ· Swagger
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });

            var app = builder.Build();

            // 3.  ÂÌ∆… «·‹ Pipeline («· — Ì» Â‰« ÕÌÊÌ Ãœ«)

            // ÌÃ» √‰ ÌﬂÊ‰ «·‹ ExceptionMiddleware ›Ì √Ê· «·”·”·…
            // ·Ì· ﬁÿ √Ì Œÿ√ ÌÕœÀ ›Ì √Ì „—Õ·… ·«Õﬁ…
            app.UseMiddleware<ExceptionMiddleware>();

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