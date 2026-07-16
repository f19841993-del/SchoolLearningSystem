using SchoolLearningSystem.Applicationf.Exceptions;

namespace SchoolLearningSystem.API.UploadHandling
{
    public class FileStorageService : IFileStorageService
    {
        private static readonly string[] AllowedExtensions = { ".jpg", ".jpeg", ".png", ".webp", ".gif" };
        private const long MaxFileSizeBytes = 5 * 1024 * 1024; // 5 ميجابايت

        private readonly IWebHostEnvironment _env;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public FileStorageService(IWebHostEnvironment env, IHttpContextAccessor httpContextAccessor)
        {
            _env = env;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<string> SaveImageAsync(IFormFile file, string subFolder)
        {
            if (file == null || file.Length == 0)
                throw new BadRequestException("الملف فارغ أو غير مرفق.");

            if (file.Length > MaxFileSizeBytes)
                throw new BadRequestException("حجم الملف يتجاوز الحد الأقصى المسموح (5 ميجابايت).");

            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
            if (!AllowedExtensions.Contains(extension))
                throw new BadRequestException($"صيغة الملف غير مسموحة. الصيغ المسموحة: {string.Join(", ", AllowedExtensions)}");

            var folderPath = Path.Combine(WebRootPath, "uploads", subFolder);
            Directory.CreateDirectory(folderPath);

            var fileName = $"{Guid.NewGuid()}{extension}";
            var filePath = Path.Combine(folderPath, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            var request = _httpContextAccessor.HttpContext!.Request;
            return $"{request.Scheme}://{request.Host}/uploads/{subFolder}/{fileName}";
        }

        public void DeleteImage(string? url)
        {
            // نستخرج المسار (Path) فقط بغض النظر إن كان الرابط كاملاً أو نسبياً،
            // ثم نتجاهل بصمت أي رابط خارجي (غير /uploads/) أو فارغ - ليس مسؤوليتنا حذفه
            var path = Uri.TryCreate(url, UriKind.Absolute, out var absolute) ? absolute.AbsolutePath : url;
            if (string.IsNullOrWhiteSpace(path) || !path.StartsWith("/uploads/"))
                return;

            var physicalPath = Path.Combine(
                WebRootPath,
                path.TrimStart('/').Replace('/', Path.DirectorySeparatorChar));

            if (File.Exists(physicalPath))
                File.Delete(physicalPath);
        }

        // بعض بيئات الاستضافة لا تُنشئ wwwroot تلقائياً إذا كانت فارغة عند أول تشغيل
        private string WebRootPath =>
            string.IsNullOrEmpty(_env.WebRootPath)
                ? Path.Combine(_env.ContentRootPath, "wwwroot")
                : _env.WebRootPath;
    }
}
