using System.ComponentModel.DataAnnotations;

namespace SchoolLearningSystem.Applicationf.DTOs.Teacher
{
    public class TeacherCreateDto
    {
        public string Name { get; set; } = string.Empty;
        public string? Bio { get; set; }

        // 💡 Subject حُذف من هنا عمداً: موثّق بالمشروع كـ "ثابتة حالياً على Math".
        // يُثبَّت من الـ Service (teacher.Subject = "Math") ولا يُترك قابلاً
        // للتغيير من الـ Client، تفادياً لكسر هذا الافتراض بالخطأ.
        // يظهر فقط بـ TeacherReadDto للعرض.
    }
}